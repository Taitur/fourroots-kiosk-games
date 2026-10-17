using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenaR : MonoBehaviour
{
    [Header("Sonido de botones")]
    public AudioSource audioSource;
    public AudioClip sonidoClick;
    public float retrasoCambioEscena = 0.12f;

    public void IrAEscenaJuego()
    {
        StartCoroutine(CambiarEscenaConSonido("R_Escena_Juego"));
    }

    public void IrAEscenaMenu()
    {
        StartCoroutine(CambiarEscenaConSonido("R_Escena_Menu"));
    }

    public void IrAEscenaRespira()
    {
        StartCoroutine(CambiarEscenaConSonido("R_Escena_Respira"));
    }

    public void IrAEscenaMuevete()
    {
        StartCoroutine(CambiarEscenaConSonido("R_Escena_Muevete"));
    }

    public void IrAEscenaEnfocate()
    {
        StartCoroutine(CambiarEscenaConSonido("R_Escena_Enfocate"));
    }

    public void IrAMenuPrincipal()
    {
        StartCoroutine(CambiarEscenaConSonido("MenuPrincipal"));
    }

    private IEnumerator CambiarEscenaConSonido(string nombreEscena)
    {
        if (audioSource != null && sonidoClick != null)
        {
            audioSource.PlayOneShot(sonidoClick);
        }

        yield return new WaitForSeconds(retrasoCambioEscena);

        SceneManager.LoadScene(nombreEscena);
    }
}