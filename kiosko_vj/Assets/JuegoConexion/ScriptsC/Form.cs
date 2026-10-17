using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class Form : MonoBehaviour
{
    public TMP_InputField nombreInput;
    public TMP_InputField puestoInput;
    public TMP_InputField expertiseInput;

    public Button finanzasButton;
    public Button marketingButton;
    public Button direccionButton;
    public Button ventasButton;
    public Button rrhhButton;
    public Button atencionButton;

    public string departamentoElegido = "";

    GameManagerConexion gm;

    void finanzasButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoFinanzas";
    }

    void marketingButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoMarketing";
       
    }

    void direccionButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoDireccion";
        
    }

    void ventasButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoVentas";
       
    }

    void rrhhButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoRRHH";
       
    }

    void atencionButtonClicked()
    {
        gm.ReproducirSFX("BotonClick");
        departamentoElegido = "DepartamentoAtencion";
       
    }
     void Start()
    {
        gm = GameManagerConexion.instancia;
        finanzasButton.onClick.AddListener(finanzasButtonClicked);
        marketingButton.onClick.AddListener(marketingButtonClicked);
        direccionButton.onClick.AddListener(direccionButtonClicked);
        ventasButton.onClick.AddListener(ventasButtonClicked);
        rrhhButton.onClick.AddListener(rrhhButtonClicked);
        atencionButton.onClick.AddListener(atencionButtonClicked);
    }

   public void SubmitForm()
    {
        gm.ReproducirSFX("BotonClick");

        if (string.IsNullOrWhiteSpace(nombreInput.text) || string.IsNullOrEmpty(departamentoElegido))
        {
            Debug.Log("Rellena todos los campos del formulario antes de entrar al juego");
            return;
        }


        gm.StartCoroutine(ApiClient.Login(nombreInput.text, puestoInput.text, expertiseInput.text, departamentoElegido,
            j =>
            {

                gm.usuario = new GameManagerConexion.PlayerData(j.nombre, j.puesto, j.expertise, j.usuarioDepartamento);
                gm.usuario.puntos = j.puntos;
                gm.jugadorId = j.id;

                SceneManager.LoadScene(gm.usuario.usuarioDepartamento);
            }));
    }
}
