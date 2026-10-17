using UnityEngine;

public class PanelComoJugarController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelComoJugar;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAbrir;
    [SerializeField] private AudioClip sonidoCerrar;

    public void AbrirPanel()
    {
        if (panelComoJugar != null)
        {
            panelComoJugar.SetActive(true);
        }

        if (audioSource != null && sonidoAbrir != null)
        {
            audioSource.PlayOneShot(sonidoAbrir);
        }
    }

    public void CerrarPanel()
    {
        if (audioSource != null && sonidoCerrar != null)
        {
            audioSource.PlayOneShot(sonidoCerrar);
        }

        if (panelComoJugar != null)
        {
            panelComoJugar.SetActive(false);
        }
    }
}