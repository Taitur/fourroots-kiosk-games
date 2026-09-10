using UnityEngine;
using UnityEngine.UI;

/// <summary>Controlador de SC_CrearDuda (bocadillo estilo cómic).</summary>
public class DudaUIController : MonoBehaviour
{
    public InputField inputDuda;
    public Button botonPublicar;

    // Emparejamiento local de ejemplo: departamento propio -> quién responde.
    private static readonly (string depto, string nombre, string deptoResp, string motivo)[] Matcher = new[]
    {
        ("Marketing", "Lucía Ortega", "Ventas", "Aprobación de presupuestos de campaña y márgenes"),
        ("Ventas", "Irene Bosch", "Finanzas", "Facturación y condiciones de pago a clientes"),
        ("RRHH", "Carmen Alcaide", "Dirección", "Política de plantilla y aprobación de vacantes"),
        ("Finanzas", "Raúl Peña", "Ventas", "Previsión de cierre y descuentos aplicados"),
        ("AtencionCliente", "Ane Beltrán", "Marketing", "Mensajes públicos y respuestas en canales"),
        ("Direccion", "Leire Goñi", "Marketing", "Datos de captación y coste por lead"),
    };

    private void Start()
    {
        botonPublicar.interactable = false;
        inputDuda.onValueChanged.AddListener(_ => botonPublicar.interactable = !string.IsNullOrWhiteSpace(inputDuda.text));
    }

    // Colgar del OnClick de "Publicar duda".
    public void PublicarDuda()
    {
        var texto = inputDuda.text.Trim();
        if (string.IsNullOrEmpty(texto)) return;

        var depto = LocalGameData.Perfil.departamento;
        var match = System.Array.Find(Matcher, m => m.depto == depto);
        if (match.nombre == null) match = Matcher[0];

        var entry = new DudaEntry
        {
            id = "DUD-" + (LocalGameData.Perfil.dudas.Count + 1).ToString("000"),
            texto = texto,
            estado = "EN_MATCH",
            match = new MatchInfo
            {
                nombreQuienResponde = match.nombre,
                departamentoQuienResponde = match.deptoResp,
                motivoSkill = match.motivo,
                hiloAbierto = false
            }
        };
        LocalGameData.Perfil.dudas.Add(entry);
        LocalGameData.Save();

        SceneRouter.ReturnToOffice();
    }

    // Colgar del OnClick de "Cancelar".
    public void Cancelar() => SceneRouter.ReturnToOffice();
}
