using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Muestra los puntos acumulados (SC_Puntos.Total) abajo al centro de la pantalla.
// Se crea su propio canvas, asi que basta con ponerlo en un objeto vacio de la escena.
public class SC_PuntosDisplay : MonoBehaviour
{
    public TMP_FontAsset fuente;
    public string prefijo = "PUNTOS: ";
    public float tamanoTexto = 64f;
    public Color colorTexto = new Color(1f, 0.85f, 0.3f);

    [Tooltip("Solo para pruebas: id de jugador a usar si el menu del kiosko no inicio sesion. 0 = ninguno.")]
    public int jugadorIdPrueba = 0;

    TextMeshProUGUI texto;

    void Start()
    {
        GameObject canvasObj = new GameObject("Puntos_Canvas");
        canvasObj.transform.SetParent(transform, false);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;
        CanvasScaler escala = canvasObj.AddComponent<CanvasScaler>();
        escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escala.referenceResolution = new Vector2(1920f, 1080f);
        escala.matchWidthOrHeight = 0.5f;

        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvasObj.transform, false);
        Image fondo = panel.AddComponent<Image>();
        fondo.color = new Color(0.04f, 0.05f, 0.1f, 0.7f);
        fondo.raycastTarget = false;
        RectTransform rp = panel.GetComponent<RectTransform>();
        rp.anchorMin = new Vector2(0.5f, 0f);
        rp.anchorMax = new Vector2(0.5f, 0f);
        rp.pivot = new Vector2(0.5f, 0f);
        rp.anchoredPosition = new Vector2(0f, 40f);
        rp.sizeDelta = new Vector2(760f, 120f);

        GameObject t = new GameObject("Texto", typeof(RectTransform));
        t.transform.SetParent(panel.transform, false);
        texto = t.AddComponent<TextMeshProUGUI>();
        if (fuente != null) texto.font = fuente;
        texto.fontSize = tamanoTexto;
        texto.color = colorTexto;
        texto.alignment = TextAlignmentOptions.Center;
        texto.raycastTarget = false;
        RectTransform rt = texto.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(20f, 10f);
        rt.offsetMax = new Vector2(-20f, -10f);

        Actualizar();

        if (!SC_Sesion.Activa && jugadorIdPrueba > 0) SC_Sesion.JugadorId = jugadorIdPrueba;
        StartCoroutine(CargarPuntos());
    }

    System.Collections.IEnumerator CargarPuntos()
    {
        yield return SC_Puntos.Cargar();
        Actualizar();
    }

    void Actualizar()
    {
        if (texto != null) texto.text = prefijo + SC_Puntos.Total;
    }
}
