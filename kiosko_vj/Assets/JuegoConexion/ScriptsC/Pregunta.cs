using UnityEngine;

public class Pregunta : MonoBehaviour
{
    
  

   void OnCollisionEnter2D(Collision2D collision)
    {
    Debug.Log("Choqué con: " + collision.gameObject.name);
    }
}
