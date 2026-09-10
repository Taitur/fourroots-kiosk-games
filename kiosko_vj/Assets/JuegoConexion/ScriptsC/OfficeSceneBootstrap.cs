using UnityEngine;

/// <summary>
/// Colocar vacío en cada escena SC_Oficina_*. Fija el departamento visitado y
/// asegura que el HUD (y el overlay de match si procede) estén cargados.
/// </summary>
public class OfficeSceneBootstrap : MonoBehaviour
{
    [Tooltip("Departamento que representa ESTA escena, p.ej. 'Marketing'.")]
    public string departmentId;

    private void Start()
    {
        SceneRouter.LoadAdditiveIfNeeded(SceneRouter.HudDudas);

        bool visitando = departmentId != LocalGameData.Perfil.departamento;
        if (!visitando && LocalGameData.TieneDudasPendientes())
            SceneRouter.LoadAdditiveIfNeeded(SceneRouter.OverlayMatch);
        else
            SceneRouter.UnloadAdditive(SceneRouter.OverlayMatch);
    }

    public bool EsVisita() => departmentId != LocalGameData.Perfil.departamento;
}
