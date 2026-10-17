using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class Marcadores : MonoBehaviour
{
    public List<GameObject> marcadores;  
    GameManagerConexion GameManager;
    public List<GameObject> preguntas;

    List<GameManagerConexion.Question> pregRRHH;

    List<GameManagerConexion.Question> pregVentas;

    List<GameManagerConexion.Question> pregDireccion;

    List<GameManagerConexion.Question> pregFinan;

    List<GameManagerConexion.Question> pregAtencion;

    List<GameManagerConexion.Question> pregMarketing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   // MarcadorManager, en la escena del mapa
    void Start()
    {
        GameManager = GameManagerConexion.instancia;
        pregRRHH = new List<GameManagerConexion.Question>();
        pregVentas = new List<GameManagerConexion.Question>();
        pregFinan = new List<GameManagerConexion.Question>();
        pregDireccion = new List<GameManagerConexion.Question>();
        pregAtencion = new List<GameManagerConexion.Question>();
        pregMarketing = new List<GameManagerConexion.Question>();
        if (GameManager.escenaAnterior == "DepartamentoRH")
        {
            marcadores.Find(m => m.name == "MarcadorRRHH").SetActive(true);
        }
        else if(GameManager.escenaAnterior == "DepartamentoVentas")
        {
            marcadores.Find(m => m.name == "MarcadorVentas").SetActive(true);
        }
        else if (GameManager.escenaAnterior == "DepartamentoDireccion")
        {
            marcadores.Find(m => m.name == "MarcadorDireccion").SetActive(true);
        }
        else if (GameManager.escenaAnterior == "DepartamentoFinanzas")
        {
            marcadores.Find(m => m.name == "MarcadorFinanzas").SetActive(true);
        }
        else if (GameManager.escenaAnterior == "DepartamentoAtencion")
        {
            marcadores.Find(m => m.name == "MarcadorAtencion").SetActive(true);
        }
        else if (GameManager.escenaAnterior == "DepartamentoMarketing")
        {
            marcadores.Find(m => m.name == "MarcadorMarketing").SetActive(true);
        }

        foreach (GameManagerConexion.PlayerData p in GameManager.gameData.jugadores)
        {
            pregRRHH.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoRH"));
            pregVentas.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoVentas"));
            pregDireccion.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoDireccion"));
            pregFinan.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoFinanzas"));
            pregAtencion.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoAtencion"));
            pregMarketing.AddRange(p.preguntas.FindAll(q => q.relatedDepartment == "DepartamentoMarketing"));
        }

        if (pregRRHH.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaRRHH").SetActive(true);
        } 
         if (pregVentas.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaVentas").SetActive(true);
        } 
         if (pregDireccion.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaDireccion").SetActive(true);
        } 
        if (pregFinan.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaFinanzas").SetActive(true);
        } 
         if (pregAtencion.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaAtencion").SetActive(true);
        } 
         if (pregMarketing.Count > 0)
        {
            preguntas.Find(m => m.name == "PreguntaMarketing").SetActive(true);
        }
       
        
       
    }

   
   
}
