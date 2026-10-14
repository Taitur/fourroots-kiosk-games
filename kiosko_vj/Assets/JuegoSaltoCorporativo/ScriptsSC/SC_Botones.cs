using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Botones del menu: sonido de click en ambos y musica de fondo en loop.
public class SC_Botones : MonoBehaviour
{
    public string escenaJuego = "Nivel 1";
    public AudioClip sonidoClick;
    public AudioClip musicaFondo;
    [Range(0f, 1f)] public float volumenClick = 0.8f;
    [Range(0f, 1f)] public float volumenMusica = 0.5f;

    AudioSource fx;
    AudioSource musica;
    bool saliendo;

    void Awake()
    {
        fx = gameObject.AddComponent<AudioSource>();
        fx.playOnAwake = false;
        fx.spatialBlend = 0f;

        if (musicaFondo != null)
        {
            musica = gameObject.AddComponent<AudioSource>();
            musica.clip = musicaFondo;
            musica.loop = true;
            musica.volume = volumenMusica;
            musica.spatialBlend = 0f;
            musica.playOnAwake = false;
            musica.Play();
        }
    }

    // Sonido de click (boton de jugar)
    public void Click()
    {
        if (sonidoClick != null) fx.PlayOneShot(sonidoClick, volumenClick);
    }

    // Boton de jugar: suena el click y carga el nivel
    public void Jugar()
    {
        if (saliendo) return;
        saliendo = true;
        StartCoroutine(IrAlNivel());
    }

    IEnumerator IrAlNivel()
    {
        Click();
        yield return new WaitForSecondsRealtime(sonidoClick != null ? sonidoClick.length : 0f);
        SceneManager.LoadScene(escenaJuego);
    }

    // Boton de salir: suena el click y despues regresa al menu principal
    public void Menu()
    {
        if (saliendo) return;
        saliendo = true;
        StartCoroutine(IrAlMenu());
    }

    IEnumerator IrAlMenu()
    {
        Click();
        yield return new WaitForSecondsRealtime(sonidoClick != null ? sonidoClick.length : 0f);
        SceneManager.LoadScene(0);
    }
}
