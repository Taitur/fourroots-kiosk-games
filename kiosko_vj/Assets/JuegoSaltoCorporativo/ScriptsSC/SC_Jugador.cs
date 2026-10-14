using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Movimiento del jugador: A/D o flechas para moverse, J para saltar, K para dash, L para agarrarse a paredes.
// Tambien funciona con gamepad (stick izquierdo, boton sur salta, boton oeste dash, R1 agarra).
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SC_Jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 12.5f;
    public float tiempoCoyote = 0.1f;
    public float tiempoBufferSalto = 0.12f;
    public float multiplicadorSaltoCorto = 2f;

    [Header("Suelo")]
    public LayerMask capaSuelo = ~0;
    public Vector2 centroChequeoSuelo = new Vector2(0f, -0.03f);
    public Vector2 tamanoChequeoSuelo = new Vector2(0.5f, 0.1f);

    [Header("Pared (L para agarrarse)")]
    public float velocidadDeslizar = 2.5f;
    public float velocidadTrepar = 3.5f;
    public Vector2 saltoPared = new Vector2(7f, 12f);
    public float bloqueoSaltoPared = 0.18f;
    public Vector2 centroChequeoPared = new Vector2(0.38f, 0.6f);
    public Vector2 tamanoChequeoPared = new Vector2(0.12f, 0.8f);

    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoAterrizaje;   // al caer al suelo y al agarrarse a una pared
    public AudioClip[] sonidosDash;      // se elige uno al azar en cada dash
    [Range(0f, 1f)] public float volumenEfectos = 0.8f;

    [Header("Dash (K)")]
    public float velocidadDash = 20f;
    public float duracionDash = 0.15f;
    public float velocidadFinalDash = 6f;
    public float cooldownDashTiempo = 0.2f;

    [Header("Muerte")]
    public string tagPeligro = "Peligro";
    public float margenCaida = 1.5f;
    public float tiempoExplosion = 0.55f;

    static readonly int HashMoveX = Animator.StringToHash("MoveX");
    static readonly int HashVelY = Animator.StringToHash("VelY");
    static readonly int HashIsGrounded = Animator.StringToHash("IsGrounded");
    static readonly int HashFacing = Animator.StringToHash("Facing");
    static readonly int HashMuerto = Animator.StringToHash("Muerto");
    static readonly int HashAgarrado = Animator.StringToHash("Agarrado");
    static readonly int HashDeslizando = Animator.StringToHash("Deslizando");
    static readonly int HashTrepY = Animator.StringToHash("TrepY");
    static readonly int HashDash = Animator.StringToHash("Dash");
    static readonly int HashDashY = Animator.StringToHash("DashY");

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    ContactFilter2D filtro;
    readonly Collider2D[] resultados = new Collider2D[8];

    float h;
    bool saltoPresionado;
    bool saltoSostenido;
    float coyote;
    float buffer;
    bool enSuelo;
    int mirando; // 0 = de frente (inicio), -1 = izquierda, 1 = derecha

    AudioSource audioFx;
    bool enSueloPrev = true;
    float tiempoEnAire;
    bool agarradoPrev;
    float velYPrev;
    bool agarrarPresionado;
    bool dashPresionado;
    bool dashing;
    bool dashDisponible = true;
    float tiempoDash;
    float cooldownDash;
    Vector2 dirDash;
    int dashY;
    bool saltoBoton;
    float vInput;
    int paredDir;       // -1 pared a la izquierda, 1 a la derecha, 0 ninguna
    bool agarrado;
    bool deslizando;
    float bloqueoPared;
    int dirSaltoPared;
    float gravedadBase;

    bool muerto;
    SC_Cuarto cuartoActual;
    Vector3 puntoReaparicion;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        gravedadBase = rb.gravityScale;
        audioFx = GetComponent<AudioSource>();
        if (audioFx == null) audioFx = gameObject.AddComponent<AudioSource>();
        audioFx.playOnAwake = false;
        audioFx.spatialBlend = 0f;
        filtro = new ContactFilter2D();
        filtro.useTriggers = false;
        filtro.SetLayerMask(capaSuelo);

#if UNITY_EDITOR
        // En el editor, el teclado solo llega al juego si la ventana Game tiene el foco.
        // Esto hace que funcione aunque no hayas hecho clic dentro de la ventana Game.
        InputSystem.settings.editorInputBehaviorInPlayMode =
            InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
        Debug.Log("SC_Jugador listo. Teclado detectado: " + (Keyboard.current != null));
    }

    void Update()
    {
        if (muerto) return;

        ActualizarCuarto();
        if (CayoFueraDelNivel())
        {
            Morir();
            return;
        }

        LeerInput();

        if (h > 0.1f) mirando = 1;
        else if (h < -0.1f) mirando = -1;

        if (bloqueoPared > 0f) bloqueoPared -= Time.deltaTime;
        bool bloqueado = bloqueoPared > 0f;

        enSuelo = ChequearSuelo();
        paredDir = ChequearPared();

        // Cada vez que vuelve a tocar el suelo (corriendo o quieto) suena el aterrizaje.
        // Se pide un instante minimo en el aire para ignorar parpadeos del chequeo de suelo.
        if (!enSuelo) tiempoEnAire += Time.deltaTime;
        if (enSuelo && !enSueloPrev && tiempoEnAire > 0.05f) Sonar(sonidoAterrizaje);
        if (enSuelo) tiempoEnAire = 0f;
        enSueloPrev = enSuelo;

        agarrado = agarrarPresionado && paredDir != 0 && !bloqueado;
        deslizando = !agarrado && !enSuelo && paredDir != 0 && !bloqueado
                     && h * paredDir > 0.1f && rb.linearVelocity.y <= 0f;

        if (agarrado && !agarradoPrev) Sonar(sonidoAterrizaje);
        agarradoPrev = agarrado;

        // Dash: un solo dash en el aire, se recarga al tocar el suelo
        if (cooldownDash > 0f) cooldownDash -= Time.deltaTime;
        if (enSuelo && !dashing && cooldownDash <= 0f) dashDisponible = true;
        if (dashPresionado && dashDisponible && !dashing) IniciarDash();

        if (dashing)
        {
            tiempoDash -= Time.deltaTime;
            if (tiempoDash > 0f)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = dirDash * velocidadDash;
                velYPrev = 0f;
                anim.SetBool(HashDash, true);
                anim.SetInteger(HashDashY, dashY);
                anim.SetBool(HashAgarrado, false);
                anim.SetBool(HashDeslizando, false);
                if (sprite != null) sprite.flipX = Mathf.Abs(dirDash.x) > 0.3f ? dirDash.x < 0f : mirando == -1;
                return;
            }
            TerminarDash();
        }

        if (agarrado) saltoPresionado = saltoBoton;

        if (enSuelo) coyote = tiempoCoyote; else coyote -= Time.deltaTime;
        if (saltoPresionado) buffer = tiempoBufferSalto; else buffer -= Time.deltaTime;

        Vector2 v = rb.linearVelocity;
        v.x = bloqueado ? dirSaltoPared * saltoPared.x : h * velocidad;

        if (saltoPresionado && !enSuelo && paredDir != 0 && !bloqueado)
        {
            // Salto de pared: sale hacia el lado contrario a la pared
            dirSaltoPared = -paredDir;
            v = new Vector2(dirSaltoPared * saltoPared.x, saltoPared.y);
            Sonar(sonidoSalto);
            bloqueoPared = bloqueoSaltoPared;
            mirando = dirSaltoPared;
            buffer = 0f;
            coyote = 0f;
            agarrado = false;
            deslizando = false;
        }
        else if (buffer > 0f && coyote > 0f)
        {
            v.y = fuerzaSalto;
            Sonar(sonidoSalto);
            buffer = 0f;
            coyote = 0f;
            enSuelo = false;
        }
        else if (agarrado)
        {
            v = new Vector2(0f, vInput * velocidadTrepar);
        }
        else
        {
            if (deslizando && v.y < -velocidadDeslizar) v.y = -velocidadDeslizar;
            if (!saltoSostenido && v.y > 0f && !bloqueado)
            {
                // Salto corto si sueltas el boton antes
                v.y += Physics2D.gravity.y * rb.gravityScale * (multiplicadorSaltoCorto - 1f) * Time.deltaTime;
            }
        }

        rb.gravityScale = agarrado ? 0f : gravedadBase;
        rb.linearVelocity = v;
        velYPrev = v.y;

        if (agarrado || deslizando) mirando = paredDir;

        anim.SetFloat(HashMoveX, h);
        anim.SetFloat(HashVelY, v.y);
        anim.SetBool(HashIsGrounded, enSuelo);
        anim.SetInteger(HashFacing, mirando);
        anim.SetBool(HashAgarrado, agarrado);
        anim.SetBool(HashDeslizando, deslizando);
        anim.SetFloat(HashTrepY, agarrado ? vInput : 0f);

        if (sprite != null)
        {
            if (agarrado || deslizando)
            {
                // Los clips de pared estan dibujados con la pared a la derecha
                sprite.flipX = paredDir == -1;
            }
            else
            {
                // Idle y caida son clips de frente: se voltean si el personaje miraba a la izquierda
                bool cayendo = !enSuelo && v.y < -0.1f;
                bool quieto = enSuelo && Mathf.Abs(h) <= 0.1f;
                sprite.flipX = mirando == -1 && (cayendo || quieto);
            }
        }
    }

    // Al entrar a un cuarto se guarda donde reaparecer: su punto de reaparicion si tiene uno,
    // y si no, el lugar por donde entro el jugador.
    void ActualizarCuarto()
    {
        Vector2 pos = transform.position;
        foreach (SC_Cuarto c in SC_Cuarto.Todos)
        {
            if (!c.Limites.Contains(pos)) continue;
            if (c != cuartoActual)
            {
                cuartoActual = c;
                puntoReaparicion = c.reaparicion != null ? c.reaparicion.position : transform.position;
            }
            return;
        }
    }

    bool CayoFueraDelNivel()
    {
        float limite = -15f;
        if (SC_Cuarto.Todos.Count > 0)
        {
            float menor = float.MaxValue;
            foreach (SC_Cuarto c in SC_Cuarto.Todos) menor = Mathf.Min(menor, c.Limites.yMin);
            limite = menor - margenCaida;
        }
        return transform.position.y < limite;
    }

    public void Morir()
    {
        if (muerto) return;
        muerto = true;
        h = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        if (sprite != null) sprite.flipX = false;
        agarrado = false;
        deslizando = false;
        dashing = false;
        rb.gravityScale = gravedadBase;
        anim.SetBool(HashDash, false);
        anim.SetBool(HashAgarrado, false);
        anim.SetBool(HashDeslizando, false);
        anim.SetBool(HashMuerto, true);
        StartCoroutine(Reaparecer());
    }

    IEnumerator Reaparecer()
    {
        yield return new WaitForSeconds(tiempoExplosion);

        transform.position = puntoReaparicion;
        rb.position = puntoReaparicion;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = true;
        coyote = 0f;
        buffer = 0f;
        dashDisponible = true;
        cooldownDash = 0f;
        velYPrev = 0f;
        enSueloPrev = true;
        tiempoEnAire = 0f;
        agarradoPrev = false;
        anim.SetFloat(HashVelY, 0f);
        anim.SetBool(HashIsGrounded, true);
        anim.SetBool(HashMuerto, false);
        muerto = false;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.gameObject.tag == tagPeligro) Morir();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == tagPeligro) Morir();
    }

    void LeerInput()
    {
        h = 0f;
        vInput = 0f;
        saltoPresionado = false;
        saltoSostenido = false;
        saltoBoton = false;
        agarrarPresionado = false;
        dashPresionado = false;

        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) h -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
            if (kb.lKey.isPressed) agarrarPresionado = true;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) vInput += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) vInput -= 1f;
            if (kb.jKey.wasPressedThisFrame) { saltoBoton = true; saltoPresionado = true; }
            if (kb.jKey.isPressed) saltoSostenido = true;
            if (kb.kKey.wasPressedThisFrame) dashPresionado = true;
        }

        Gamepad gp = Gamepad.current;
        if (gp != null)
        {
            float x = gp.leftStick.ReadValue().x;
            if (Mathf.Abs(x) > 0.2f) h += x;
            float y = gp.leftStick.ReadValue().y;
            if (Mathf.Abs(y) > 0.2f) vInput += y;
            if (gp.rightShoulder.isPressed || gp.rightTrigger.isPressed) agarrarPresionado = true;
            if (gp.buttonSouth.wasPressedThisFrame) saltoBoton = true;
            if (gp.buttonSouth.wasPressedThisFrame) saltoPresionado = true;
            if (gp.buttonSouth.isPressed) saltoSostenido = true;
            if (gp.buttonWest.wasPressedThisFrame) dashPresionado = true;
        }

        h = Mathf.Clamp(h, -1f, 1f);
        vInput = Mathf.Clamp(vInput, -1f, 1f);
    }

    void Sonar(AudioClip clip)
    {
        if (clip != null && audioFx != null) audioFx.PlayOneShot(clip, volumenEfectos);
    }

    void IniciarDash()
    {
        Vector2 d = new Vector2(h, vInput);
        if (d.sqrMagnitude < 0.01f)
        {
            // Sin direccion: hacia donde mira, o alejandose de la pared si estaba pegado a una
            float lado = (agarrado || deslizando) ? -paredDir : (mirando != 0 ? mirando : 1);
            d = new Vector2(lado, 0f);
        }
        dirDash = d.normalized;
        dashY = dirDash.y > 0.5f ? 1 : (dirDash.y < -0.5f ? -1 : 0);
        if (Mathf.Abs(dirDash.x) > 0.3f) mirando = dirDash.x > 0f ? 1 : -1;
        if (sonidosDash != null && sonidosDash.Length > 0)
            Sonar(sonidosDash[Random.Range(0, sonidosDash.Length)]);
        dashing = true;
        dashDisponible = false;
        tiempoDash = duracionDash;
        bloqueoPared = 0f;
        agarrado = false;
        deslizando = false;
    }

    void TerminarDash()
    {
        dashing = false;
        cooldownDash = cooldownDashTiempo;
        rb.linearVelocity = dirDash * velocidadFinalDash;
        anim.SetBool(HashDash, false);
    }

    // Detecta una pared pegada al costado del personaje (a la altura del cuerpo)
    int ChequearPared()
    {
        bool izq = HayColisionLateral(-1);
        bool der = HayColisionLateral(1);
        if (izq && der) return mirando != 0 ? mirando : 1;
        if (izq) return -1;
        if (der) return 1;
        return 0;
    }

    bool HayColisionLateral(int lado)
    {
        Vector2 centro = (Vector2)transform.position + new Vector2(centroChequeoPared.x * lado, centroChequeoPared.y);
        int n = Physics2D.OverlapBox(centro, tamanoChequeoPared, 0f, filtro, resultados);
        for (int i = 0; i < n; i++)
        {
            if (resultados[i] != null && resultados[i].attachedRigidbody != rb) return true;
        }
        return false;
    }

    bool ChequearSuelo()
    {
        Vector2 centro = (Vector2)transform.position + centroChequeoSuelo;
        int n = Physics2D.OverlapBox(centro, tamanoChequeoSuelo, 0f, filtro, resultados);
        for (int i = 0; i < n; i++)
        {
            if (resultados[i] != null && resultados[i].attachedRigidbody != rb) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + centroChequeoSuelo, tamanoChequeoSuelo);
        Gizmos.color = Color.cyan;
        for (int lado = -1; lado <= 1; lado += 2)
            Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(centroChequeoPared.x * lado, centroChequeoPared.y), tamanoChequeoPared);
    }
}
