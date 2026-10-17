using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    GameManagerConexion GameManager;

    void Start()
    {
        GameManager = GameManagerConexion.instancia;
    }
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void EntrarAlForms()
    {
        SceneManager.LoadScene("MainForm");
    }
    public void EntrarAlJuego()
    {
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
        SceneManager.LoadScene("Menu");
    }

    public void EntrarRH()
    {
        SceneManager.LoadScene("DepartamentoRH");
    }
    
    public void EntrarVentas()
    {
        SceneManager.LoadScene("DepartamentoVentas");
    }

    public void EntrarFinanzas()
    {
        SceneManager.LoadScene("DepartamentoFinanzas");
    }

    public void EntrarAtencion()
    {
        SceneManager.LoadScene("DepartamentoAtencion");
    }

    public void EntrarMarketing()
    {
        SceneManager.LoadScene("DepartamentoMarketing");
    }

    public void EntrarDireccion()
    {
        SceneManager.LoadScene("DepartamentoDireccion");
    }



    public void SalirDelMapa()
    {
        if(GameManager.escenaAnterior != null)
        {
            SceneManager.LoadScene(GameManager.escenaAnterior);
        }
        
    }


}
