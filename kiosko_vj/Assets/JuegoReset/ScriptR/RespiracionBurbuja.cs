using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespiracionBurbuja : MonoBehaviour
{
    [Header("Tamaño")]
    public float escalaMinima = 1.2f;
    public float escalaMaxima = 2.2f;

    [Header("Tiempos")]
    public float tiempoInhalar = 4f;
    public float tiempoMantener = 2f;
    public float tiempoExhalar = 6f;

    [Header("Indicaciones")]
    public Image textoRespira;
    public Sprite spriteInhala;
    public Sprite spriteSosten;
    public Sprite spriteExhala;

    [Header("Barra de progreso")]
    public Image barraProgreso;
    public Sprite barraVacia;
    public Sprite barra25;
    public Sprite barra50;
    public Sprite barra75;
    public Sprite barra85;
    public Sprite barra100;
    public Sprite barraCompletado;

    [Header("Sonido final")]
    public AudioSource audioSource;
    public AudioClip sonidoCompletado;

    private bool respiracionIniciada = false;

    private void Start()
    {
        transform.localScale = Vector3.one * escalaMinima;

        if (barraProgreso != null && barraVacia != null)
        {
            barraProgreso.sprite = barraVacia;
        }
    }

    public void IniciarRespiracion()
    {
        if (respiracionIniciada)
            return;

        respiracionIniciada = true;

        StartCoroutine(RutinaRespiracion());
    }

    private IEnumerator RutinaRespiracion()
    {
        for (int ronda = 1; ronda <= 5; ronda++)
        {
            // INHALAR
            if (textoRespira != null && spriteInhala != null)
            {
                textoRespira.sprite = spriteInhala;
            }

            yield return CambiarEscala(
                escalaMinima,
                escalaMaxima,
                tiempoInhalar
            );

            // SOSTENER
            if (textoRespira != null && spriteSosten != null)
            {
                textoRespira.sprite = spriteSosten;
            }

            yield return new WaitForSeconds(tiempoMantener);

            // EXHALAR
            if (textoRespira != null && spriteExhala != null)
            {
                textoRespira.sprite = spriteExhala;
            }

            yield return CambiarEscala(
                escalaMaxima,
                escalaMinima,
                tiempoExhalar
            );

            // PROGRESO
            if (barraProgreso != null)
            {
                switch (ronda)
                {
                    case 1:
                        barraProgreso.sprite = barra25;
                        break;

                    case 2:
                        barraProgreso.sprite = barra50;
                        break;

                    case 3:
                        barraProgreso.sprite = barra75;
                        break;

                    case 4:
                        barraProgreso.sprite = barra85;
                        break;

                    case 5:
                        barraProgreso.sprite = barra100;
                        break;
                }
            }
        }

        yield return new WaitForSeconds(1f);

        if (barraProgreso != null && barraCompletado != null)
        {
            barraProgreso.sprite = barraCompletado;
        }

        if (audioSource != null && sonidoCompletado != null)
        {
            audioSource.PlayOneShot(sonidoCompletado);
        }

        respiracionIniciada = false;
    }

    private IEnumerator CambiarEscala(
        float inicio,
        float fin,
        float duracion
    )
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