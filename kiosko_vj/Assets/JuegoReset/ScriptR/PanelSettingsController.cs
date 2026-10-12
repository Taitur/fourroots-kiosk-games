using UnityEngine;

public class PanelSettingsController : MonoBehaviour
{
    [SerializeField] private GameObject panelSettings;

    public void AbrirSettings()
    {
        if (panelSettings != null)
        {
            panelSettings.SetActive(true);
        }
    }

    public void CerrarSettings()
    {
        if (panelSettings != null)
        {
            panelSettings.SetActive(false);
        }
    }
}