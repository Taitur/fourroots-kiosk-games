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
        departamentoElegido = "DepartamentoFinanzas";
    }

    void marketingButtonClicked()
    {
        departamentoElegido = "DepartamentoMarketing";
       
    }

    void direccionButtonClicked()
    {
        departamentoElegido = "DepartamentoDireccion";
        
    }

    void ventasButtonClicked()
    {
        departamentoElegido = "DepartamentoVentas";
       
    }

    void rrhhButtonClicked()
    {
        departamentoElegido = "DepartamentoRRHH";
       
    }

    void atencionButtonClicked()
    {
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
       if (string.IsNullOrWhiteSpace(nombreInput.text) || departamentoElegido == "")
        {
            Debug.LogWarning("Falta el nombre o el departamento");
            return;
        }

        gm.usuario = new GameManagerConexion.PlayerData(
            nombreInput.text, puestoInput.text, expertiseInput.text, departamentoElegido);

        gm.IniciarSesion();   
        
    }
}
