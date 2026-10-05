using System.Collections;
using UnityEngine;

public class AnimacionAvatarRespira : MonoBehaviour
{
    public float escalaMinima = 1f;
    public float escalaMaxima = 1.08f;
    public float duracion = 4f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return CambiarEscala(escalaMinima, escalaMaxima, duracion);
            yield return CambiarEscala(escalaMaxima, escalaMinima, duracion);
        }
    }

    private IEnumerator CambiarEscala(float inicio, float fin, float tiempoTotal)
    {
        float tiempo = 0f;

        while (tiempo < tiempoTotal)
        {
            tiempo += Time.deltaTime;

            float progreso = tiempo / tiempoTotal;
            float escala = Mathf.Lerp(inicio, fin, progreso);

            transform.localScale = Vector3.one * escala;

            yield return null;
        }

        transform.localScale = Vector3.one * fin;
    }
}