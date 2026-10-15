using UnityEngine;
using System.Collections.Generic;

public class Oficina : MonoBehaviour
{
   public GameManagerConexion gameManager;

   public List<GameManagerConexion.Question> preguntas;

   public Transform[] puntos;

   public Transform[] ocupado;

   public GameObject prefabPreg;

    void Awake()
    {
        ocupado = new Transform[puntos.Length];
    }

    public Transform PuntoLibre()
    {
        for (int i = 0; i < puntos.Length; i++)
        {
            if (!ocupado[i])
            {
                ocupado[i] = puntos[i];
                return puntos[i];
            }
        }
        return null;                  
    }

    public void Liberar(Transform punto)
    {
        int i = System.Array.IndexOf(puntos, punto);
        if (i >= 0) ocupado[i] = null;
    }

    void Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();
        var datos = gameManager.gameData;

        

        foreach (var jugador in datos.jugadores)
        {
            if (jugador.preguntas == null) continue;

            for (int i = 0; i < jugador.preguntas.Count; i++)
            {
                var q = jugador.preguntas[i];

                if (q.oficinaId != gameObject.name) continue; 

                Transform punto = puntos[q.slotIndex];         

                jugador.preguntas[i] = q.Reaparecer(prefabPreg, punto);
            }
                
        }

    }

    public void AgregarPregunta(GameManagerConexion.Question pregunta)
    {
        preguntas.Add(pregunta);
    }
}
