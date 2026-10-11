using UnityEngine;

public class DrawingSaver : MonoBehaviour
{
    [SerializeField] private CA_Pintar CAPintar;
    [SerializeField] private WordPicker wordPicker;
    [SerializeField] private string playerName = "Jugador";

    public void SaveDrawing()
    {
        if (CAPintar == null || wordPicker == null)
        {
            Debug.LogWarning("DrawingSaver: falta asignar el CA_Pintar o el WordPicker.");
            return;
        }

        DrawingStore.Entries.Add(new PlayerDrawing
        {
            playerName = playerName,
            word = wordPicker.CurrentWord,
            png = CAPintar.GetPNG()
        });
    }
}