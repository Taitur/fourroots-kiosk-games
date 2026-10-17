using UnityEngine;
using UnityEngine.SceneManagement;

public class Triggers : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
           SceneManager.LoadScene("MapaDepartamentos");
        }
    }
}
