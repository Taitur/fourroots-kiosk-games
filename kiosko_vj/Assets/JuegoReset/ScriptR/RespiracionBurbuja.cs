using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespiracionBurbuja : MonoBehaviour
{
    [Header("Tamaño")]
    public float escalaMinima = 0.9f;
    public float escalaMaxima = 1.6f;

    [Header("Tiempos")]
    public float tiempoInhalar = 4f;
    public float tiempoMantener = 2f;
    public float tiempoExhalar = 4f;

    [Header("Indicaciones")]
    public Image textoRespira;
    public Sprite spriteInhala;
    public Sprite spriteSosten;
    public Sprite spriteExhala;

    private IEnumerator Start()
    {
        transform.localScale = Vector3.one * escalaMinima;

        while (true)
        {
            // INHALAR
            textoRespira.sprite = spriteInhala;

            yield return CambiarEscala(
                escalaMinima,
                escalaMaxima,
                tiempoInhalar
            );

            // SOSTENER
            textoRespira.sprite = spriteSosten;

            yield return new WaitForSeconds(tiempoMantener);

            // EXHALAR
            textoRespira.sprite = spriteExhala;

            yield return CambiarEscala(
                escalaMaxima,
                escalaMinima,
                tiempoExhalar
            );
        }
    }

    private IEnumerator CambiarEscala(float inicio, float fin, float duracion)
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float progreso = tiempo / duracion;
            float escala = Mathf.Lerp(inicio, fin, progreso);

            transform.localScale = Vector3.one * escala;

            yield return null;
        }

        transform.localScale = Vector3.one * fin;
    }
}