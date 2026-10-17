using UnityEngine;

public class InicioRespira : MonoBehaviour
{
    [Header("Panel de inicio")]
    public GameObject panelInicio;

    [Header("Respiracion")]
    public RespiracionBurbuja respiracion;

    [Header("Sonido de inicio")]
    public AudioSource audioSource;
    public AudioClip sonidoComenzar;

    public void Comenzar()
    {
        if (audioSource != null && sonidoComenzar != null)
        {
            audioSource.PlayOneShot(sonidoComenzar);
        }

        if (panelInicio != null)
        {
            panelInicio.SetActive(false);
        }

        if (respiracion != null)
        {
            respiracion.IniciarRespiracion();
        }
    }
}