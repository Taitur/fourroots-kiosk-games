using UnityEngine;
using UnityEngine.SceneManagement;

public class CA_Botones : MonoBehaviour
{
        public void Menu()
    {
        SceneManager.LoadScene(0);
    }
        public void Instrucciones()
    {
        SceneManager.LoadScene(2);
    }
}
