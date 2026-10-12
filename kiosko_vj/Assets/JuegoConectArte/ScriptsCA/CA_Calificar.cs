using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Calificación de 1 a 5. Al elegir el botón N, se iluminan los botones 1 hasta N.
/// Solo arrastra los 5 botones EN ORDEN a la lista; el script les agrega el clic solo.
/// </summary>
public class CA_Calificar : MonoBehaviour
{
    [Header("Los 5 botones, en orden (del 1 al 5)")]
    [SerializeField] private Button[] ratingButtons;

    [Header("Dibujo que se califica (para guardar la calificación en la base de datos)")]
    [SerializeField] private CA_Adivinar guessManager;

    [Header("Sprites")]
    [SerializeField] private Sprite offSprite;   // apagado
    [SerializeField] private Sprite onSprite;    // iluminado

    // 0 = todavía no calificó, 1 a 5 = calificación elegida
    public int CurrentRating { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < ratingButtons.Length; i++)
        {
            int rating = i + 1;   // copia local para que cada botón recuerde su número
            ratingButtons[i].onClick.AddListener(() => SetRating(rating));
        }

        Refresh();
    }

    // Califica de 1 a 5 (también puedes llamarla desde código)
    public void SetRating(int rating)
    {
        CurrentRating = Mathf.Clamp(rating, 0, ratingButtons.Length);
        Refresh();

        // Guarda la calificación en puntos_por_calificacion (la última que elija es la que queda)
        if (CurrentRating > 0 && guessManager != null && guessManager.Current != null && CA_ApiDibujos.Instance != null)
            CA_ApiDibujos.Instance.SendRatingPoints(guessManager.Current.idDibujo, CurrentRating);
    }

    // Quita la calificación (útil si luego cargas otro dibujo)
    public void ResetRating() => SetRating(0);

    private void Refresh()
    {
        for (int i = 0; i < ratingButtons.Length; i++)
        {
            Image img = ratingButtons[i].targetGraphic as Image;
            if (img == null) continue;

            // Si el botón está dentro del rango elegido, usa el sprite iluminado
            img.sprite = (i < CurrentRating) ? onSprite : offSprite;
        }
    }
}