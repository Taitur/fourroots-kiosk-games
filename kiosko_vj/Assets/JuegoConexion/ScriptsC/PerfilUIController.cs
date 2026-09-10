using UnityEngine;
using UnityEngine.UI;

/// <summary>Controlador de SC_CrearPerfil (corcho + hoja).</summary>
public class PerfilUIController : MonoBehaviour
{
    [Header("Campos")]
    public InputField inputNombre;
    public InputField inputPuesto;
    public InputField inputSkills;
    public Text departamentoSeleccionadoLabel;
    public Button botonFichar;

    private string _departamentoElegido = "Marketing";

    private void Start()
    {
        botonFichar.interactable = false;
        if (inputNombre != null) inputNombre.onValueChanged.AddListener(_ => ValidarFormulario());
    }

    // Colgar del OnClick de cada uno de los 6 botones de departamento.
    public void SetDepartamento(string departamento)
    {
        _departamentoElegido = departamento;
        if (departamentoSeleccionadoLabel != null) departamentoSeleccionadoLabel.text = departamento;
        ValidarFormulario();
    }

    private void ValidarFormulario()
    {
        botonFichar.interactable = !string.IsNullOrWhiteSpace(inputNombre.text);
    }

    // Colgar del OnClick del botón "Fichar y entrar".
    public void ConfirmarPerfil()
    {
        var perfil = LocalGameData.Perfil;
        perfil.nombre = inputNombre.text;
        perfil.puesto = inputPuesto.text;
        perfil.departamento = _departamentoElegido;
        perfil.skills = inputSkills.text;
        LocalGameData.Save();

        SceneRouter.VisitarOficina(_departamentoElegido);
    }
}
