using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameManagerConexion : MonoBehaviour
{
    public static GameManagerConexion instancia;
    public int puntos;

    


    [Serializable]
    public struct PlayerData
    {
        public string nombre;
        public int puntos;
        public string puesto;
        public string expertise;
        public string usuarioDepartamento;

        public List<Question> preguntas;
        public PlayerData(string nombre, int puntos, string puesto, string expertise, string usuarioDepartamento)
        {
            this.nombre = nombre;
            this.puntos = puntos;
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
        

        public Question(string question, string relatedDepartment)
        {
            this.question = question;
            this.relatedDepartment = relatedDepartment;
        }

        public Question(string question)
        {
            this.question = question;
            this.relatedDepartment = "";
        }
    }

   

    public PlayerData usuario;

    public string escenaAnterior = "";
    string escenaActual = "";


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

    


}