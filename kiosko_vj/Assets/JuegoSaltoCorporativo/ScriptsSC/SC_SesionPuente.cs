using System;
using UnityEngine;

// Puente entre la web del portal y el juego (build WebGL).
// La pagina juegos.html llama: unityInstance.SendMessage("SesionPuente", "AsignarJugador", "<id>").
// Este objeto se crea solo al iniciar el juego (no hace falta ponerlo en ninguna escena) y no se destruye
// al cambiar de escena, asi que el menu principal y todos los juegos ven el mismo SC_Sesion.JugadorId.
public class SC_SesionPuente : MonoBehaviour
{
    const string Nombre = "SesionPuente";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Crear()
    {
        if (GameObject.Find(Nombre) != null) return;
        var go = new GameObject(Nombre);
        go.AddComponent<SC_SesionPuente>();
        DontDestroyOnLoad(go);

#if UNITY_WEBGL && !UNITY_EDITOR
        // En la web el backend vive en el mismo dominio que la pagina: usa su direccion, no localhost.
        try
        {
            string origen = new Uri(Application.absoluteURL).GetLeftPart(UriPartial.Authority);
            SC_Sesion.UrlApi = origen + "/api";
        }
        catch (Exception e) { Debug.LogWarning("SC_SesionPuente: no se pudo leer la URL de la pagina: " + e.Message); }
#endif
    }

    // Lo llama la web con el id del usuario del portal (texto).
    public void AsignarJugador(string id)
    {
        int n;
        if (int.TryParse(id, out n) && n > 0)
        {
            SC_Sesion.JugadorId = n;
            Debug.Log("SC_SesionPuente: jugador " + n);
        }
        else
        {
            SC_Sesion.JugadorId = 0;
            Debug.LogWarning("SC_SesionPuente: id invalido '" + id + "'");
        }
    }
}
