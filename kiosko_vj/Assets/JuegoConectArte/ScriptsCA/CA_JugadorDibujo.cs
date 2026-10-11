using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDrawing
{
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
public static class DrawingStore
{
    public static readonly List<PlayerDrawing> Entries = new List<PlayerDrawing>();
}