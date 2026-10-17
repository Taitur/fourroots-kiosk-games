using System.Collections.Generic;
using UnityEngine;

// Define un cuarto: un rectangulo que limita hasta donde puede mover la camara.
// Mueve el objeto para posicionarlo y cambia "tamano" para redimensionarlo.
// El rectangulo amarillo se ve en la Scene view.
public class SC_Cuarto : MonoBehaviour
{
    public Vector2 tamano = new Vector2(17.84f, 10f);

    // Opcional: donde reaparece el jugador en este cuarto. Si esta vacio, reaparece donde entro.
    public Transform reaparicion;

    public static readonly List<SC_Cuarto> Todos = new List<SC_Cuarto>();

    public Rect Limites
    {
        get
        {
            Vector2 centro = transform.position;
            return new Rect(centro - tamano * 0.5f, tamano);
        }
    }

    void OnEnable() { if (!Todos.Contains(this)) Todos.Add(this); }
    void OnDisable() { Todos.Remove(this); }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.9f, 0.2f, 0.9f);
        Gizmos.DrawWireCube(transform.position, new Vector3(tamano.x, tamano.y, 0f));
        if (reaparicion != null) Gizmos.DrawWireSphere(reaparicion.position, 0.3f);
    }
}
