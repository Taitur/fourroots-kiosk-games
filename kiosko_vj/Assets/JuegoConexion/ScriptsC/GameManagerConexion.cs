using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;



public class GameManagerConexion : MonoBehaviour
{
    public static GameManagerConexion instancia;


    [Serializable]
    public struct PlayerData
    {
        public string nombre;
        public int puntos;
        public string puesto;
        public string expertise;
        public string usuarioDepartamento;

        public PlayerData(string nombre, string puesto, string expertise, string usuarioDepartamento)
        {
            this.nombre = nombre;
            this.puntos = 0;
            this.puesto = puesto;
            this.expertise = expertise;
            this.usuarioDepartamento = usuarioDepartamento;
        }


    }

    public struct Question
    {
        public int id;
        public string question;
        public string relatedDepartment;

        public string response;

        public Transform punto;

        public GameObject relatedPrefab;

        public string relatedPlayerName;

        public string responsePlayerName;

        public string oficinaId;    
        public int slotIndex;  
        


         public Question(string question, string relatedDepartment, string name, string offiName, int idxslot)
        {
            this.question = question;
            this.relatedDepartment = relatedDepartment;
            this.response = "";
            this.punto = null;
            relatedPlayerName = name;
            oficinaId = offiName;
            slotIndex = idxslot;
            responsePlayerName = "";
            relatedPrefab = null;
            id = 0;
  
        }

        public Question Reaparecer(GameObject prefab, Transform nuevoPunto)
        {
            Question q = this;                        
            q.punto = nuevoPunto;
            q.relatedPrefab = Instantiate(
                prefab, nuevoPunto.position, Quaternion.identity, nuevoPunto.parent);
            q.relatedPrefab.GetComponent<Pregunta>().preguntaData = q;
            return q;
        }

        public void Aparecer(GameObject prefab, Transform punto)
        {
            this.punto = punto;
            relatedPrefab = UnityEngine.Object.Instantiate(prefab, punto.position, UnityEngine.Quaternion.identity, punto.parent);
            relatedPrefab.GetComponent<Pregunta>().preguntaData = this;
        }
    }

   

    public PlayerData usuario;

    public string escenaAnterior = "";
    public string escenaActual = "";
    public int jugadorId;

    AudioSource audioSource;

    void Awake()
    {
        if (instancia != null) { Destroy(gameObject); return; }
        instancia = this;
        DontDestroyOnLoad(gameObject);
        escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += AlCambiarEscena;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void ReproducirSFX(string nombreClip)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/" + nombreClip));
    }

    void OnDestroy()
    {   
        if (instancia == this)
            SceneManager.activeSceneChanged -= AlCambiarEscena;
    }

    void AlCambiarEscena(Scene anterior, Scene nueva)
    {
        escenaAnterior = escenaActual;   
        escenaActual = nueva.name;

        // Al volver al menu principal del kiosko se destruye, para que su musica no siga sonando en los demas juegos.
        if (nueva.name == "MenuPrincipal") Destroy(gameObject);
    }


    public static Question DesdeDTO(PreguntaDTO d)
    {
        var q = new Question(d.question, d.relatedDepartment, d.relatedPlayerName, d.oficinaId, d.slotIndex);
        q.id = d.id;
        q.response = d.response;
        q.responsePlayerName = d.responsePlayerName;
        return q;
    }

    


}