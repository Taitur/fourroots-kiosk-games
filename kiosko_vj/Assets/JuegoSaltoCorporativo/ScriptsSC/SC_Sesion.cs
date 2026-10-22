// Datos de la sesion del kiosko que necesita el juego para hablar con el servidor.
// JugadorId es el id del usuario del portal; lo debe llenar el menu del kiosko despues del login (0 = sin sesion).
// Mientras el menu no lo haga, SC_PuntosDisplay puede asignar un id de prueba.
public static class SC_Sesion
{
    // Direccion del backend (sin barra final). Cambiala cuando el kiosko no corra en localhost.
    public static string UrlApi = "http://localhost:3000/api";

    public static int JugadorId;
    public static string Nombre;

    public static bool Activa { get { return JugadorId > 0; } }
}
