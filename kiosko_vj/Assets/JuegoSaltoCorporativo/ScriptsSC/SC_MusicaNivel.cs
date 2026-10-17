using UnityEngine;

// Musica de fondo del nivel: suena en loop y baja de volumen para no tapar los efectos.
public class SC_MusicaNivel : MonoBehaviour
{
    public AudioClip musica;
    [Range(0f, 1f)] public float volumen = 0.3f;

    void Start()
    {
        if (musica == null) return;
        AudioSource fuente = gameObject.AddComponent<AudioSource>();
        fuente.clip = musica;
        fuente.loop = true;
        fuente.volume = volumen;
        fuente.spatialBlend = 0f;
        fuente.playOnAwake = false;
        fuente.Play();
    }
}
