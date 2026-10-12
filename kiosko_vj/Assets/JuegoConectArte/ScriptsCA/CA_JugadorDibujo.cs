using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Un dibujo de un jugador: su nombre, la palabra (prompt) y la imagen.
/// La imagen puede venir como Sprite (datos de prueba en el Inspector)
/// o como bytes PNG (cuando se guarda desde la escena de dibujar).
/// </summary>
[System.Serializable]
public class CA_JugadorDibujo
{
    public int idDibujo;                       // Id_dibujo en la base de datos
    public string playerName;
    public string word;
    public Sprite sprite;                      // para datos de prueba en el Inspector
    [HideInInspector] public byte[] png;       // para dibujos guardados en juego

    private Sprite cachedSprite;

    public Sprite GetSprite()
    {
        if (sprite != null) return sprite;
        if (cachedSprite != null) return cachedSprite;
        if (png == null || png.Length == 0) return null;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(png);   // ajusta el tamaño solo
        cachedSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        return cachedSprite;
    }
}

/// <summary>
/// Almacén temporal en memoria que sobrevive al cambio de escena.
/// Más adelante lo puedes reemplazar por lo que llegue de la red / servidor.
/// </summary>
public static class DrawingStore
{
    public static readonly List<CA_JugadorDibujo> Entries = new List<CA_JugadorDibujo>();
}