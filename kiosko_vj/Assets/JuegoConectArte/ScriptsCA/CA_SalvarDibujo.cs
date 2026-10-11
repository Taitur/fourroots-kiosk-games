using UnityEngine;

/// <summary>
/// Escena de DIBUJAR. Sube el dibujo a la base de datos (vía DrawingApi).
/// Conecta SaveDrawing() al evento "On Time Up" del SceneTimer.
/// </summary>
public class CA_SalvarDibujo : MonoBehaviour
{
    [SerializeField] private CA_Pintar drawingCanvas;
    [SerializeField] private CA_Prompts wordPicker;

    public void SaveDrawing()
    {
        CA_ApiDibujos api = CA_ApiDibujos.Instance;

        if (api == null || drawingCanvas == null || wordPicker == null)
        {
            Debug.LogWarning("DrawingSaver: falta el DrawingApi en la escena o asignar el DrawingCanvas / WordPicker.");
            return;
        }

        if (wordPicker.CurrentPromptId <= 0)
        {
            Debug.LogWarning("DrawingSaver: el prompt todavía no se había cargado, no se subió el dibujo.");
            return;
        }

        // El PNG se genera aquí mismo, antes de que cambie la escena.
        // La subida termina sola porque DrawingApi no se destruye al cambiar de escena.
        byte[] png = drawingCanvas.GetPNG();

        api.UploadDrawing(api.PlayerId, wordPicker.CurrentPromptId, png, (ok, msg) =>
        {
            Debug.Log(ok ? "Dibujo subido: " + msg : "Error al subir el dibujo: " + msg);
        });
    }
}