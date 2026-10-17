using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnfocateController : MonoBehaviour
{
    [Header("Avatar")]
    public Image avatar;

    [Header("Animacion Feliz")]
    public Sprite[] felizFrames;

    [Header("Animacion No")]
    public Sprite[] noFrames;

    [Header("Hojas")]
    public Button[] hojas;

    [Header("Contador")]
    public Image contadorImagen;
    public Sprite[] contadorSprites;

    [Header("Cartel Final")]
    public Image cartelEnfocate;
    public Sprite spriteCompletado;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoHojaCorrecta;
    public AudioClip sonidoError;

    [Header("Velocidad")]
    public float tiempoEntreFrames = 0.18f;

    private int hojasEncontradas = 0;
    private bool reaccionando = false;
    private bool juegoTerminado = false;

    private void Start()
    {
        hojasEncontradas = 0;
        juegoTerminado = false;

        if (avatar != null && felizFrames.Length > 0)
        {
            avatar.sprite = felizFrames[0];
        }

        ActualizarContador();

        for (int i = 0; i < hojas.Length; i++)
        {
            int indice = i;

            if (hojas[i] != null)
            {
                hojas[i].onClick.RemoveAllListeners();
                hojas[i].onClick.AddListener(() => EncontrarHoja(indice));
            }
        }
    }

    private void EncontrarHoja(int indice)
    {
        if (juegoTerminado)
            return;

        if (indice < 0 || indice >= hojas.Length)
            return;

        if (!hojas[indice].gameObject.activeSelf)
            return;

        hojas[indice].gameObject.SetActive(false);

        hojasEncontradas++;

        ActualizarContador();

        if (audioSource != null && sonidoHojaCorrecta != null)
        {
            audioSource.PlayOneShot(sonidoHojaCorrecta);
        }

        if (!reaccionando)
        {
            StartCoroutine(AnimacionFeliz());
        }

        if (hojasEncontradas >= 10)
        {
            juegoTerminado = true;

            if (cartelEnfocate != null && spriteCompletado != null)
            {
                cartelEnfocate.sprite = spriteCompletado;
            }

            Debug.Log("¡ENFOCATE COMPLETADO!");
        }
    }

    public void ClickIncorrecto()
    {
        if (juegoTerminado)
            return;

        if (reaccionando)
            return;

        if (audioSource != null && sonidoError != null)
        {
            audioSource.PlayOneShot(sonidoError);
        }

        StartCoroutine(AnimacionNo());
    }

    private void ActualizarContador()
    {
        if (contadorImagen == null)
            return;

        if (contadorSprites == null || contadorSprites.Length == 0)
            return;

        int indice = Mathf.Clamp(
            hojasEncontradas,
            0,
            contadorSprites.Length - 1
        );

        contadorImagen.sprite = contadorSprites[indice];
    }

    private IEnumerator AnimacionFeliz()
    {
        reaccionando = true;

        for (int i = 0; i < felizFrames.Length; i++)
        {
            if (avatar != null)
            {
                avatar.sprite = felizFrames[i];
            }

            yield return new WaitForSeconds(tiempoEntreFrames);
        }

        if (avatar != null && felizFrames.Length > 0)
        {
            avatar.sprite = felizFrames[0];
        }

        reaccionando = false;
    }

    private IEnumerator AnimacionNo()
    {
        reaccionando = true;

        for (int i = 0; i < noFrames.Length; i++)
        {
            if (avatar != null)
            {
                avatar.sprite = noFrames[i];
            }

            yield return new WaitForSeconds(tiempoEntreFrames);
        }

        if (avatar != null && felizFrames.Length > 0)
        {
            avatar.sprite = felizFrames[0];
        }

        reaccionando = false;
    }
}