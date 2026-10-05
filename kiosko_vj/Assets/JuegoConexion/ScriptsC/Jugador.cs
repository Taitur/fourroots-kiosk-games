using UnityEngine;

public class Jugador : MonoBehaviour
{
    Animator anim;
    Rigidbody2D cuerpo;
    float velocidad = 5f;

    bool piso;
    public Transform pies;
    public LayerMask listaPiso;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        cuerpo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direccion = new Vector2(horizontal, vertical).normalized;
        cuerpo.linearVelocity = direccion * velocidad;

        if (horizontal < 0)
            anim.Play("JugadorCaminaIzq");
        else if (horizontal > 0)
            anim.Play("JugadorCaminaDer");
        else if (vertical != 0)
            anim.Play("JugadorCamina");
        else
            anim.Play("JugadorEspera");
        }
}
