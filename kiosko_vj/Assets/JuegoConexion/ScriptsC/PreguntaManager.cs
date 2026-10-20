using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Collections;




public class PreguntaManager : MonoBehaviour
{
    public GameManagerConexion gameManager;

    public GameObject[] oficinas;

    public List<GameManagerConexion.Question> preguntas;

    public TMP_InputField pregInputTexto;

    public string inputDepartamento;

    public GameObject panelPreguntas;

    public GameObject panelMisPreguntas;

    public GameObject panelRespuesta;

    public GameObject preguntaPrefab;

    public GameObject panelAvisos;


    public Jugador jugador;

    public TMP_Dropdown dropdown;
    public TMP_Text textoEnunciado;
    public TMP_Text textoRespuesta;

    public static PreguntaManager Instancia;

    public GameObject prefabPanel;     
    public Transform canvas;    

     

    GameObject panelActual;

    void Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();


    }



    public void finanzasClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoFinanzas";
    }

    public void rrhhClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoRRHH";
    }

    public void marketingClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoMarketing";
    }

    public void ventasClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoVentas";
    }

    public void atencionClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoAtencion";
    }

    public void direccionClick()
    {
        gameManager.ReproducirSFX("BotonClick");
        inputDepartamento = "DepartamentoDireccion";
    }


    public void MostrarPanelPreguntas()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelPreguntas.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }

   public void CrearPregunta()
    {
        gameManager.ReproducirSFX("BotonClick");

        gameManager.StartCoroutine(CrearPreguntaCoroutine( pregInputTexto.text, inputDepartamento));


        panelPreguntas.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    IEnumerator CrearPreguntaCoroutine(string texto, string depto)
    {
        string oficinaPreferida = oficinas[Random.Range(0, oficinas.Length)].name;

        yield return ApiClient.CrearPregunta(
            gameManager.jugadorId, depto, texto, oficinaPreferida,

            r =>
            {
            
                GameObject objOficina = System.Array.Find(oficinas, o => o.name == r.oficinaId);
                if (objOficina == null) { Debug.LogError("Oficina no encontrada: " + r.oficinaId); return; }
                var oficina = objOficina.GetComponent<Oficina>();

                var nuevaPregunta = new GameManagerConexion.Question(
                    texto, depto, gameManager.usuario.nombre, r.oficinaId, r.slotIndex);
                nuevaPregunta.id = r.id;

                if (nuevaPregunta.relatedDepartment == gameManager.escenaActual)
                    nuevaPregunta.Aparecer(preguntaPrefab, oficina.puntos[r.slotIndex]);

                oficina.AgregarPregunta(nuevaPregunta);
            },

            () => Debug.Log("Oficinas llenas: hay que responder una pregunta antes de crear otra"));
    }

    public void EnviarRespuesta(GameManagerConexion.Question pregunta)
    {
        gameManager.StartCoroutine(EnviarRespuestaCoroutine(pregunta));
    }

    IEnumerator EnviarRespuestaCoroutine(GameManagerConexion.Question pregunta)
    {
      yield return ApiClient.Responder(pregunta.id, gameManager.jugadorId, pregunta.response, () =>
        {
            pregunta.responsePlayerName = gameManager.usuario.nombre;

            var oficina = oficinas.FirstOrDefault(o => o.name == pregunta.oficinaId)?.GetComponent<Oficina>();
            if (oficina != null)
                oficina.Liberar(oficina.puntos[pregunta.slotIndex]);

            if (pregunta.relatedPrefab != null)
                Destroy(pregunta.relatedPrefab);
            else
                foreach (var po in FindObjectsByType<Pregunta>())
                    if (po.preguntaData.id == pregunta.id)
                        Destroy(po.gameObject);

            gameManager.usuario.puntos += 10;
        });
    }

    public void CerrarPanelRespuesta()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelActual.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

   public void AbrirPanelMisPreguntas()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelMisPreguntas.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        gameManager.StartCoroutine(CargarMisPreguntas());
    }

    IEnumerator CargarMisPreguntas()
    {
        yield return ApiClient.ListarPreguntas(lista =>
        {
            var mias = new List<GameManagerConexion.Question>();
            foreach (var d in lista)
                if (d.relatedPlayerName == gameManager.usuario.nombre)
                    mias.Add(GameManagerConexion.DesdeDTO(d));

            Cargar(mias);
        });
    }

    public void CerrarPanelPreguntas()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelPreguntas.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

    }

    public void CerrarPanelMisPreguntas()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelMisPreguntas.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

    }





    public void Cargar(List<GameManagerConexion.Question> lista)
    {
        preguntas = lista;

        dropdown.ClearOptions();
        List<string> titulos = new List<string>();
        foreach (GameManagerConexion.Question p in preguntas)
            titulos.Add(p.question);
        dropdown.AddOptions(titulos);

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(Mostrar);

        if (preguntas.Count > 0)
            Mostrar(0);
        else
        {
            textoEnunciado.text = "No has creado ninguna pregunta todavía";
            textoRespuesta.text = "";
        }
    }

    void Mostrar(int indice)
    {
        if (indice < 0 || indice >= preguntas.Count) return;

        GameManagerConexion.Question p = preguntas[indice];
        textoEnunciado.text = p.question;
        textoRespuesta.text = p.response;
    }

    void Awake() { Instancia = this; }

    public void MostrarPanel(GameManagerConexion.Question p)
    {
        if (panelActual != null) return;
        if(p.relatedPlayerName == gameManager.usuario.nombre)
        {
            MostrarPanelAvisos("Esta pregunta ha sido creada por tí, no puedes autoresponderte");
            return;
        }
        
        

        panelActual = Instantiate(prefabPanel, canvas);   
        panelActual.GetComponent<PanelRespuesta>().Inicializar(p);
        panelActual.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;    
    }

    public void MostrarPanelAvisos(string aviso)
    {
        panelAvisos.SetActive(true);
        panelAvisos.GetComponentInChildren<TMP_Text>().text = aviso;
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;    
    }

    public void CerrarPanelAvisos()
    {
        gameManager.ReproducirSFX("BotonClick");
        panelAvisos.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void PanelCerrado()
    {
        panelActual = null;
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; 
    }

   public void buttonMatchMaking()
    {
        if (panelActual != null) return;
        gameManager.ReproducirSFX("BotonClick");
        gameManager.StartCoroutine(MatchmakingCoroutine());
    }

    IEnumerator MatchmakingCoroutine()
    {
        yield return ApiClient.Matchmaking(
            gameManager.jugadorId,
            d => MostrarPanel(GameManagerConexion.DesdeDTO(d)),
            () => MostrarPanelAvisos("No hay preguntas disponibles ahora"));
    }
}
