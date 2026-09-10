using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador de Overlay_Match (additive, barra superior). Único lugar donde
/// se revela la duda/skill de otro perfil, y solo del match del usuario actual.
/// </summary>
public class MatchOverlayController : MonoBehaviour
{
    public Text barraTexto;
    public GameObject panelHilo;
    public Text hiloDudaText;
    public Text hiloSkillText;
    public Text hiloConQuienText;

    private DudaEntry _dudaActual;

    private void OnEnable()
    {
        _dudaActual = LocalGameData.UltimaDudaConMatchNoAbierto();
        if (_dudaActual == null) { gameObject.SetActive(false); return; }
        barraTexto.text = $"{_dudaActual.match.nombreQuienResponde} ({_dudaActual.match.departamentoQuienResponde}) puede responder tu duda";
    }

    // Colgar del OnClick de "Abrir hilo".
    public void AbrirHilo()
    {
        if (_dudaActual == null) return;
        panelHilo.SetActive(true);
        hiloConQuienText.text = $"{_dudaActual.match.nombreQuienResponde} · {_dudaActual.match.departamentoQuienResponde}";
        hiloDudaText.text = _dudaActual.texto;
        hiloSkillText.text = _dudaActual.match.motivoSkill;
        _dudaActual.match.hiloAbierto = true;
        LocalGameData.Save();
    }

    // Colgar del OnClick de "Cerrar" dentro del hilo.
    public void CerrarHilo()
    {
        panelHilo.SetActive(false);
        SceneRouter.UnloadAdditive(SceneRouter.OverlayMatch);
    }

    // Colgar del OnClick de la "X" de la barra (descartar sin leer).
    public void Descartar() => SceneRouter.UnloadAdditive(SceneRouter.OverlayMatch);
}
