using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Punto único de navegación entre escenas. Mismo espíritu que
/// BotonoesMenu.cs / CA_Botones.cs del repo, pero por nombre (no índice)
/// porque aquí hay 6 oficinas equivalentes.
/// </summary>
public static class SceneRouter
{
    public const string CrearPerfil = "SC_CrearPerfil";
    public const string CrearDuda = "SC_CrearDuda";
    public const string MapaDepartamentos = "SC_MapaDepartamentos";
    public const string HudDudas = "HUD_Dudas";
    public const string OverlayMatch = "Overlay_Match";

    public static string OficinaDe(string departamento) => "SC_Oficina_" + departamento;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void LoadDudaScene()
    {
        SceneManager.LoadScene(CrearDuda, LoadSceneMode.Single);
    }

    /// <summary>Vuelve a la oficina del departamento del perfil actual.</summary>
    public static void ReturnToOffice()
    {
        var depto = LocalGameData.Perfil.departamento;
        SceneManager.LoadScene(OficinaDe(string.IsNullOrEmpty(depto) ? "Marketing" : depto), LoadSceneMode.Single);
    }

    public static void VisitarOficina(string departamento)
    {
        SceneManager.LoadScene(OficinaDe(departamento), LoadSceneMode.Single);
    }

    public static void LoadAdditiveIfNeeded(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public static void UnloadAdditive(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }
}
