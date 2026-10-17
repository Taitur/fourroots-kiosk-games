using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Marcadores : MonoBehaviour
{
    public List<GameObject> marcadores;  
    GameManagerConexion GameManager;
    public List<GameObject> preguntas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   // MarcadorManager, en la escena del mapa
    IEnumerator Start()
    {
        GameManager = GameManagerConexion.instancia;
        if (GameManager.escenaAnterior == "DepartamentoRRHH")
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

        yield return ApiClient.ListarPreguntas(lista =>
        {
            bool Pendiente(string depto) =>
                lista.Any(d => d.relatedDepartment == depto && string.IsNullOrEmpty(d.response));

            preguntas.Find(m => m.name == "PreguntaRRHH").SetActive(Pendiente("DepartamentoRRHH"));
            preguntas.Find(m => m.name == "PreguntaVentas").SetActive(Pendiente("DepartamentoVentas"));
            preguntas.Find(m => m.name == "PreguntaDireccion").SetActive(Pendiente("DepartamentoDireccion"));
            preguntas.Find(m => m.name == "PreguntaFinanzas").SetActive(Pendiente("DepartamentoFinanzas"));
            preguntas.Find(m => m.name == "PreguntaAtencion").SetActive(Pendiente("DepartamentoAtencion"));
            preguntas.Find(m => m.name == "PreguntaMarketing").SetActive(Pendiente("DepartamentoMarketing"));
        });
    }

   
   
}
