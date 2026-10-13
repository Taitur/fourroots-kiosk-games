using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonoesMenu : MonoBehaviour
{
        public void Juego1()
    {
        SceneManager.LoadScene("Menu_SP");
    }

        public void Juego2()
    {
        SceneManager.LoadScene("Menu");
    }

        public void Juego3()
    {
        SceneManager.LoadScene("R_Escena_Menu");
    }

        public void Juego4()
    {
        SceneManager.LoadScene("IAvventuraScene");
    }
    
    public void Juego5()
    {
        SceneManager.LoadScene("CA_Escena_Menu");
    }
}
