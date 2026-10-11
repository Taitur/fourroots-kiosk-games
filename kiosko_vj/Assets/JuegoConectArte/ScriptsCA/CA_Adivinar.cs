using System.Collections;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Escena de ADIVINAR. Pide a la base de datos (vía DrawingApi) un dibujo al azar de OTRO jugador,
/// muestra el nombre y la imagen, compara lo que escribe el jugador con la palabra
/// y manda los puntos a la base de datos si acierta.
/// Funciona con TextMeshPro o con los componentes clásicos de UI (usa solo uno de cada par).
/// </summary>
public class CA_Adivinar : MonoBehaviour
{
    [Header("Mostrar el dibujo elegido")]
    [SerializeField] private TMP_Text tmpPlayerName;
    [SerializeField] private Text legacyPlayerName;
    [SerializeField] private string namePrefix = "Dibujo de: ";
    [SerializeField] private Image drawingImage;               // si usas UI Image
    [SerializeField] private SpriteRenderer drawingRenderer;   // o si usas un SpriteRenderer
    [Tooltip("Tamaño máximo (ancho, alto) en unidades de mundo que ocupará el dibujo si usas SpriteRenderer")]
    [SerializeField] private Vector2 drawingMaxSize = new Vector2(8f, 6f);

    [Header("Adivinar (usa solo uno)")]
    [SerializeField] private TMP_InputField tmpInput;
    [SerializeField] private InputField legacyInput;

    [Header("Resultado (opcional, usa solo uno)")]
    [SerializeField] private TMP_Text tmpResult;
    [SerializeField] private Text legacyResult;
    [SerializeField] private string loadingMessage = "Cargando dibujo...";
    [SerializeField] private string noDrawingsMessage = "No hay dibujos de otros jugadores todavía.";
    [SerializeField] private string correctMessage = "¡Correcto!";
    [SerializeField] private string wrongMessage = "Incorrecto. Era: ";
    [SerializeField] private bool onlyOneGuess = true;

    [Header("Puntos")]
    [Tooltip("Puntos que se guardan en puntos_por_adivinar cuando la respuesta es correcta")]
    [SerializeField] private int pointsForCorrectGuess = 100;

    [Header("Al acertar")]
    [Tooltip("Escena a cargar cuando la respuesta es correcta (debe estar en Build Settings). Vacío = no cambia")]
    [SerializeField] private string nextSceneName;
    [Tooltip("Segundos de espera para que el jugador alcance a ver el resultado")]
    [SerializeField] private float delayBeforeChange = 2f;

    public CA_JugadorDibujo Current { get; private set; }
    public bool HasGuessed { get; private set; }
    public bool GuessedCorrectly { get; private set; }

    private void Start()
    {
        // Enter también envía la respuesta (solo con TMP_InputField)
        if (tmpInput != null) tmpInput.onSubmit.AddListener(_ => SubmitGuess());

        CA_ApiDibujos api = CA_ApiDibujos.Instance;
        if (api == null)
        {
            Debug.LogWarning("GuessManager: no hay un objeto CA_ApiDibujos en la escena.");
            SetResult("Falta el objeto CA_ApiDibujos en la escena.");
            return;
        }

        SetResult(loadingMessage);
        api.GetRandomDrawing(api.PlayerId, OnDrawingLoaded);
    }

    private void OnDrawingLoaded(CA_ApiDibujos.DrawingResponse response)
    {
        if (response == null || string.IsNullOrEmpty(response.img_dibujo))
        {
            SetResult(noDrawingsMessage);
            return;
        }

        Current = CA_ApiDibujos.ToPlayerDrawing(response);
        ShowCurrent();
        SetResult("");
    }

    // Conecta esto al botón de "Enviar" / "Adivinar"
    public void SubmitGuess()
    {
        if (Current == null) return;                  // todavía no carga el dibujo
        if (GuessedCorrectly) return;                 // ya acertó, no reenviar
        if (onlyOneGuess && HasGuessed) return;

        string guess = tmpInput != null ? tmpInput.text : (legacyInput != null ? legacyInput.text : "");
        if (string.IsNullOrWhiteSpace(guess)) return;   // no cuenta si está vacío

        HasGuessed = true;
        GuessedCorrectly = Normalize(guess) == Normalize(Current.word);

        SetResult(GuessedCorrectly ? correctMessage : wrongMessage + Current.word);

        if (GuessedCorrectly && CA_ApiDibujos.Instance != null)
            CA_ApiDibujos.Instance.SendGuessPoints(Current.idDibujo, pointsForCorrectGuess);

        if (onlyOneGuess || GuessedCorrectly)
        {
            if (tmpInput != null) tmpInput.interactable = false;
            if (legacyInput != null) legacyInput.interactable = false;
        }

        if (GuessedCorrectly && !string.IsNullOrEmpty(nextSceneName))
            StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeChange);
        SceneManager.LoadScene(nextSceneName);
    }

    private void ShowCurrent()
    {
        string nameText = namePrefix + Current.playerName;
        if (tmpPlayerName != null) tmpPlayerName.text = nameText;
        if (legacyPlayerName != null) legacyPlayerName.text = nameText;

        Sprite s = Current.GetSprite();
        if (drawingImage != null)
        {
            drawingImage.sprite = s;
            drawingImage.preserveAspect = true;
        }

        if (drawingRenderer != null)
        {
            drawingRenderer.sprite = s;

            if (s != null)
            {
                // Ajusta la escala para que el dibujo quepa en drawingMaxSize sin deformarse
                Vector2 spriteSize = s.bounds.size;
                float scale = Mathf.Min(drawingMaxSize.x / spriteSize.x, drawingMaxSize.y / spriteSize.y);
                drawingRenderer.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }

    private void SetResult(string text)
    {
        if (tmpResult != null) tmpResult.text = text;
        if (legacyResult != null) legacyResult.text = text;
    }

    // Ignora mayúsculas, espacios sobrantes y acentos: "Árbol" == "arbol"
    private static string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";

        string decomposed = s.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();
        foreach (char c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}