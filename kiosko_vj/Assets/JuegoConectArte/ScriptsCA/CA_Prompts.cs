using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Elige UNA palabra al azar al iniciar la escena y la muestra en un Text del Canvas.
/// Funciona con TextMeshPro o con el Text clásico de UI (arrastra el que uses).
/// </summary>
public class WordPicker : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;

    [SerializeField] private string prefix = "Dibuja: ";

    [SerializeField]
    private List<string> words = new List<string>
    {
        "Gato", "Perro", "Casa", "Árbol", "Sol", "Luna", "Estrella", "Carro",
        "Avión", "Barco", "Bicicleta", "Pizza", "Helado", "Manzana", "Plátano",
        "Flor", "Mariposa", "Pez", "Pájaro", "Elefante", "León", "Tortuga",
        "Corazón", "Reloj", "Libro", "Guitarra", "Paraguas", "Montaña",
        "Playa", "Cohete"
    };

    // Otros scripts pueden leer la palabra elegida con: wordPicker.CurrentWord
    public string CurrentWord { get; private set; }

    private void Start()
    {
        if (words == null || words.Count == 0)
        {
            Debug.LogWarning("WordPicker: la lista de palabras está vacía.");
            return;
        }

        CurrentWord = words[Random.Range(0, words.Count)];

        string text = prefix + CurrentWord;
        if (tmpText != null) tmpText.text = text;
    }
}