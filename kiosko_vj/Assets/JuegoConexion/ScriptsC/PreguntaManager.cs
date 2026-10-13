using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class PreguntaManager : MonoBehaviour
{
   public GameManagerConexion gameManager;

   public GameObject[] oficinas;

   public List<GameManagerConexion.Question> preguntas;

   public TMP_InputField pregInputTexto;

   public string inputDepartamento;

   public GameObject panelPreguntas;

   public Jugador jugador;

    void Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();
    }

    public void AdjudicarPreguntaAOficina(GameManagerConexion.Question pregunta)
    {
        var rndIndx = Random.Range(0, oficinas.Length);
        oficinas[rndIndx].GetComponent<Oficina>().AgregarPregunta(pregunta);
    }

    public void finanzasClick()
    {
        inputDepartamento = "Finanzas";
    }

    public void rrhhClick()
    {
        inputDepartamento = "RRHH";
    }

    public void marketingClick()
    {
        inputDepartamento = "Marketing";
    }

    public void ventasClick()
    {
        inputDepartamento = "Ventas";
    }

    public void atencionClick()
    {
        inputDepartamento = "Atencion";
    }

    public void direccionClick()
    {
        inputDepartamento = "Direccion";
    }


    public void MostrarPanelPreguntas()
    {
        panelPreguntas.SetActive(true);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
    }

   public void CrearPregunta()
    {
        GameManagerConexion.Question nuevaPregunta = new GameManagerConexion.Question(pregInputTexto.text, inputDepartamento);
        preguntas.Add(nuevaPregunta);
        AdjudicarPreguntaAOficina(nuevaPregunta);
       
    }

    public void CerrarPanelPreguntas()
    {
        panelPreguntas.SetActive(false);
        jugador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
    }
}
