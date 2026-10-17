using UnityEngine;
using UnityEngine.SceneManagement;

public class CA_Botones : MonoBehaviour
{
        public void Menu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
        public void Instrucciones()
    {
        SceneManager.LoadScene("CA_Escena_instrucciones");
    }
        public void Entregado()
    {
        SceneManager.LoadScene("CA_Calificar");
    }
        public void Jugar()
    {
        SceneManager.LoadScene("CA_EScena_juego");
    }
}
