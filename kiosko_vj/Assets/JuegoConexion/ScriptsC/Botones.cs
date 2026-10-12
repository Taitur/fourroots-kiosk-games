using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void EntrarAlJuego()
    {
        SceneManager.LoadScene(5);
    }

    public void SalirAlMenuMinijuego()
    {
        SceneManager.LoadScene(1);
    }

}
