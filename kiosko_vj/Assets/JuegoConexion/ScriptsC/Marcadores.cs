using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Marcadores : MonoBehaviour
{
    public List<GameObject> marcadores;  
    GameManagerConexion GameManager;
    public List<GameObject> preguntas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   // MarcadorManager, en la escena del mapa
    void Start()
    {
        GameManager = GameManagerConexion.instancia;
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
       
        
       
    }

   
   
}
