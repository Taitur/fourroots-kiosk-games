using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// Puntos del jugador. Total es la copia local; si hay sesion (SC_Sesion.Activa) se sincroniza con
// el servidor: Cargar() lee los puntos y Guardar() suma y los envia a la tabla juego_SP (columna Puntos).
public static class SC_Puntos
{
    public static int Total { get; private set; }

    [Serializable] class PuntosBody { public int id_jugador; public int puntos; }
    [Serializable] class PuntosResp { public int puntos; }

    const float TiempoMaximo = 5f;

    public static void Sumar(int cantidad)
    {
        Total += cantidad;
    }

    public static void Reiniciar()
    {
        Total = 0;
    }

    // Lee los puntos del jugador desde el servidor (GET /juego-sp/puntos/{id}).
    public static IEnumerator Cargar()
    {
        if (!SC_Sesion.Activa) yield break;

        using (UnityWebRequest req = UnityWebRequest.Get(SC_Sesion.UrlApi + "/juego-sp/puntos/" + SC_Sesion.JugadorId))
        {
            req.timeout = (int)TiempoMaximo;
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("SC_Puntos.Cargar: " + req.error);
                yield break;
            }
            Total = JsonUtility.FromJson<PuntosResp>(req.downloadHandler.text).puntos;
        }
    }

    // Suma los puntos localmente y los guarda en el servidor (POST /juego-sp/puntos).
    // Si no hay sesion o el servidor falla, los puntos locales se conservan.
    public static IEnumerator Guardar(int cantidad)
    {
        Sumar(cantidad);
        if (!SC_Sesion.Activa) yield break;

        string json = JsonUtility.ToJson(new PuntosBody { id_jugador = SC_Sesion.JugadorId, puntos = cantidad });
        using (UnityWebRequest req = new UnityWebRequest(SC_Sesion.UrlApi + "/juego-sp/puntos", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = (int)TiempoMaximo;
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("SC_Puntos.Guardar: " + req.error);
                yield break;
            }
            Total = JsonUtility.FromJson<PuntosResp>(req.downloadHandler.text).puntos;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void AlIniciar()
    {
        Total = 0;
    }
}
