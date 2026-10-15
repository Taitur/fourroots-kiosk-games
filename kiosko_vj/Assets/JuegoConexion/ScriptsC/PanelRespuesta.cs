using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelRespuesta : MonoBehaviour
{
    public Button botonCancelar;      
    public Button botonEnviar; 

    public TMP_Text textoEnunciado;
    public TMP_InputField response;

    GameManagerConexion.Question pregunta; 
    
    void Awake()
    {
        botonCancelar.onClick.AddListener(Cerrar);
        botonEnviar.onClick.AddListener(Enviar);
    }



    void Enviar()
    {
        pregunta.response = response.text;
        PreguntaManager.Instancia.EnviarRespuesta(pregunta); 
        Cerrar();
      
    }

   
    public void Inicializar(GameManagerConexion.Question p)
    {
        textoEnunciado.text = p.question;
        pregunta = p;
        
    }
    public void Cerrar()
    {
        PreguntaManager.Instancia.PanelCerrado();
        Destroy(gameObject);
    }

}
