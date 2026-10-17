using UnityEngine;

public class InicioMuevete : MonoBehaviour
{
    public GameObject panelInicio;
    public GameObject textoEjercicio;
    public GameObject barraProgreso;

    private void Start()
    {
        textoEjercicio.SetActive(false);
        barraProgreso.SetActive(false);
    }

    public void Comenzar()
    {
        panelInicio.SetActive(false);

        textoEjercicio.SetActive(true);
        barraProgreso.SetActive(true);
    }
}