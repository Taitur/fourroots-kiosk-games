using UnityEngine;

public class Jugador : MonoBehaviour
{
    Animator anim;
    Rigidbody2D cuerpo;
    AudioSource audioSource;
    float velocidad = 5f;

    bool piso;
    public Transform pies;
    public LayerMask listaPiso;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        cuerpo = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("SFX/PasosJugador");
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        if(cuerpo.bodyType == RigidbodyType2D.Dynamic)
        {
            Vector2 direccion = new Vector2(horizontal, vertical).normalized;
            cuerpo.linearVelocity = direccion * velocidad;
            if (horizontal < 0)
                anim.Play("JugadorCaminaIzq");
            else if (horizontal > 0)
                anim.Play("JugadorCaminaDer");
            else if (vertical > 0)
                anim.Play("JugadorCaminaArriba");
            else if (vertical < 0)
                anim.Play("JugadorCaminaAbajo");
            else
                anim.Play("JugadorEspera");

            bool seMueve = direccion.sqrMagnitude > 0.01f;
            if (seMueve && !audioSource.isPlaying)
                audioSource.Play();
            else if (!seMueve && audioSource.isPlaying)
                audioSource.Stop();
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        }


}
