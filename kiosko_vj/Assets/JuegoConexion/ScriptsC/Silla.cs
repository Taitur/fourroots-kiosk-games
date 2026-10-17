using UnityEngine;


public class Silla : MonoBehaviour
{

    Animator anim;
    AudioSource audioSource;

    Rigidbody2D cuerpo;
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
        anim.SetBool("IsTouchingChair", true);
        anim.Play("SillaInteractuable");
        audioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/Interactuable"));
        cuerpo = collision.gameObject.GetComponent<Rigidbody2D>();


    }



    void OnCollisionExit2D(Collision2D colission)
    {
        anim.SetBool("IsTouchingChair", false);
       
    }

    void Update()
    {
        if (anim.GetBool("IsTouchingChair") && Input.GetKeyDown(KeyCode.E))
        {
            
        }
       
    }


}
