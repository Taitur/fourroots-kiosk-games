using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    IEnumerator Start()
    {
        gameManager = GameManagerConexion.instancia;
        preguntas = new List<GameManagerConexion.Question>();

        yield return ApiClient.ListarPreguntas(lista =>
        {
            foreach (var d in lista)
            {
                if (d.oficinaId != gameObject.name) continue;
                if (d.relatedDepartment != SceneManager.GetActiveScene().name) continue;
                if (!string.IsNullOrEmpty(d.response)) continue;

                var q = GameManagerConexion.DesdeDTO(d);
                Transform punto = puntos[q.slotIndex];
                q.Reaparecer(prefabPreg, punto);
            }
        });
    }

    public void AgregarPregunta(GameManagerConexion.Question pregunta)
    {
        preguntas.Add(pregunta);
    }
}
