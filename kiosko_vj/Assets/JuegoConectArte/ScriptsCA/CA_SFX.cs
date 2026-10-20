using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Música de fondo + 2 sonidos de botón.
/// Es un singleton que NO se destruye al cambiar de escena, así la música
/// sigue sonando sin cortarse. Ponlo en un objeto RAÍZ (sin padre) de la primera escena.
/// Si lo dejas también en las demás escenas (para probarlas por separado),
/// el repetido se destruye solo.
/// </summary>
public class CA_SFX : MonoBehaviour
{
    public static CA_SFX Instance { get; private set; }

    [Header("Audio (arrastra tus archivos aquí)")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonSound1;
    [SerializeField] private AudioClip buttonSound2;

    [Header("Volumen")]
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.4f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // ya existe uno (viene de la escena anterior)
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += AlCambiarEscena;

        // Un AudioSource para la música (en loop) y otro para los efectos
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;

        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // Al salir de ConectArte (escenas "CA_...", p. ej. de vuelta al MenuPrincipal) se destruye
    // para que la música no siga sonando en el menú ni en los demás juegos.
    private void AlCambiarEscena(Scene anterior, Scene nueva)
    {
        if (!nueva.name.StartsWith("CA_")) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        SceneManager.activeSceneChanged -= AlCambiarEscena;
        Instance = null;
    }

    public void PlayButtonSound1() => PlaySfx(buttonSound1);
    public void PlayButtonSound2() => PlaySfx(buttonSound2);

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);   // permite que se encimen si presionan rápido
    }

    // ---------- Volumen (por si luego agregas sliders de opciones) ----------
    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        if (musicSource != null) musicSource.volume = musicVolume;
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
    }
}