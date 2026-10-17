using UnityEngine;
using UnityEngine.UI;

// Abre y cierra el panel de configuración y controla los sliders de volumen.
// Si se abre durante una pregunta, pausa el reloj y lo reanuda al cerrar.
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private TimeManager timeManager;

    // Recuerda si el reloj estaba corriendo cuando se abrió Settings
    private bool timerWasRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsPanel.SetActive(false);

        if (soundManager != null)
        {
            // Los sliders muestran el volumen guardado
            musicSlider.value = soundManager.GetMusicVolume();
            sfxSlider.value = soundManager.GetSfxVolume();

            // Conecta los sliders por código (no hace falta configurar
            // On Value Changed en el Inspector)
            musicSlider.onValueChanged.AddListener(soundManager.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(soundManager.SetSfxVolume);
        }
    }

    // Botón de engrane (en Inicio y en Preguntas)
    public void OpenSettings()
    {
        timerWasRunning = timeManager != null && timeManager.isRunning;

        if (timerWasRunning)
        {
            timeManager.PauseTimer();
        }

        settingsPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayOpenSettings();
        }
    }

    // Botón "Cerrar" dentro de Panel_Settings
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (timerWasRunning)
        {
            timeManager.ResumeTimer();
            timerWasRunning = false;
        }

        if (soundManager != null)
        {
            soundManager.PlayCloseSettings();
        }
    }
}