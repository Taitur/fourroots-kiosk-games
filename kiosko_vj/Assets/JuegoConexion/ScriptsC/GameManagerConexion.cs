using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

using System.Numerics;

public class GameManagerConexion : MonoBehaviour
{
    public static GameManagerConexion instancia;
    public int puntos;

    public struct GameData
    {
        public List<PlayerData> jugadores;

        public UserSession sesion;

       

        public GameData(string name)
        {
            
            jugadores = new List<PlayerData>();
            sesion = new UserSession(name);
        }

    
    }

    [Serializable]
    public struct UserSession
    {
        public string nombre;

        public UserSession(string nombre)
        {
            this.nombre = nombre;
           
        }

        public void Cerrar()
        {
            this.nombre = "";
        }

    }

    [Serializable]
    public struct PlayerData
    {
        public string nombre;
        public int puntos;
        public string puesto;
        public string expertise;
        public string usuarioDepartamento;

        public List<Question> preguntas;
        public PlayerData(string nombre, string puesto, string expertise, string usuarioDepartamento)
        {
            this.nombre = nombre;
            this.puntos = 0;
            this.puesto = puesto;
            this.expertise = expertise;
            this.usuarioDepartamento = usuarioDepartamento;
            this.preguntas = new List<Question>();
        }

        public PlayerData(string departamento)
        {
            this.nombre = "";
            this.puntos = 0;
            this.puesto = "";
            this.expertise = "";
            this.usuarioDepartamento = departamento;
            this.preguntas = new List<Question>();
        }
    }

    public struct Question
    {
        public string question;
        public string relatedDepartment;

        public string response;

        public Transform punto;

        public GameObject relatedPrefab;

        public string relatedPlayerName;

        public string responsePlayerName;

        public string oficinaId;    
        public int slotIndex;  
        

        public Question(string question, string relatedDepartment)
        {
            this.question = question;
            this.relatedDepartment = relatedDepartment;
            this.response = "";
            relatedPrefab = null;
            punto = null;
            relatedPlayerName = "";
            oficinaId = "";
            slotIndex = 0;
            responsePlayerName = "";
        }

        public Question(string question)
        {
            this.question = question;
            this.relatedDepartment = "";
            this.response = "";
            relatedPrefab = null;
            punto = null;
            relatedPlayerName = "";
            oficinaId = "";
            slotIndex = 0;
            responsePlayerName = "";
        }

        public Question(string question, string relatedDepartment, GameObject prefab, Transform punto)
        {
            this.question = question;
            this.relatedDepartment = relatedDepartment;
            this.response = "";
            relatedPrefab = UnityEngine.Object.Instantiate(prefab, punto.position, UnityEngine.Quaternion.identity, punto.parent);
            this.punto = punto;
            relatedPlayerName = "";
            oficinaId = "";
            slotIndex = 0;
            responsePlayerName = "";
            relatedPrefab.GetComponent<Pregunta>().preguntaData = this;
           
  
        }

         public Question(string question, string relatedDepartment, GameObject prefab, Transform punto, string name, string offiName, int idxslot)
        {
            this.question = question;
            this.relatedDepartment = relatedDepartment;
            this.response = "";
            relatedPrefab = UnityEngine.Object.Instantiate(prefab, punto.position, UnityEngine.Quaternion.identity, punto.parent);
            this.punto = punto;
            relatedPlayerName = name;
            oficinaId = offiName;
            slotIndex = idxslot;
            responsePlayerName = "";
            relatedPrefab.GetComponent<Pregunta>().preguntaData = this;
           
  
        }

        public Question Reaparecer(GameObject prefab, Transform nuevoPunto)
        {
            Question q = this;                        // copia de los datos
            q.punto = nuevoPunto;
            q.relatedPrefab = UnityEngine.Object.Instantiate(
                prefab, nuevoPunto.position, UnityEngine.Quaternion.identity, nuevoPunto.parent);
            q.relatedPrefab.GetComponent<Pregunta>().preguntaData = q;
            return q;
        }
    }

   

    public PlayerData usuario;

    public GameData gameData;

    public string escenaAnterior = "";
    public string escenaActual = "";



    bool iniciado = false;

    public void IniciarSesion()
    {
        if (iniciado) return;       // evita crear los datos dos veces si pulsan Enviar dos veces
        iniciado = true;

        gameData = new GameData(usuario.nombre);
        gameData.jugadores.Add(usuario);
    }

    void Awake()
    {
        if (instancia != null) { Destroy(gameObject); return; }
        instancia = this;
        DontDestroyOnLoad(gameObject);
        escenaActual = SceneManager.GetActiveScene().name;  
        SceneManager.activeSceneChanged += AlCambiarEscena;
       
    }

    void OnDestroy()
    {   
        if (instancia == this)
            SceneManager.activeSceneChanged -= AlCambiarEscena;
    }

    void AlCambiarEscena(Scene anterior, Scene nueva)
    {
        escenaAnterior = escenaActual;   // el nombre que ya teniamos guardado
        escenaActual = nueva.name;
        
    }

    public bool BuscarPreguntaParaResponder(string miNombre, string miDepartamento,
                                        out GameManagerConexion.Question resultado)
    {
        resultado = default;
        bool encontrada = false;
        bool esDeOtroDepto = false;

        foreach (var jugador in gameData.jugadores)
        {
            if (jugador.nombre == miNombre || jugador.preguntas == null) continue;

            foreach (var q in jugador.preguntas)
            {
                if (!string.IsNullOrEmpty(q.response)) continue;        

                bool otroDepto = q.relatedDepartment != miDepartamento;

            
                if (!encontrada || (otroDepto && !esDeOtroDepto))
                {
                    resultado = q;
                    encontrada = true;
                    esDeOtroDepto = otroDepto;
                }
            }
        }
        return encontrada;
    }

    


}