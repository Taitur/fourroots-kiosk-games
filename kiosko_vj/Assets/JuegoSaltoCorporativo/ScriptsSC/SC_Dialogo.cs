using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// Personaje con el que se puede hablar. Cuando el jugador esta cerca aparece una "E" encima;
// al presionar E se abre el cuadro de dialogo y mientras tanto el jugador no se mueve.
// Presiona E de nuevo para avanzar o cerrar.
public class SC_Dialogo : MonoBehaviour
{
    public static bool EnDialogo;

    public TMP_FontAsset fuente;
    public string nombre = "Maceta";
    [TextArea] public string[] lineas = { "Hello world" };
    public float distanciaInteraccion = 2f;
    public Vector3 offsetIcono = new Vector3(0f, 1.8f, 0f);
    public float letrasPorSegundo = 40f;

    Transform jugador;
    TextMeshPro icono;
    GameObject canvasObj;
    TextMeshProUGUI textoDialogo;
    int indice;
    float letrasMostradas;
    bool abierto;

    void Start()
    {
        CrearIcono();
        CrearDialogo();
        canvasObj.SetActive(false);
    }

    void OnDisable() { Cerrar(); }
    void OnDestroy() { if (canvasObj != null) Destroy(canvasObj); }

    void Update()
    {
        if (jugador == null)
        {
            GameObject j = GameObject.FindGameObjectWithTag("Player");
            if (j != null) jugador = j.transform;
        }

        bool cerca = jugador != null &&
            Vector2.Distance(transform.position, jugador.position) <= distanciaInteraccion;

        if (icono != null)
        {
            icono.gameObject.SetActive(cerca && !abierto);
            icono.transform.position = transform.position + offsetIcono +
                new Vector3(0f, Mathf.Sin(Time.time * 4f) * 0.06f, 0f);
        }

        if (!PresionoInteractuar()) { if (abierto) Escribir(); return; }

        if (!abierto)
        {
            if (cerca) Abrir();
        }
        else if (letrasMostradas < textoDialogo.textInfo.characterCount)
        {
            letrasMostradas = textoDialogo.textInfo.characterCount; // muestra todo el texto
            textoDialogo.maxVisibleCharacters = (int)letrasMostradas;
        }
        else
        {
            indice++;
            if (indice >= lineas.Length) Cerrar();
            else MostrarLinea();
        }
    }

    bool PresionoInteractuar()
    {
        Keyboard kb = Keyboard.current;
        if (kb != null && kb.eKey.wasPressedThisFrame) return true;
        Gamepad gp = Gamepad.current;
        return gp != null && gp.buttonNorth.wasPressedThisFrame;
    }

    void Abrir()
    {
        if (lineas == null || lineas.Length == 0) return;
        abierto = true;
        EnDialogo = true;
        indice = 0;
        canvasObj.SetActive(true);
        MostrarLinea();
    }

    void Cerrar()
    {
        abierto = false;
        EnDialogo = false;
        if (canvasObj != null) canvasObj.SetActive(false);
    }

    void MostrarLinea()
    {
        textoDialogo.text = lineas[indice];
        textoDialogo.ForceMeshUpdate();
        letrasMostradas = 0f;
        textoDialogo.maxVisibleCharacters = 0;
    }

    void Escribir()
    {
        int total = textoDialogo.textInfo.characterCount;
        if (letrasMostradas >= total) return;
        letrasMostradas += letrasPorSegundo * Time.deltaTime;
        textoDialogo.maxVisibleCharacters = Mathf.Min(total, (int)letrasMostradas);
    }

    void CrearIcono()
    {
        GameObject go = new GameObject("Icono_E");
        go.transform.SetParent(transform, false);
        icono = go.AddComponent<TextMeshPro>();
        if (fuente != null) icono.font = fuente;
        icono.text = "E";
        icono.fontSize = 6f;
        icono.alignment = TextAlignmentOptions.Center;
        icono.color = Color.white;
        icono.sortingOrder = 30;
        icono.rectTransform.sizeDelta = new Vector2(2f, 1f);
        go.SetActive(false);
    }

    void CrearDialogo()
    {
        canvasObj = new GameObject("Dialogo_Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        CanvasScaler escala = canvasObj.AddComponent<CanvasScaler>();
        escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escala.referenceResolution = new Vector2(1920f, 1080f);
        escala.matchWidthOrHeight = 0.5f;

        GameObject panel = NuevoUI("Panel", canvasObj.transform);
        Image fondo = panel.AddComponent<Image>();
        fondo.color = new Color(0.04f, 0.05f, 0.1f, 0.88f);
        RectTransform rp = panel.GetComponent<RectTransform>();
        rp.anchorMin = new Vector2(0.08f, 0.04f);
        rp.anchorMax = new Vector2(0.92f, 0.30f);
        rp.offsetMin = Vector2.zero;
        rp.offsetMax = Vector2.zero;

        TextMeshProUGUI nombreTxt = NuevoTexto("Nombre", panel.transform, 46f,
            new Color(1f, 0.85f, 0.3f), TextAlignmentOptions.TopLeft);
        nombreTxt.text = nombre;
        Rellenar(nombreTxt.rectTransform, new Vector2(40f, 0f), new Vector2(-40f, -22f));

        textoDialogo = NuevoTexto("Texto", panel.transform, 56f, Color.white, TextAlignmentOptions.TopLeft);
        Rellenar(textoDialogo.rectTransform, new Vector2(40f, 24f), new Vector2(-40f, -90f));

        TextMeshProUGUI pista = NuevoTexto("Pista", panel.transform, 34f,
            new Color(1f, 1f, 1f, 0.6f), TextAlignmentOptions.BottomRight);
        pista.text = "[E]";
        Rellenar(pista.rectTransform, new Vector2(40f, 18f), new Vector2(-40f, 0f));
    }

    static GameObject NuevoUI(string nombreObj, Transform padre)
    {
        GameObject go = new GameObject(nombreObj, typeof(RectTransform));
        go.transform.SetParent(padre, false);
        return go;
    }

    TextMeshProUGUI NuevoTexto(string nombreObj, Transform padre, float tam, Color color, TextAlignmentOptions alin)
    {
        GameObject go = NuevoUI(nombreObj, padre);
        TextMeshProUGUI t = go.AddComponent<TextMeshProUGUI>();
        if (fuente != null) t.font = fuente;
        t.fontSize = tam;
        t.color = color;
        t.alignment = alin;
        t.raycastTarget = false;
        return t;
    }

    static void Rellenar(RectTransform r, Vector2 offsetMin, Vector2 offsetMax)
    {
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = offsetMin;
        r.offsetMax = offsetMax;
    }
}
