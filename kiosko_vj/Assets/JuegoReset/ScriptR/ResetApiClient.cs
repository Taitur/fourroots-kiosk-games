using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ResetRegistroBody
{
    public int idactividad;
    public int idusuario;
    public int idusuarioregistro;
}

[Serializable]
public class ResetActividadDTO
{
    public int iidactividad;
    public int iidusuario;
    public int iidusuarioregistro;
    public string dtfecharegistro;
    public string dtfechamodificacion;
}

[Serializable]
public class ResetListaActividades
{
    public ResetActividadDTO[] actividades;
}

public static class ResetApiClient
{
    private const string BASE = "http://localhost:3000/api";

    private static IEnumerator Enviar(
        string metodo,
        string ruta,
        string json,
        Action<string, long> respuesta)
    {
        UnityWebRequest req = new UnityWebRequest(BASE + ruta, metodo);

        if (json != null)
        {
            byte[] datos = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(datos);
        }

        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        respuesta(req.downloadHandler.text, req.responseCode);
    }

    public static IEnumerator RegistrarActividad(
        int idactividad,
        int idusuario,
        int idusuarioregistro,
        Action ok,
        Action<string> error)
    {
        ResetRegistroBody datos = new ResetRegistroBody
        {
            idactividad = idactividad,
            idusuario = idusuario,
            idusuarioregistro = idusuarioregistro
        };

        string json = JsonUtility.ToJson(datos);

        yield return Enviar(
            "POST",
            "/reset/registrar",
            json,
            (texto, codigo) =>
            {
                if (codigo == 200 || codigo == 201)
                {
                    Debug.Log("RESET: actividad registrada correctamente.");
                    ok?.Invoke();
                }
                else
                {
                    Debug.LogError(
                        "RESET: error al registrar. Código: " +
                        codigo +
                        " Respuesta: " +
                        texto
                    );

                    error?.Invoke(texto);
                }
            }
        );
    }

    public static IEnumerator ConsultarActividades(
        int idusuario,
        Action<ResetActividadDTO[]> ok,
        Action<string> error)
    {
        yield return Enviar(
            "GET",
            "/reset/progreso/" + idusuario,
            null,
            (texto, codigo) =>
            {
                if (codigo == 200)
                {
                    ResetListaActividades resultado =
                        JsonUtility.FromJson<ResetListaActividades>(texto);

                    ok?.Invoke(resultado.actividades);
                }
                else
                {
                    Debug.LogError(
                        "RESET: error al consultar. Código: " +
                        codigo +
                        " Respuesta: " +
                        texto
                    );

                    error?.Invoke(texto);
                }
            }
        );
    }
}