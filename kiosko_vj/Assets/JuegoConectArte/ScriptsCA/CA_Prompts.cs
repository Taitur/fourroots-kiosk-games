using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pide UN prompt al azar a la base de datos (vía DrawingApi) al iniciar la escena
/// y lo muestra en un Text del Canvas. Guarda su palabra y su id.
/// Funciona con TextMeshPro o con el Text clásico de UI (usa solo uno).
/// </summary>
public class CA_Prompts : MonoBehaviour
{
    [Header("Dónde se muestra la palabra (usa solo uno)")]
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Text legacyText;

    [Header("Textos")]
    [SerializeField] private string prefix = "Dibuja: ";
    [SerializeField] private string loadingMessage = "Cargando palabra...";
    [SerializeField] private string errorMessage = "No se pudo cargar la palabra";

    public string CurrentWord { get; private set; }
    public int CurrentPromptId { get; private set; }   // 0 = todavía no se ha cargado

    private void Start()
    {
        SetText(loadingMessage);

        if (CA_ApiDibujos.Instance == null)
        {
            Debug.LogWarning("WordPicker: no hay un objeto CA_ApiDibujos en la escena.");
            SetText(errorMessage);
            return;
        }

        CA_ApiDibujos.Instance.GetRandomPrompt(OnPromptLoaded);
    }

    private void OnPromptLoaded(CA_ApiDibujos.PromptResponse prompt)
    {
        if (prompt == null || prompt.id_prompt <= 0)
        {
            SetText(errorMessage);
            return;
        }

        CurrentPromptId = prompt.id_prompt;
        CurrentWord = prompt.nombre_prompt;
        SetText(prefix + CurrentWord);
    }

    private void SetText(string text)
    {
        if (tmpText != null) tmpText.text = text;
        if (legacyText != null) legacyText.text = text;
    }
}