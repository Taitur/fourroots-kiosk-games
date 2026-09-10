using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonoesMenu : MonoBehaviour
{
        public void Juego1()
    {
        SceneManager.LoadScene(6);
    }

        public void Juego2()
    {
        SceneManager.LoadScene(4);
    }

        public void Juego3()
    {
        SceneManager.LoadScene(3);
    }

        public void Juego4()
    {
        SceneManager.LoadScene(5);
    }
    
    public void Juego5()
    {
        SceneManager.LoadScene(1);
    }
}
