using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Cliente HTTP: Unity habla con TU API y la API habla con la base de datos.
/// Es un singleton que NO se destruye al cambiar de escena, así una subida
/// que empezó en la escena de dibujar termina aunque ya se haya cargado la siguiente.
/// Ponlo en un objeto RAÍZ (sin padre) de la escena. Si pruebas cada escena por separado,
/// pon uno en cada una: el duplicado se destruye solo.
/// </summary>
public class CA_ApiDibujos : MonoBehaviour
{
    public static CA_ApiDibujos Instance { get; private set; }

    [SerializeField] private string baseUrl = "http://localhost:5000/api";

    [Tooltip("Id del jugador actual (tabla Jugador). Por ahora fijo para pruebas; luego lo asignará el login.")]
    [SerializeField] private int playerId = 1;
    public int PlayerId => playerId;

    // ---------- Datos que se envían / reciben (los nombres coinciden con tu API) ----------
    [Serializable]
    private class UploadRequest
    {
        public int id_jugador;
        public int id_prompt;
        public string img_dibujo;   // PNG en base64
    }

    [Serializable]
    private class PointsRequest
    {
        public int id_dibujo;
        public int puntos;
        public int id_jugador;   // quien manda los puntos (quien adivinó / quien calificó)
    }

    [Serializable]
    public class DrawingResponse
    {
        public int id_dibujo;
        public int id_jugador;
        public string nombre_jugador;
        public string nombre_prompt;
        public string img_dibujo;   // PNG en base64
    }

    [Serializable]
    public class PromptResponse
    {
        public int id_prompt;
        public string nombre_prompt;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // ya existe uno (viene de la escena anterior)
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------- Métodos públicos ----------

    // Trae un prompt al azar (id + nombre) para la escena de dibujar
    public void GetRandomPrompt(Action<PromptResponse> onDone)
    {
        StartCoroutine(GetJson("/prompts/aleatorio", onDone));
    }

    // Sube el dibujo (PNG) con el jugador y el prompt
    public void UploadDrawing(int idJugador, int idPrompt, byte[] png, Action<bool, string> onDone = null)
    {
        var body = new UploadRequest
        {
            id_jugador = idJugador,
            id_prompt = idPrompt,
            img_dibujo = Convert.ToBase64String(png)
        };
        StartCoroutine(PostJson("/dibujos", JsonUtility.ToJson(body), onDone));
    }

    // Trae un dibujo al azar de OTRO jugador (excluye el tuyo)
    public void GetRandomDrawing(int excludePlayerId, Action<DrawingResponse> onDone)
    {
        StartCoroutine(GetJson("/dibujos/aleatorio?excluir=" + excludePlayerId, onDone));
    }

    // Guarda los puntos de quien adivinó
    public void SendGuessPoints(int idDibujo, int puntos, Action<bool, string> onDone = null)
    {
        var body = new PointsRequest { id_dibujo = idDibujo, puntos = puntos, id_jugador = playerId };
        StartCoroutine(PostJson("/dibujos/puntos-adivinar", JsonUtility.ToJson(body), onDone));
    }

    // Guarda los puntos de la calificación (1 a 5)
    public void SendRatingPoints(int idDibujo, int puntos, Action<bool, string> onDone = null)
    {
        var body = new PointsRequest { id_dibujo = idDibujo, puntos = puntos, id_jugador = playerId };
        StartCoroutine(PostJson("/dibujos/puntos-calificacion", JsonUtility.ToJson(body), onDone));
    }

    // Convierte la respuesta de la API a tu PlayerDrawing
    public static CA_JugadorDibujo ToPlayerDrawing(DrawingResponse r)
    {
        return new CA_JugadorDibujo
        {
            idDibujo = r.id_dibujo,
            playerName = r.nombre_jugador,
            word = r.nombre_prompt,
            png = Convert.FromBase64String(r.img_dibujo)
        };
    }

    // ---------- Internos ----------
    private IEnumerator PostJson(string path, string json, Action<bool, string> onDone)
    {
        using (UnityWebRequest req = new UnityWebRequest(baseUrl + path, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 20;

            yield return req.SendWebRequest();

            bool ok = req.result == UnityWebRequest.Result.Success;
            if (!ok) Debug.LogWarning("DrawingApi POST " + path + " falló: " + req.error + " | " + req.downloadHandler.text);
            onDone?.Invoke(ok, ok ? req.downloadHandler.text : req.error);
        }
    }

    private IEnumerator GetJson<T>(string path, Action<T> onDone) where T : class
    {
        using (UnityWebRequest req = UnityWebRequest.Get(baseUrl + path))
        {
            req.timeout = 20;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("DrawingApi GET " + path + " falló: " + req.error + " | " + req.downloadHandler.text);
                onDone?.Invoke(null);
                yield break;
            }

            T response = JsonUtility.FromJson<T>(req.downloadHandler.text);
            onDone?.Invoke(response);
        }
    }
}