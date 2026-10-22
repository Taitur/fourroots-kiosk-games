using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MueveteController : MonoBehaviour
{
    [Header("Panel de inicio")]
    public GameObject panelInicio;

    [Header("Avatar")]
    public Image avatar;

    [Header("Ejercicio 1 - Marcha")]
    public Sprite[] marchaFrames;
    public float tiempoEntreFramesMarcha = 0.25f;

    [Header("Ejercicio 2 - Elevacion de brazos")]
    public Sprite[] brazosFrames;
    public float tiempoEntreFramesBrazos = 0.30f;

    [Header("Ejercicio 3 - Inclinacion lateral")]
    public Sprite[] lateralFrames;
    public float tiempoEntreFramesLateral = 0.30f;

    [Header("Texto del ejercicio")]
    public Image textoEjercicio;
    public Sprite spriteMarchaLugar;
    public Sprite spriteBrazos;
    public Sprite spriteLateral;
    public Sprite spriteCompletado;
    public Sprite spriteSiguienteEjercicio;

    [Header("Barra de progreso")]
    public Image barraProgreso;
    public Sprite barra0;
    public Sprite barra25;
    public Sprite barra50;
    public Sprite barra75;
    public Sprite barra100;

    [Header("Duracion")]
    public float duracionMarcha = 8f;
    public float duracionBrazos = 8f;
    public float duracionLateral = 8f;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoInicio;
    public AudioClip sonidoEjercicioCompletado;
    public AudioClip sonidoSiguienteEjercicio;
    public AudioClip sonidoRutinaCompletada;

    [Header("Base de datos - Reset")]
    public int idActividad = 3;
    public int idUsuario = 1;
    public int idUsuarioRegistro = 1;

    private Coroutine animacionActual;
    private bool rutinaIniciada = false;
    private bool actividadRegistrada = false;

    public void IniciarRutina()
    {
        if (rutinaIniciada)
            return;

        rutinaIniciada = true;
        actividadRegistrada = false;

        if (audioSource != null && sonidoInicio != null)
        {
            audioSource.PlayOneShot(sonidoInicio);
        }

        StartCoroutine(RutinaCompleta());
    }

    private IEnumerator RutinaCompleta()
    {
        yield return StartCoroutine(RutinaMarcha());

        textoEjercicio.sprite = spriteCompletado;

        if (audioSource != null && sonidoEjercicioCompletado != null)
        {
            audioSource.PlayOneShot(sonidoEjercicioCompletado);
        }

        yield return new WaitForSeconds(3f);

        textoEjercicio.sprite = spriteSiguienteEjercicio;

        if (audioSource != null && sonidoSiguienteEjercicio != null)
        {
            audioSource.PlayOneShot(sonidoSiguienteEjercicio);
        }

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(RutinaBrazos());

        textoEjercicio.sprite = spriteCompletado;

        if (audioSource != null && sonidoEjercicioCompletado != null)
        {
            audioSource.PlayOneShot(sonidoEjercicioCompletado);
        }

        yield return new WaitForSeconds(3f);

        textoEjercicio.sprite = spriteSiguienteEjercicio;

        if (audioSource != null && sonidoSiguienteEjercicio != null)
        {
            audioSource.PlayOneShot(sonidoSiguienteEjercicio);
        }

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(RutinaLateral());

        textoEjercicio.sprite = spriteCompletado;

        if (audioSource != null && sonidoRutinaCompletada != null)
        {
            audioSource.PlayOneShot(sonidoRutinaCompletada);
        }

        if (!actividadRegistrada)
        {
            actividadRegistrada = true;

            yield return StartCoroutine(
                ResetApiClient.RegistrarActividad(
                    idActividad,
                    idUsuario,
                    idUsuarioRegistro,
                    () =>
                    {
                        Debug.Log(
                            "RESET MUEVETE: actividad registrada correctamente."
                        );
                    },
                    (error) =>
                    {
                        Debug.LogError(
                            "RESET MUEVETE: no se pudo registrar la actividad. " +
                            error
                        );

                        actividadRegistrada = false;
                    }
                )
            );
        }

        rutinaIniciada = false;
    }

    private IEnumerator RutinaMarcha()
    {
        textoEjercicio.sprite = spriteMarchaLugar;
        barraProgreso.sprite = barra0;

        animacionActual = StartCoroutine(
            AnimarSprites(marchaFrames, tiempoEntreFramesMarcha)
        );

        yield return StartCoroutine(
            ActualizarBarra(duracionMarcha)
        );

        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
            animacionActual = null;
        }
    }

    private IEnumerator RutinaBrazos()
    {
        textoEjercicio.sprite = spriteBrazos;
        barraProgreso.sprite = barra0;

        animacionActual = StartCoroutine(
            AnimarSprites(brazosFrames, tiempoEntreFramesBrazos)
        );

        yield return StartCoroutine(
            ActualizarBarra(duracionBrazos)
        );

        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
            animacionActual = null;
        }
    }

    private IEnumerator RutinaLateral()
    {
        textoEjercicio.sprite = spriteLateral;
        barraProgreso.sprite = barra0;

        animacionActual = StartCoroutine(
            AnimarSprites(lateralFrames, tiempoEntreFramesLateral)
        );

        yield return StartCoroutine(
            ActualizarBarra(duracionLateral)
        );

        if (animacionActual != null)
        {
            StopCoroutine(animacionActual);
            animacionActual = null;
        }
    }

    private IEnumerator ActualizarBarra(float duracion)
    {
        float tramo = duracion / 4f;

        yield return new WaitForSeconds(tramo);
        barraProgreso.sprite = barra25;

        yield return new WaitForSeconds(tramo);
        barraProgreso.sprite = barra50;

        yield return new WaitForSeconds(tramo);
        barraProgreso.sprite = barra75;

        yield return new WaitForSeconds(tramo);
        barraProgreso.sprite = barra100;
    }

    private IEnumerator AnimarSprites(Sprite[] frames, float velocidad)
    {
        int i = 0;

        while (true)
        {
            if (frames != null && frames.Length > 0)
            {
                avatar.sprite = frames[i];
                i = (i + 1) % frames.Length;
            }

            yield return new WaitForSeconds(velocidad);
        }
    }
}