using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable] public class PreguntaDTO {
    public int id; public string question; public string relatedDepartment; public string response;
    public string relatedPlayerName; public string responsePlayerName; public string oficinaId; public int slotIndex;
}
[Serializable] public class ListaPreguntas { public PreguntaDTO[] preguntas; }
[Serializable] public class JugadorDTO {
    public int id; public string nombre; public string puesto; public string expertise;
    public string usuarioDepartamento; public int puntos;
}
[Serializable] class LoginBody { public string nombre, puesto, expertise, departamento; }
[Serializable] class CrearBody { public int autorId; public string departamento, enunciado, oficinaId; }
[Serializable] class RespBody { public int respondedorId; public string respuesta; }
[Serializable] public class CrearResp { public int id; public string oficinaId; public int slotIndex; }



public static class ApiClient
{
    const string BASE = "http://localhost:3000/api";    

    static IEnumerator Enviar(string metodo, string ruta, string json, Action<string, long> cb)
    {
        var req = new UnityWebRequest(BASE + ruta, metodo);
        if (json != null) req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        cb(req.downloadHandler.text, req.responseCode);
    }

    public static IEnumerator Login(string nombre, string puesto, string expertise, string depto, Action<JugadorDTO> ok)
    {
        string body = JsonUtility.ToJson(new LoginBody { nombre = nombre, puesto = puesto, expertise = expertise, departamento = depto });
        yield return Enviar("POST", "/jugadores/login", body, (txt, code) => {
            if (code == 200) ok(JsonUtility.FromJson<JugadorDTO>(txt)); else Debug.LogError("login " + code);
        });
    }

    public static IEnumerator CrearPregunta(int autorId, string depto, string texto, string oficina, Action<CrearResp> ok, Action llenas)
    {
        string body = JsonUtility.ToJson(new CrearBody { autorId = autorId, departamento = depto, enunciado = texto, oficinaId = oficina });
        yield return Enviar("POST", "/preguntas", body, (txt, code) => {
            if (code == 200) ok(JsonUtility.FromJson<CrearResp>(txt));
            else if (code == 409) llenas();
            else Debug.LogError("crear " + code);
        });
    }

    public static IEnumerator ListarPreguntas(Action<PreguntaDTO[]> ok)
    {
        yield return Enviar("GET", "/preguntas", null, (txt, code) => {
            if (code == 200) ok(JsonUtility.FromJson<ListaPreguntas>(txt).preguntas);
        });
    }

    public static IEnumerator Responder(int preguntaId, int respondedorId, string respuesta, Action ok)
    {
        string body = JsonUtility.ToJson(new RespBody { respondedorId = respondedorId, respuesta = respuesta });
        yield return Enviar("POST", "/preguntas/" + preguntaId + "/responder", body, (txt, code) => {
            if (code == 200) ok(); else Debug.LogError("responder " + code);
        });
    }

    public static IEnumerator Matchmaking(int jugadorId, Action<PreguntaDTO> ok, Action ninguna)
    {
        yield return Enviar("GET", "/preguntas/matchmaking?jugadorId=" + jugadorId, null, (txt, code) => {
            if (code == 200) ok(JsonUtility.FromJson<PreguntaDTO>(txt));
            else if (code == 204) ninguna();
        });
    }

   
   
}