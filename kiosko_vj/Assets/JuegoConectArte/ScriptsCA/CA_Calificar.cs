using UnityEngine;
using UnityEngine.UI;

public class CA_Calificar : MonoBehaviour
{    [SerializeField] private Button[] ratingButtons;

    [SerializeField] private Sprite offSprite;   // apagado
    [SerializeField] private Sprite onSprite;    // iluminado

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

    public void SetRating(int rating)
    {
        CurrentRating = Mathf.Clamp(rating, 0, ratingButtons.Length);
        Refresh();
    }

    public void ResetRating() => SetRating(0);

    private void Refresh()
    {
        for (int i = 0; i < ratingButtons.Length; i++)
        {
            Image img = ratingButtons[i].targetGraphic as Image;
            if (img == null) continue;

            // Si el botón está dentro del rango elegido, se ilumina
            img.sprite = (i < CurrentRating) ? onSprite : offSprite;
        }
    }
}