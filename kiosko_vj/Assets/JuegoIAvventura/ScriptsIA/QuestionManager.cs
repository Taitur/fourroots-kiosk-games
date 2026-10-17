using UnityEngine;
using UnityEngine.UI; // NEW: needed to change the color of the answer buttons
using System.Collections.Generic;
using TMPro;

/*
Indexes:
Answer1 → 0
Answer2 → 1
Answer3 → 2
Answer4 → 3
*/

public class QuestionManager : MonoBehaviour
{
    private int currentQuestion = 0; // Initializes in the first question

    private List<QuestionData> questions = new List<QuestionData>();

    //  Questions for each category
    private List<QuestionData> benefitsQuestions = new List<QuestionData>();
    private List<QuestionData> ethicsQuestions = new List<QuestionData>();
    private List<QuestionData> practicalQuestions = new List<QuestionData>();
    private List<QuestionData> thinkingQuestions = new List<QuestionData>();

    // NUEVO: categoría que se está jugando (1 a 4), para reiniciar y para la música
    private int currentCategory = 0;

    // segundos que se muestra el color verde/rojo antes de pasar a la siguiente pregunta
    private float feedbackTime = 1f;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private TimeManager timeManager;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private GameObject questionPanel;
    [SerializeField]
    private GameObject finalResultPanel;
    [SerializeField]
    private TMP_Text scoreText, correctAnswersText, incorrectAnswersText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject modeSelectPanel;

    // panel de instrucciones (aparece después de presionar Jugar)
    [SerializeField]
    private GameObject instructionsPanel;

    // título de la pantalla de resultados (el que ahora dice "New Text")
    [SerializeField]
    private TMP_Text resultTitleText;

    [SerializeField] // Allows to keep a variable as private but to see it in the Inspector
    private TMP_Text questionText, answerText1, answerText2, answerText3, answerText4;  // TMP_Text references to objects in Unity

    // el componente Image de cada botón de respuesta (Answer1..Answer4)
    [SerializeField]
    private Image answerImage1, answerImage2, answerImage3, answerImage4;

    // colores de retroalimentación
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color correctColor = new Color(0.4f, 0.9f, 0.4f);
    [SerializeField]
    private Color incorrectColor = new Color(1f, 0.45f, 0.45f);

    // NEW: Subscribe to the timer event
    void Awake()
    {
        if (timeManager != null)
        {
            timeManager.OnTimeUp += HandleTimeUp;
        }
    }

    // Unsubscribe when this object is destroyed
    void OnDestroy()
    {
        if (timeManager != null)
        {
            timeManager.OnTimeUp -= HandleTimeUp;
        }
    }

    void Start()
    {
        Debug.Log("QuestionManager START");

        //questions.Add(new QuestionData("Funcionalidad de la IA", "Copiarse", "Aprender", "Dejar de pensar", "No resumir", 1));

        // Category 1 - Importancia y beneficios
        benefitsQuestions.Add(new QuestionData(
            "¿Cuál es un beneficio de la Inteligencia Artificial?",
            "Automatizar tareas repetitivas",
            "Eliminar todos los empleos",
            "Evitar el uso de datos",
            "Reemplazar siempre a las personas",
            0
        ));

        benefitsQuestions.Add(new QuestionData(
            "¿Cómo puede ayudar la IA en la educación?",
            "Eliminando a los maestros",
            "Ofreciendo aprendizaje personalizado",
            "Evitando que los alumnos estudien",
            "Garantizando todas las respuestas",
            1
        ));

        benefitsQuestions.Add(new QuestionData(
            "¿Qué beneficio puede aportar la IA en medicina?",
            "Sustituir siempre al médico",
            "Eliminar todos los riesgos",
            "Apoyar el análisis de imágenes médicas",
            "Garantizar una curación",
            2
        ));

        benefitsQuestions.Add(new QuestionData(
            "¿Cómo ayuda la IA a las empresas?",
            "Eliminando toda supervisión",
            "Impidiendo la innovación",
            "Evitando tomar decisiones",
            "Analizando grandes cantidades de datos",
            3
        ));

        benefitsQuestions.Add(new QuestionData(
            "¿Qué tarea puede facilitar la IA?",
            "Clasificar información",
            "Hacer infalibles las computadoras",
            "Eliminar la privacidad",
            "Evitar el pensamiento humano",
            0
        ));

        // Category 2 - Ética y uso responsable
        ethicsQuestions.Add(new QuestionData(
            "¿Cuál es una práctica responsable al usar IA?",
            "Compartir datos personales",
            "Confiar siempre en sus respuestas",
            "Verificar la información que genera",
            "Usarla sin supervisión",
            2
        ));

        ethicsQuestions.Add(new QuestionData(
            "¿Qué dato no debes compartir con una IA pública?",
            "Una pregunta general",
            "Tu contraseña personal",
            "El título de un libro",
            "Una operación matemática",
            1
        ));

        ethicsQuestions.Add(new QuestionData(
            "¿Qué es un sesgo en Inteligencia Artificial?",
            "Una respuesta siempre correcta",
            "Un programa sin datos",
            "Una computadora lenta",
            "Una tendencia a resultados injustos",
            3
        ));

        ethicsQuestions.Add(new QuestionData(
            "¿Qué debes hacer si una IA inventa información?",
            "Verificarla y corregirla",
            "Compartirla inmediatamente",
            "Usarla como si fuera verdadera",
            "Ignorar el problema",
            0
        ));

        ethicsQuestions.Add(new QuestionData(
            "¿Quién debe asumir responsabilidad al utilizar IA?",
            "Solamente la computadora",
            "Nadie",
            "Las personas y organizaciones que la usan",
            "Únicamente el internet",
            2
        ));

        // Category 3 - Uso práctico
        practicalQuestions.Add(new QuestionData(
            "¿Cuál es una funcionalidad de la IA?",
            "Copiarse",
            "Aprender patrones",
            "Dejar de pensar",
            "No resumir",
            1
        ));

        practicalQuestions.Add(new QuestionData(
            "¿Cuál es un ejemplo de IA en la vida diaria?",
            "Un cable de corriente",
            "Un cuaderno",
            "Un asistente de voz",
            "Una silla",
            2
        ));

        practicalQuestions.Add(new QuestionData(
            "¿Para qué puede utilizarse un modelo de IA?",
            "Resumir textos",
            "Fabricar electricidad",
            "Evitar todas las fallas",
            "Reemplazar cualquier profesión",
            0
        ));

        practicalQuestions.Add(new QuestionData(
            "¿Qué aplicación puede usar reconocimiento de imágenes?",
            "Un lápiz",
            "Un foco tradicional",
            "Una mesa",
            "Una herramienta que identifica objetos",
            3
        ));

        practicalQuestions.Add(new QuestionData(
            "¿Qué puedes pedirle a una IA generativa?",
            "Que garantice resultados perfectos",
            "Que proponga ideas para un proyecto",
            "Que nunca cometa errores",
            "Que decida todo por ti",
            1
        ));

        // Category 4 - Pensar primero, potenciar después
        thinkingQuestions.Add(new QuestionData(
            "¿Qué conviene hacer antes de pedir ayuda a una IA?",
            "Intentar comprender el problema",
            "Copiar sin leer",
            "Dejar de investigar",
            "Aceptar cualquier respuesta",
            0
        ));

        thinkingQuestions.Add(new QuestionData(
            "¿Cómo aprovechar mejor una respuesta de IA?",
            "Memorizarla sin entender",
            "Evitar hacer preguntas",
            "Revisarla y aplicar criterio propio",
            "Usarla sin comprobar nada",
            2
        ));

        thinkingQuestions.Add(new QuestionData(
            "¿Qué habilidad sigue siendo importante con la IA?",
            "No tomar decisiones",
            "Pensamiento crítico",
            "Depender de toda respuesta",
            "Dejar de aprender",
            1
        ));

        thinkingQuestions.Add(new QuestionData(
            "Si una IA te da código, ¿qué deberías hacer?",
            "Entregarlo sin revisarlo",
            "Evitar ejecutarlo",
            "Suponer que funciona",
            "Comprenderlo y probarlo",
            3
        ));

        thinkingQuestions.Add(new QuestionData(
            "¿Cuál es el mejor papel de la IA al aprender?",
            "Apoyar el aprendizaje, no sustituirlo",
            "Pensar siempre por nosotros",
            "Evitar cualquier esfuerzo",
            "Eliminar la creatividad",
            0
        ));

        // Do not start the timer while the player is in the menu
        timeManager.StopTimer();

        // ShowQuestion() was moved to SelectMode()
        // so the timer starts when the game begins.
    }

    public void Play()
    {
        menu.SetActive(false);

        // NUEVO: primero se muestran las instrucciones
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
        else
        {
            modeSelectPanel.SetActive(true);
        }

        if (soundManager != null)
        {
            soundManager.PlayClick();
        }
    }

    // botón "¡Entendido!" del panel de instrucciones
    public void ContinueToCategories()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
        modeSelectPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayClick();
        }
    }

    // Receives the category number from the button
    public void SelectMode(int category)
    {
        questions.Clear();

        if (category == 1)
        {
            questions.AddRange(benefitsQuestions);
        }
        else if (category == 2)
        {
            questions.AddRange(ethicsQuestions);
        }
        else if (category == 3)
        {
            questions.AddRange(practicalQuestions);
        }
        else if (category == 4)
        {
            questions.AddRange(thinkingQuestions);
        }
        else
        {
            Debug.LogError("Categoría no válida");
            return;
        }

        currentCategory = category;
        currentQuestion = 0;
        gameManager.ResetGame();

        modeSelectPanel.SetActive(false);
        questionPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayCategory();
            soundManager.PlayCategoryMusic(category);
        }

        // Display the first question and start the timer
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (currentQuestion >= questions.Count)
        {
            ShowFinalResults();
            return;
        }

        Debug.Log("Mostrando pregunta: " + currentQuestion);
        questionText.text = questions[currentQuestion].GetQuestion();

        answerText1.text = questions[currentQuestion].GetAnswer1();
        answerText2.text = questions[currentQuestion].GetAnswer2();
        answerText3.text = questions[currentQuestion].GetAnswer3();
        answerText4.text = questions[currentQuestion].GetAnswer4();

        // NUEVO: los botones regresan a su color normal
        ResetAnswerColors();

        if (soundManager != null)
        {
            soundManager.PlayNewQuestion();
        }

        // Reset the timer for each question
        timeManager.StartTimer();
    }

    public void AnswerQuestion(int selectedAnswer)
    {
        // Prevent answering when time has expired
        // or the question has already been answered
        if (currentQuestion >= questions.Count || !timeManager.isRunning)
        {
            return;
        }

        // Stop the timer when the player answers
        timeManager.StopTimer();

        int correctAnswer = questions[currentQuestion].GetCorrectAnswer();

        if(selectedAnswer == correctAnswer)
        {
            gameManager.RegisterCorrectAnswer();

            // NUEVO: el botón elegido se pone verde
            SetAnswerColor(selectedAnswer, correctColor);

            if (soundManager != null)
            {
                soundManager.PlayCorrect();
            }
        }
        else
        {
            gameManager.RegisterIncorrectAnswer();

            // el botón elegido se pone rojo y se muestra la correcta en verde
            SetAnswerColor(selectedAnswer, incorrectColor);
            SetAnswerColor(correctAnswer, correctColor);

            if (soundManager != null)
            {
                soundManager.PlayIncorrect();
            }
        }

        // Move to the next question
        // espera 1 segundo para que se vea el color
        Invoke(nameof(NextQuestion), feedbackTime);
    }

    // Called when the timer reaches zero
    private void HandleTimeUp()
    {
        if (currentQuestion >= questions.Count || !questionPanel.activeInHierarchy)
        {
            return;
        }

        Debug.Log("Tiempo agotado");

        // No answer was selected, so it counts as incorrect
        gameManager.RegisterIncorrectAnswer();

        //  muestra cuál era la correcta
        SetAnswerColor(questions[currentQuestion].GetCorrectAnswer(), correctColor);

        if (soundManager != null)
        {
            soundManager.PlayTimeUp();
        }

        Invoke(nameof(NextQuestion), feedbackTime);
    }

    // Shared logic for moving to the next question
    private void NextQuestion()
    {
        currentQuestion++;

        if(currentQuestion < questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            ShowFinalResults();
        }
    }

    // Display final results
    private void ShowFinalResults()
    {
        timeManager.StopTimer();

        questionPanel.SetActive(false);
        finalResultPanel.SetActive(true);

        scoreText.text = gameManager.GetScore().ToString();
        correctAnswersText.text = gameManager.GetCorrectAnswers().ToString();
        incorrectAnswersText.text = gameManager.GetIncorrectAnswers().ToString();

        int correct = gameManager.GetCorrectAnswers();

        // NUEVO: título según el resultado (reemplaza "New Text")
        if (resultTitleText != null)
        {
            if (correct == questions.Count)
            {
                resultTitleText.text = "¡Perfecto!";
            }
            else if (correct * 2 > questions.Count) // más de la mitad
            {
                resultTitleText.text = "¡Muy bien!";
            }
            else
            {
                resultTitleText.text = "¡Sigue practicando!";
            }
        }

        if (soundManager != null)
        {
            soundManager.PlayResultsMusic();

            if (correct == questions.Count)
            {
                soundManager.PlayPerfect();
            }
            else
            {
                soundManager.PlayFinish();
            }
        }
    }

    public void RestartGame()
    {
        // NUEVO: cancela cualquier "siguiente pregunta" pendiente
        CancelInvoke();

        // Stop the previous timer
        timeManager.StopTimer();

        currentQuestion = 0;
        finalResultPanel.SetActive(false);
        modeSelectPanel.SetActive(false);
        menu.SetActive(false);
        questionPanel.SetActive(true);
        gameManager.ResetGame();

        if (soundManager != null)
        {
            soundManager.PlayClick();
            soundManager.PlayCategoryMusic(currentCategory);
        }

        ShowQuestion();
    }

    public void GoToMainMenu()
    {
        if (soundManager != null)
        {
            soundManager.PlayClick();
        }

        ReturnToMenu();
    }

    // Called by the X button during a question
    public void CloseQuestion()
    {
        if (soundManager != null)
        {
            soundManager.PlayExit();
        }

        ReturnToMenu();
    }

    // lógica compartida para regresar al menú (antes estaba dentro de GoToMainMenu)
    private void ReturnToMenu()
    {
        // Cancela cualquier "siguiente pregunta" pendiente
        CancelInvoke();

        // Stop the timer when returning to the menu
        timeManager.StopTimer();

        finalResultPanel.SetActive(false);
        questionPanel.SetActive(false);
        modeSelectPanel.SetActive(false);
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
        menu.SetActive(true);
        gameManager.ResetGame();
        currentQuestion = 0;

        if (soundManager != null)
        {
            soundManager.PlayMenuMusic();
        }
    }

    // ---------- colores de los botones de respuesta ----------

    private Image GetAnswerImage(int index)
    {
        if (index == 0) return answerImage1;
        if (index == 1) return answerImage2;
        if (index == 2) return answerImage3;
        if (index == 3) return answerImage4;
        return null;
    }

    private void SetAnswerColor(int index, Color color)
    {
        Image image = GetAnswerImage(index);
        if (image != null)
        {
            image.color = color;
        }
    }

    private void ResetAnswerColors()
    {
        for (int i = 0; i < 4; i++)
        {
            SetAnswerColor(i, normalColor);
        }
    }
}