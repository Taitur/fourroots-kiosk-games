using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuessManager : MonoBehaviour
{
    [Header("Mostrar el dibujo elegido")]
    [SerializeField] private TMP_Text tmpPlayerName;
    [SerializeField] private SpriteRenderer drawingRenderer;   // o si usas un SpriteRenderer
    [SerializeField] private Vector2 drawingMaxSize = new Vector2(8f, 6f);

    [SerializeField] private TMP_InputField tmpInput;

    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeChange = 2f;
    [SerializeField] private List<PlayerDrawing> testEntries = new List<PlayerDrawing>();

    public PlayerDrawing Current { get; private set; }
    public bool HasGuessed { get; private set; }
    public bool GuessedCorrectly { get; private set; }

    private void Start()
    {
        // Enter también envía la respuesta (solo con TMP_InputField)
        if (tmpInput != null) tmpInput.onSubmit.AddListener(_ => SubmitGuess());

        List<PlayerDrawing> pool = new List<PlayerDrawing>(DrawingStore.Entries);
        pool.AddRange(testEntries);

        if (pool.Count == 0)
        {
            Debug.LogWarning("GuessManager: no hay dibujos disponibles (ni guardados ni de prueba).");
            return;
        }

        Current = pool[Random.Range(0, pool.Count)];
        ShowCurrent();
    }

    // Conecta esto al botón de "Enviar" / "Adivinar"
    public void SubmitGuess()
    {
        if (Current == null) return;
        if (GuessedCorrectly) return;                 // ya acertó, no reenviar

        string guess = tmpInput != null ? tmpInput.text : "";
        if (string.IsNullOrWhiteSpace(guess)) return;   // no cuenta si está vacío

        HasGuessed = true;
        GuessedCorrectly = Normalize(guess) == Normalize(Current.word);

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
        string nameText = Current.playerName;
        if (tmpPlayerName != null) tmpPlayerName.text = nameText;

        Sprite s = Current.GetSprite();
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