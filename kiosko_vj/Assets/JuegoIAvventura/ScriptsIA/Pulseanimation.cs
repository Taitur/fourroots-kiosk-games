using UnityEngine;

// Animación de "respirar": el objeto crece y se encoge suavemente sin parar.
// para logo del robot, el título IAvventura o los personajes de resultados.
public class PulseAnimation : MonoBehaviour
{
    public float speed = 2f;     // Qué tan rápido late
    public float amount = 0.05f; // Cuánto crece (0.05 = 5%)

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * speed) * amount;
        transform.localScale = originalScale * scale;
    }
}