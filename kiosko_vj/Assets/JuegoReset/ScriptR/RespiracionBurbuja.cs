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

    private IEnumerator Start()
    {
        transform.localScale = Vector3.one * escalaMinima;

        barraProgreso.sprite = barraVacia;

        for (int ronda = 1; ronda <= 5; ronda++)
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

            // PROGRESO
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

        yield return new WaitForSeconds(1f);

        barraProgreso.sprite = barraCompletado;
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