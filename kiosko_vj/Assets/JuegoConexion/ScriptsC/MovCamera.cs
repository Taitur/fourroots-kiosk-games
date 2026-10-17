using UnityEngine;

public class MovCamera : MonoBehaviour
{
    public Transform jugador;
    public float velocidadTransicion = 8f;

    Camera cam;
    Vector3 destino;

    void Start()
    {
        cam = GetComponent<Camera>();
        destino = transform.position;
    }

    void LateUpdate()
    {
        float mitadAlto = cam.orthographicSize;
        float mitadAncho = mitadAlto * cam.aspect;
        Vector3 p = jugador.position;

        if (p.x > destino.x + mitadAncho)      destino.x += mitadAncho * 2;
        else if (p.x < destino.x - mitadAncho) destino.x -= mitadAncho * 2;

        if (p.y > destino.y + mitadAlto)       destino.y += mitadAlto * 2;
        else if (p.y < destino.y - mitadAlto)  destino.y -= mitadAlto * 2;

        transform.position = Vector3.Lerp(transform.position, destino, velocidadTransicion * Time.deltaTime);
    }
}