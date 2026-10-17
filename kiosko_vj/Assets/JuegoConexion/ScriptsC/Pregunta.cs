using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class Pregunta : MonoBehaviour
{
  
    Animator anim;
    AudioSource audioSource;

    public GameObject panelResponse;


    Rigidbody2D cuerpo;

    public GameManagerConexion.Question preguntaData;




    void Start()
    {
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

   void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("IsTouching", true);
        anim.Play("PreguntaInteractuable");
        audioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/Interactuable"));
        cuerpo = collision.gameObject.GetComponent<Rigidbody2D>();


    }



    void OnCollisionExit2D(Collision2D colission)
    {
        anim.SetBool("IsTouching", false);
       
    }

    void Update()
    {
        if (anim.GetBool("IsTouching") && Input.GetKeyDown(KeyCode.E))
        {
            PreguntaManager.Instancia.MostrarPanel(preguntaData);
            cuerpo.bodyType = RigidbodyType2D.Static;
        }
       
    }

}
