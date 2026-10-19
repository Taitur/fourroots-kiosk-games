using UnityEngine;
using UnityEngine.UI;

// Abre y cierra el panel de Ajustes.
// - Toggle de Música: enciende o apaga la música.
// - Barra de Volumen: volumen general del juego (música y efectos).
// - Reiniciar: reinicia la categoría que se está jugando
//   (solo aparece durante una partida).
// Si se abre durante una pregunta, pausa el reloj y lo reanuda al cerrar.
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    [Header("Música y volumen")]
    [SerializeField] private Toggle musicToggle;  // MusicToggle
    [SerializeField] private Slider volumeSlider; // VolumeSlider

    [Header("Botón Reiniciar")]
    [SerializeField] private GameObject restartButton; // ReiniciarButton

    [Header("Referencias")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private QuestionManager questionManager;
    [SerializeField] private GameObject questionPanel; // Panel_Question

    // Recuerda si el reloj estaba corriendo cuando se abrió Ajustes
    private bool timerWasRunning = false;

    // Volumen de la música cuando está encendida
    private float musicOnVolume = 0.6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsPanel.SetActive(false);

        // Volumen general guardado (1 = máximo)
        float volume = PlayerPrefs.GetFloat("GeneralVolume", 1f);
        AudioListener.volume = volume;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = volume;

            // Conecta la barra por código
            volumeSlider.onValueChanged.AddListener(SetGeneralVolume);
        }

        if (musicToggle != null)
        {
            // El toggle muestra si la música está encendida o apagada
            musicToggle.isOn = IsMusicOn();

            // Conecta el toggle por código
            musicToggle.onValueChanged.AddListener(SetMusicOn);
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

        // Reiniciar solo se muestra si hay una partida en curso
        if (restartButton != null)
        {
            bool isPlaying = questionPanel != null && questionPanel.activeInHierarchy;
            restartButton.SetActive(isPlaying);
        }

        settingsPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayOpenSettings();
        }
    }

    // Botón X dentro de Panel_Settings
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

    // Se llama solo cuando el jugador toca el toggle de Música
    public void SetMusicOn(bool isOn)
    {
        if (soundManager == null)
        {
            return;
        }

        if (isOn)
        {
            soundManager.SetMusicVolume(musicOnVolume); // Enciende la música
        }
        else
        {
            soundManager.SetMusicVolume(0f);            // Apaga la música
        }

        soundManager.PlayClick();
    }

    // Barra de Volumen
    public void SetGeneralVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GeneralVolume", volume);
    }

    // Botón Reiniciar dentro de Ajustes
    public void RestartFromSettings()
    {
        settingsPanel.SetActive(false);
        timerWasRunning = false;

        if (questionManager != null)
        {
            questionManager.RestartGame();
        }
    }

    private bool IsMusicOn()
    {
        return soundManager != null && soundManager.GetMusicVolume() > 0f;
    }
}