using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;



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


    public Jugador jugador;

    public TMP_Dropdown dropdown;
    public TMP_Text textoEnunciado;
    public TMP_Text textoRespuesta;

    public static PreguntaManager Instancia;

    public GameObject prefabPanel;     
    public Transform canvas;          

    GameObject panelActual;       

    GameManagerConexion.PlayerData jugadorActual;

    void Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();
        jugadorActual = gameManager.gameData.jugadores.Find( jugador => jugador.nombre == gameManager.gameData.sesion.nombre);

    }



    public void finanzasClick()
    {
        inputDepartamento = "DepartamentoFinanzas";
    }

    public void rrhhClick()
    {
        inputDepartamento = "DepartamentoRRHH";
    }

    public void marketingClick()
    {
        inputDepartamento = "DepartamentoMarketing";
    }

    public void ventasClick()
    {
        inputDepartamento = "DepartamentoVentas";
    }

    public void atencionClick()
    {
        inputDepartamento = "DepartamentoAtencion";
    }

    public void direccionClick()
    {
        inputDepartamento = "DepartamentoDireccion";
    }


    public void MostrarPanelPreguntas()
    {
        panelPreguntas.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
    }

   public void CrearPregunta()
    {
        var rndIndx = Random.Range(0, oficinas.Length);
        var puntoIndx = 0;
        var oficinaSeleccionada = oficinas[rndIndx].GetComponent<Oficina>();

        for (int i = 0; i < oficinaSeleccionada.puntos.Length - 1; i++)
        {
            if(!oficinaSeleccionada.ocupado.Contains(oficinaSeleccionada.puntos[i]) ){
                puntoIndx = i;
                break;
            }   

        }

        GameManagerConexion.Question nuevaPregunta = new GameManagerConexion.Question(pregInputTexto.text, inputDepartamento, preguntaPrefab,oficinaSeleccionada.puntos[puntoIndx], jugadorActual.nombre, oficinaSeleccionada.gameObject.name, puntoIndx );
        jugadorActual.preguntas.Add(nuevaPregunta);
        
       
        oficinas[rndIndx].GetComponent<Oficina>().AgregarPregunta(nuevaPregunta);
        panelPreguntas.SetActive(false);       
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void EnviarRespuesta(GameManagerConexion.Question pregunta)
    {
        GameManagerConexion.PlayerData jugadorBuscado;
        var i = 0;
        jugadorBuscado = gameManager.gameData.jugadores.Find( jugador => jugador.nombre == pregunta.relatedPlayerName);
        i = jugadorBuscado.preguntas.FindIndex(q => q.question == pregunta.question);
        pregunta.responsePlayerName = jugadorActual.nombre;
        jugadorBuscado.preguntas[i] = pregunta;
        
    }

    public void CerrarPanelRespuesta()
    {
        panelActual.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void AbrirPanelMisPreguntas()
    {
        panelMisPreguntas.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; 
        
        if(jugadorActual.preguntas.Count > 0)
        {
             Cargar(jugadorActual.preguntas);
        }
       
       
    }

    public void CerrarPanelPreguntas()
    {
    
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
    }

    public void CerrarPanelMisPreguntas()
    {
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
        Mostrar(0);
    }

    void Mostrar(int indice)
    {
        GameManagerConexion.Question p = preguntas[indice];
        textoEnunciado.text = p.question;
        textoRespuesta.text = p.response;
    }

    void Awake() { Instancia = this; }

    public void MostrarPanel(GameManagerConexion.Question p)
    {
        if (panelActual != null) return;       
        

        panelActual = Instantiate(prefabPanel, canvas);   
        panelActual.GetComponent<PanelRespuesta>().Inicializar(p);
        panelActual.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;    
    }

    public void PanelCerrado()
    {
        panelActual = null;
    }

    public void buttonMatchMaking()
    {
        if (gameManager.BuscarPreguntaParaResponder(gameManager.usuario.nombre, gameManager.usuario.usuarioDepartamento, out var q))
            MostrarPanel(q);
        else
            Debug.Log("No hay preguntas disponibles ahora");
    }
}
