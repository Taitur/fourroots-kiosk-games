using UnityEngine;

// Controla la música de fondo y los efectos sonoros de IAvventura.
// Va en un GameObject vacío llamado "SoundManager" que tenga DOS AudioSource:
// uno para la música y otro para los efectos.
public class SoundManager : MonoBehaviour
{
    [Header("Fuentes de audio")]
    [SerializeField] private AudioSource musicSource; // Música (Loop activado)
    [SerializeField] private AudioSource sfxSource;   // Efectos sonoros

    [Header("Música (6 pistas)")]
    [SerializeField] private AudioClip menuMusic;        // Pantalla de inicio y categorías
    [SerializeField] private AudioClip[] categoryMusic;  // 4 pistas: una por categoría (elemento 0 = categoría 1)
    [SerializeField] private AudioClip resultsMusic;     // Pantalla de resultados

    [Header("Efectos sonoros (12 efectos)")]
    [SerializeField] private AudioClip clickSound;         // Botones en general
    [SerializeField] private AudioClip categorySound;      // Al elegir una categoría
    [SerializeField] private AudioClip newQuestionSound;   // Al aparecer una pregunta
    [SerializeField] private AudioClip correctSound;       // Respuesta correcta
    [SerializeField] private AudioClip incorrectSound;     // Respuesta incorrecta
    [SerializeField] private AudioClip tickSound;          // Últimos 5 segundos del reloj
    [SerializeField] private AudioClip timeUpSound;        // Se acabó el tiempo
    [SerializeField] private AudioClip finishSound;        // Fin de la partida
    [SerializeField] private AudioClip perfectSound;       // Todas correctas
    [SerializeField] private AudioClip openSettingsSound;  // Abrir configuración
    [SerializeField] private AudioClip closeSettingsSound; // Cerrar configuración
    [SerializeField] private AudioClip exitSound;          // Botón X (abandonar partida)

    // Awake se ejecuta antes que Start, así el volumen guardado
    // ya está listo cuando SettingsManager lee los sliders.
    void Awake()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            musicSource.loop = true;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    void Start()
    {
        PlayMenuMusic();
    }

    // ---------- MÚSICA ----------

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
        {
            return;
        }

        // Si ya está sonando esa misma canción, no la reinicia
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    // category va de 1 a 4
    public void PlayCategoryMusic(int category)
    {
        int index = category - 1;

        if (categoryMusic != null && index >= 0 && index < categoryMusic.Length)
        {
            PlayMusic(categoryMusic[index]);
        }
    }

    public void PlayResultsMusic()
    {
        PlayMusic(resultsMusic);
    }

    // ---------- EFECTOS ----------

    private void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayClick() { PlaySfx(clickSound); }
    public void PlayCategory() { PlaySfx(categorySound); }
    public void PlayNewQuestion() { PlaySfx(newQuestionSound); }
    public void PlayCorrect() { PlaySfx(correctSound); }
    public void PlayIncorrect() { PlaySfx(incorrectSound); }
    public void PlayTick() { PlaySfx(tickSound); }
    public void PlayTimeUp() { PlaySfx(timeUpSound); }
    public void PlayFinish() { PlaySfx(finishSound); }
    public void PlayPerfect() { PlaySfx(perfectSound); }
    public void PlayOpenSettings() { PlaySfx(openSettingsSound); }
    public void PlayCloseSettings() { PlaySfx(closeSettingsSound); }
    public void PlayExit() { PlaySfx(exitSound); }

    // ---------- VOLUMEN (lo usan los sliders de Settings) ----------

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("MusicVolume", volume); // Se guarda para la próxima vez
    }

    public void SetSfxVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.6f);
    }

    public float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat("SfxVolume", 1f);
    }
}