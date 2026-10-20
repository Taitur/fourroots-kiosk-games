using UnityEngine;

public class PanelSettingsController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelSettings;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAbrir;
    [SerializeField] private AudioClip sonidoCerrar;

    public void AbrirSettings()
    {
        if (panelSettings != null)
        {
            panelSettings.SetActive(true);
        }

        if (audioSource != null && sonidoAbrir != null)
        {
            audioSource.PlayOneShot(sonidoAbrir);
        }
    }

    public void CerrarSettings()
    {
        if (audioSource != null && sonidoCerrar != null)
        {
            audioSource.PlayOneShot(sonidoCerrar);
        }

        if (panelSettings != null)
        {
            panelSettings.SetActive(false);
        }
    }
}