using UnityEngine;

/// <summary>Controlador de SC_MapaDepartamentos (6 salas seleccionables).</summary>
public class MapaDepartamentosController : MonoBehaviour
{
    // Colgar del OnClick de cada botón de sala, con el nombre exacto del depto.
    public void VisitarDepartamento(string departamento)
    {
        SceneRouter.VisitarOficina(departamento);
    }

    // Colgar del OnClick de "Volver a mi oficina".
    public void VolverAMiOficina()
    {
        SceneRouter.ReturnToOffice();
    }
}
