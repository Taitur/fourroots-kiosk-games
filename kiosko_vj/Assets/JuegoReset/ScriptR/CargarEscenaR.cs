using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenaR : MonoBehaviour
{
    public void IrAEscenaJuego()
    {
        SceneManager.LoadScene("R_Escena_Juego");
    }

    public void IrAEscenaMenu()
    {
        SceneManager.LoadScene("R_Escena_Menu");
    }

    public void IrAEscenaRespira()
    {
        SceneManager.LoadScene("R_Escena_Respira");
    }

    public void IrAMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}