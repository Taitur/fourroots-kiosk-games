using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    GameManagerConexion GameManager;

    public GameObject explicacionIzq;

    public GameObject explicacionDer;

    public GameObject panelComoJugar;
    void Start()
    {
        GameManager = GameManagerConexion.instancia;
    }

    void ReproducirSonidoBoton()
    {
        GameManager.ReproducirSFX("BotonClick");
    }

    public void VolverAlMenu()
    {
        ReproducirSonidoBoton();
        GameManager.GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void EntrarAlForms()
    {
        ReproducirSonidoBoton();
        GameManager.GetComponent<AudioSource>().Play();
        
        SceneManager.LoadScene("MainForm");
    }
    public void EntrarAlJuego()
    {
        ReproducirSonidoBoton();
        if(GameManager.usuario.nombre != "" && GameManager.usuario.expertise != "" && GameManager.usuario.puesto!= "" && GameManager.usuario.usuarioDepartamento != "")
        {
           SceneManager.LoadScene(GameManager.usuario.usuarioDepartamento);
        }else
        {
            Debug.Log("Rellena todos los campos del formulario antes de entrar al juego");
        }
    }

    public void SalirAlMenuMinijuego()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("Menu");
    }

    public void EntrarRH()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoRRHH");
    }

    public void EntrarVentas()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoVentas");
    }

    public void EntrarFinanzas()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoFinanzas");
    }

    public void EntrarAtencion()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoAtencion");
    }

    public void EntrarMarketing()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoMarketing");
    }

    public void EntrarDireccion()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("DepartamentoDireccion");
    }

    public void MostrarExplicacionIzq()
    {
        if (!explicacionIzq.activeInHierarchy && explicacionDer.activeInHierarchy)
        {
            ReproducirSonidoBoton();
            explicacionDer.SetActive(false);
            explicacionIzq.SetActive(true);

        }
    }

    public void MostrarExplicacionDer()
    {
        if (!explicacionDer.activeInHierarchy && explicacionIzq.activeInHierarchy)
        {
            ReproducirSonidoBoton();
            explicacionIzq.SetActive(false);
            explicacionDer.SetActive(true);

        }
    }

    public void AbrirPanelComoJugar()
    {
        ReproducirSonidoBoton();
        panelComoJugar.SetActive(true);
    }
    public void CerrarPanelComoJugar()
    {
        ReproducirSonidoBoton();
        panelComoJugar.SetActive(false);
    }


    public void SalirDelMapa()
    {
        ReproducirSonidoBoton();
        if(GameManager.escenaAnterior != null)
        {
            SceneManager.LoadScene(GameManager.escenaAnterior);
        }

    }


}
