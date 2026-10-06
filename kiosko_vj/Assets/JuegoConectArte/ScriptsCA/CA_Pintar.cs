using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Ponlo en tu caja blanca (la que tiene el SpriteRenderer).
/// Crea una textura en tiempo de ejecución y pinta sobre ella.
/// Conecta los botones a: SetBrushSmall(), SetBrushBig(), SetEraser() y ClearCanvas().
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class DrawingCanvas : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private int textureWidth = 1024;          // la altura se calcula sola según la proporción de tu caja
    [SerializeField] private Color backgroundColor = Color.white;

    [Header("Pinceles (radio en píxeles de la textura)")]
    [SerializeField] private int smallBrushRadius = 3;
    [SerializeField] private int bigBrushRadius = 10;
    [SerializeField] private int eraserRadius = 20;
    [SerializeField] private Color brushColor = Color.black;

    [Header("Paleta (un color por cada botón, en orden: índice 0 a 5)")]
    [SerializeField] private Color[] paletteColors = new Color[]
    {
        Color.black,
        Color.red,
        new Color(1f, 0.5f, 0f),   // naranja
        Color.yellow,
        Color.green,
        Color.blue
    };

    private enum Tool { SmallBrush, BigBrush, Eraser }
    private Tool currentTool = Tool.SmallBrush;

    private SpriteRenderer sr;
    private Texture2D texture;
    private Color32[] pixels;
    private int texW, texH;
    private Vector2 localSize;      // tamaño del sprite original en unidades locales
    private Camera cam;

    private bool isDrawing;         // true si el trazo actual empezó dentro del canvas
    private bool hasLastPoint;
    private Vector2 lastPixel;
    private bool dirty;

    // ---------- Botones ----------
    private Tool lastBrush = Tool.SmallBrush;   // último pincel usado (para volver del borrador)

    public void SetBrushSmall() { currentTool = Tool.SmallBrush; lastBrush = currentTool; }
    public void SetBrushBig() { currentTool = Tool.BigBrush; lastBrush = currentTool; }
    public void SetEraser() => currentTool = Tool.Eraser;

    // Para los botones de color: en OnClick elige SetColorByIndex y escribe 0, 1, 2, 3, 4 o 5
    public void SetColorByIndex(int index)
    {
        if (index < 0 || index >= paletteColors.Length) return;
        brushColor = paletteColors[index];

        // Si estaba en el borrador, al elegir color vuelve al último pincel (el grosor no cambia)
        if (currentTool == Tool.Eraser) currentTool = lastBrush;
    }

    public void SetColor(Color c) { brushColor = c; }   // por si lo llamas desde código

    public void ClearCanvas()
    {
        Color32 bg = backgroundColor;
        for (int i = 0; i < pixels.Length; i++) pixels[i] = bg;
        dirty = true;
    }

    // ---------- Unity ----------
    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();

        // Guardamos el tamaño original de la caja para que no cambie visualmente
        localSize = sr.sprite.bounds.size;

        texW = textureWidth;
        texH = Mathf.RoundToInt(texW * (localSize.y / localSize.x));

        texture = new Texture2D(texW, texH, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        pixels = new Color32[texW * texH];
        ClearCanvas();
        ApplyTexture();

        // ppu elegido para que el sprite mida lo mismo que tu caja original
        float ppu = texW / localSize.x;
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, texW, texH), new Vector2(0.5f, 0.5f), ppu);
    }

    private void Update()
    {
        HandleInput();

        if (dirty)
        {
            ApplyTexture();
            dirty = false;
        }
    }

    // ---------- Input ----------
    private void HandleInput()
    {
        Pointer pointer = Pointer.current;   // mouse o pantalla táctil
        if (pointer == null) return;

        if (pointer.press.wasPressedThisFrame)
        {
            // Si el click fue sobre un botón de la UI, no dibujar
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                isDrawing = false;
                return;
            }
            isDrawing = true;
            hasLastPoint = false;
        }

        if (pointer.press.wasReleasedThisFrame)
        {
            isDrawing = false;
            hasLastPoint = false;
            return;
        }

        if (!isDrawing || !pointer.press.isPressed) return;

        Vector2 screenPos = pointer.position.ReadValue();
        if (!TryGetPixel(screenPos, out Vector2 pixel))
        {
            hasLastPoint = false;   // salió del canvas: no unir con el siguiente punto
            return;
        }

        int radius = GetCurrentRadius();
        Color32 color = currentTool == Tool.Eraser ? (Color32)backgroundColor : (Color32)brushColor;

        if (hasLastPoint) DrawLine(lastPixel, pixel, radius, color);
        else DrawCircle(Mathf.RoundToInt(pixel.x), Mathf.RoundToInt(pixel.y), radius, color);

        lastPixel = pixel;
        hasLastPoint = true;
        dirty = true;
    }

    private int GetCurrentRadius()
    {
        switch (currentTool)
        {
            case Tool.BigBrush: return bigBrushRadius;
            case Tool.Eraser: return eraserRadius;
            default: return smallBrushRadius;
        }
    }

    // Convierte posición de pantalla -> píxel de la textura (funciona con escala/rotación)
    private bool TryGetPixel(Vector3 screenPos, out Vector2 pixel)
    {
        Vector3 world = cam.ScreenToWorldPoint(screenPos);
        Vector3 local = transform.InverseTransformPoint(world);

        float u = local.x / localSize.x + 0.5f;
        float v = local.y / localSize.y + 0.5f;

        pixel = new Vector2(u * texW, v * texH);
        return u >= 0f && u <= 1f && v >= 0f && v <= 1f;
    }

    // ---------- Dibujo ----------
    // Rellena el espacio entre dos puntos para que el trazo no quede punteado
    private void DrawLine(Vector2 from, Vector2 to, int radius, Color32 color)
    {
        float distance = Vector2.Distance(from, to);
        float step = Mathf.Max(1f, radius * 0.5f);
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / step));

        for (int i = 0; i <= steps; i++)
        {
            Vector2 p = Vector2.Lerp(from, to, i / (float)steps);
            DrawCircle(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), radius, color);
        }
    }

    private void DrawCircle(int cx, int cy, int radius, Color32 color)
    {
        int rSqr = radius * radius;
        for (int y = -radius; y <= radius; y++)
        {
            int py = cy + y;
            if (py < 0 || py >= texH) continue;

            for (int x = -radius; x <= radius; x++)
            {
                int px = cx + x;
                if (px < 0 || px >= texW) continue;
                if (x * x + y * y > rSqr) continue;

                pixels[py * texW + px] = color;
            }
        }
    }

    private void ApplyTexture()
    {
        texture.SetPixels32(pixels);
        texture.Apply(false);
    }
}