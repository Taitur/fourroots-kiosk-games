using UnityEngine;
using UnityEngine.EventSystems;

// Animación de botón: se encoge un poco al presionarlo y regresa al soltarlo.
public class ButtonPop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressScale = 0.9f; // Tamaño al presionar (0.9 = 90%)

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    // Se llama al tocar/presionar el botón
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressScale;
    }

    // Se llama al soltar el botón
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // Si el panel se oculta mientras está presionado, regresa a su tamaño
    void OnDisable()
    {
        transform.localScale = originalScale;
    }
}