using UnityEngine;

// Camara tipo Celeste: sigue al jugador pero sin salirse del cuarto actual.
// Al pasar a otro cuarto se desliza suavemente hacia el.
[RequireComponent(typeof(Camera))]
public class SC_CamaraCuartos : MonoBehaviour
{
    public Transform objetivo;
    public Vector2 desfaseObjetivo = new Vector2(0f, 1f);
    public float suavizadoSeguimiento = 0.12f;
    public float duracionTransicion = 0.3f;

    Camera cam;
    SC_Cuarto actual;
    Vector3 velocidad;
    float tiempoTransicion;
    bool primerFrame = true;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (objetivo == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador == null) return;
            objetivo = jugador.transform;
        }

        Vector2 pos = objetivo.position;
        SC_Cuarto nuevo = BuscarCuarto(pos);
        if (nuevo != null && nuevo != actual)
        {
            if (actual != null) tiempoTransicion = duracionTransicion;
            actual = nuevo;
        }

        Vector3 destino = CalcularDestino(pos + desfaseObjetivo);

        if (primerFrame)
        {
            transform.position = destino;
            primerFrame = false;
            return;
        }

        float suavizado = tiempoTransicion > 0f ? duracionTransicion : suavizadoSeguimiento;
        tiempoTransicion -= Time.deltaTime;
        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velocidad, suavizado);
    }

    // Conserva el cuarto actual mientras el jugador siga dentro. Si sale de todos, usa el mas cercano.
    SC_Cuarto BuscarCuarto(Vector2 pos)
    {
        if (actual != null && actual.Limites.Contains(pos)) return actual;

        SC_Cuarto masCercano = null;
        float mejor = float.MaxValue;
        foreach (SC_Cuarto c in SC_Cuarto.Todos)
        {
            Rect r = c.Limites;
            if (r.Contains(pos)) return c;
            Vector2 punto = new Vector2(Mathf.Clamp(pos.x, r.xMin, r.xMax), Mathf.Clamp(pos.y, r.yMin, r.yMax));
            float d = (punto - pos).sqrMagnitude;
            if (d < mejor) { mejor = d; masCercano = c; }
        }
        return masCercano != null ? masCercano : actual;
    }

    Vector3 CalcularDestino(Vector2 deseado)
    {
        float z = transform.position.z;
        if (actual == null) return new Vector3(deseado.x, deseado.y, z);

        Rect r = actual.Limites;
        float mitadAlto = cam.orthographicSize;
        float mitadAncho = mitadAlto * cam.aspect;

        float x = r.width <= mitadAncho * 2f ? r.center.x : Mathf.Clamp(deseado.x, r.xMin + mitadAncho, r.xMax - mitadAncho);
        float y = r.height <= mitadAlto * 2f ? r.center.y : Mathf.Clamp(deseado.y, r.yMin + mitadAlto, r.yMax - mitadAlto);
        return new Vector3(x, y, z);
    }
}
