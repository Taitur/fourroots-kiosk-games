using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverMenuPrincipal : MonoBehaviour
{
    [Header("Sonido del boton")]
    public AudioSource audioSource;
    public AudioClip sonidoClick;
    public float retraso = 0.12f;

    public void VolverAlMenu()
    {
        StartCoroutine(VolverConSonido());
    }

    private IEnumerator VolverConSonido()
    {
        if (audioSource != null && sonidoClick != null)
        {
            audioSource.PlayOneShot(sonidoClick);
        }

        yield return new WaitForSeconds(retraso);

        SceneManager.LoadScene("MenuPrincipal");
    }
}