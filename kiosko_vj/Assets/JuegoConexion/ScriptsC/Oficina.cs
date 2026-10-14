using UnityEngine;
using System.Collections.Generic;

public class Oficina : MonoBehaviour
{
   public GameManagerConexion gameManager;

   public List<GameManagerConexion.Question> preguntas;

    void Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();
    }

    public void AgregarPregunta(GameManagerConexion.Question pregunta)
    {
        preguntas.Add(pregunta);
    }
}
