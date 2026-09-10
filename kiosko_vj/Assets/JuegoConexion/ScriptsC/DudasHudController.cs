using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador de HUD_Dudas (additive, raíl derecho persistente).
/// Solo muestra las dudas del propio usuario.
/// </summary>
public class DudasHudController : MonoBehaviour
{
    public GameObject panelDudas;
    public Text contadorBadge;
    public Transform listaContainer;
    public GameObject filaDudaPrefab; // prefab con un Text para "texto" y otro para "estado/meta"

    private void OnEnable() => Refrescar();

    public void TogglePanel()
    {
        panelDudas.SetActive(!panelDudas.activeSelf);
        if (panelDudas.activeSelf) Refrescar();
    }

    public void Refrescar()
    {
        var dudas = LocalGameData.Perfil.dudas;
        contadorBadge.text = dudas.Count.ToString();
        contadorBadge.gameObject.SetActive(dudas.Count > 0);

        foreach (Transform child in listaContainer) Destroy(child.gameObject);
        foreach (var d in dudas)
        {
            var fila = Instantiate(filaDudaPrefab, listaContainer);
            var texts = fila.GetComponentsInChildren<Text>();
            if (texts.Length > 0) texts[0].text = d.texto;
            if (texts.Length > 1)
                texts[1].text = d.match != null
                    ? $"{d.estado} · {d.match.nombreQuienResponde} ({d.match.departamentoQuienResponde})"
                    : d.estado;
        }
    }

    // Colgar del OnClick de "Nueva duda".
    public void IrANuevaDuda() => SceneRouter.LoadDudaScene();

    // Colgar del OnClick de "Otros deptos".
    public void IrAMapaDepartamentos() => SceneRouter.LoadScene(SceneRouter.MapaDepartamentos);
}
