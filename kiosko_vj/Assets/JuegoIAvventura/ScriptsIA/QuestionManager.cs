using UnityEngine;
using UnityEngine.UI; // needed to change the color of the answer buttons
using UnityEngine.Networking; // para conectarse a la API
using System;
using System.Collections;
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

    // categoria que se esta jugando (1 a 4), para reiniciar y para la musica
    private int currentCategory = 0;

    // segundos que se muestra el color verde/rojo antes de pasar a la siguiente pregunta
    private float feedbackTime = 1f;
    // mas tiempo cuando se equivocan, para que lean la respuesta correcta
    private float wrongFeedbackTime = 2.5f;

    // cuantas preguntas al azar salen en cada partida
    private int questionsPerGame = 6;

    // Direccion de la API del kiosco (en la version Web se toma sola de la URL)
    [SerializeField]
    private string apiUrl = "http://localhost:3000/api";

    // Una pregunta tal como la manda la API
    [Serializable]
    private class PreguntaApi
    {
        public int idPregunta;
        public string categoria;
        public string pregunta;
        public string respuesta1;
        public string respuesta2;
        public string respuesta3;
        public string respuesta4;
        public int respuestaCorrecta;
    }

    // La API responde { "preguntas": [ ... ] }
    [Serializable]
    private class RespuestaPreguntas
    {
        public PreguntaApi[] preguntas;
    }

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

    // panel de instrucciones (se abre con el boton "¿Como jugar?")
    [SerializeField]
    private GameObject instructionsPanel;

    // panel para elegir dificultad (aparece despues de elegir categoria)
    [SerializeField]
    private GameObject difficultyPanel;

    // Dificultad Facil: mas tiempo, menos puntos
    private float easyTime = 20f;
    private int easyPoints = 100;

    // Dificultad Dificil: menos tiempo, mas puntos
    private float hardTime = 10f;
    private int hardPoints = 200;

    // titulo de la pantalla de resultados (el que ahora dice "New Text")
    [SerializeField]
    private TMP_Text resultTitleText;

    [SerializeField] // Allows to keep a variable as private but to see it in the Inspector
    private TMP_Text questionText, answerText1, answerText2, answerText3, answerText4;  // TMP_Text references to objects in Unity

    // el componente Image de cada boton de respuesta (Answer1..Answer4)
    [SerializeField]
    private Image answerImage1, answerImage2, answerImage3, answerImage4;

    // colores de retroalimentacion
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color correctColor = new Color(0.4f, 0.9f, 0.4f);
    [SerializeField]
    private Color incorrectColor = new Color(1f, 0.45f, 0.45f);

    // Subscribe to the timer event
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

        // Do not start the timer while the player is in the menu
        timeManager.StopTimer();

        // Lee la direccion de la API desde la URL
        ReadUrl();

        // ShowQuestion() was moved to SelectMode()
        // so the timer starts when the game begins.
    }

    public void Play()
    {
        menu.SetActive(false);

        modeSelectPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayClick();
        }
    }

    // boton "¡Entendido!" del panel de instrucciones
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
    // boton "¿Como jugar?" de la pantalla de categorias
    public void ShowInstructions()
    {
        modeSelectPanel.SetActive(false);
        instructionsPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayClick();
        }
    }

    // Receives the category number from the button
    public void SelectMode(int category)
    {
        questions.Clear();
        currentCategory = category;

        // Trae las preguntas de la base de datos
        StartCoroutine(LoadQuestionsFromApi(category));

        // Despues de elegir categoria se elige la dificultad
        modeSelectPanel.SetActive(false);
        difficultyPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayCategory();
        }
    }

    // Botones de dificultad: 1 = Facil, 2 = Dificil
    public void SelectDifficulty(int level)
    {
        // Si todavia no llegan las preguntas de la base de datos, no empieza
        if (questions.Count == 0)
        {
            Debug.LogWarning("Todavia no hay preguntas de la base de datos");
            return;
        }

        if (level == 1)
        {
            timeManager.SetStartTime(easyTime);
            gameManager.SetPointsPerAnswer(easyPoints);
        }
        else
        {
            timeManager.SetStartTime(hardTime);
            gameManager.SetPointsPerAnswer(hardPoints);
        }

        currentQuestion = 0;
        gameManager.ResetGame();

        difficultyPanel.SetActive(false);
        questionPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayClick();
            soundManager.PlayCategoryMusic(currentCategory);
        }

        // Display the first question and start the timer
        ShowQuestion();
    }

    // Boton "Regresar" del panel de dificultad
    public void BackToCategories()
    {
        difficultyPanel.SetActive(false);
        modeSelectPanel.SetActive(true);

        if (soundManager != null)
        {
            soundManager.PlayClick();
        }
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

        // los botones regresan a su color normal
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
        // Prevent answering when time has expired or the question has already been answered
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

            // el boton elegido se pone verde
            SetAnswerColor(selectedAnswer, correctColor);

            if (soundManager != null)
            {
                soundManager.PlayCorrect();
            }

            // espera 1 segundo para que se vea el color
            Invoke(nameof(NextQuestion), feedbackTime);
        }
        else
        {
            gameManager.RegisterIncorrectAnswer();

            // espera mas tiempo para que lean la respuesta correcta
            Invoke(nameof(NextQuestion), wrongFeedbackTime);

            // el boton elegido se pone rojo y se muestra la correcta en verde
            SetAnswerColor(selectedAnswer, incorrectColor);
            SetAnswerColor(correctAnswer, correctColor);

            if (soundManager != null)
            {
                soundManager.PlayIncorrect();
            }
        }
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

        //  muestra cual era la correcta
        SetAnswerColor(questions[currentQuestion].GetCorrectAnswer(), correctColor);

        if (soundManager != null)
        {
            soundManager.PlayTimeUp();
        }

        Invoke(nameof(NextQuestion), wrongFeedbackTime);
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

        // titulo segun el resultado
        if (resultTitleText != null)
        {
            if (correct == questions.Count)
            {
                resultTitleText.text = "¡Perfecto!";
            }
            else if (correct * 2 > questions.Count) // mas de la mitad
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
        // cancela cualquier "siguiente pregunta" pendiente
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

    // logica compartida para regresar al menu (antes estaba dentro de GoToMainMenu)
    private void ReturnToMenu()
    {
        // Cancela cualquier "siguiente pregunta" pendiente
        CancelInvoke();

        // Stop the timer when returning to the menu
        timeManager.StopTimer();

        finalResultPanel.SetActive(false);
        questionPanel.SetActive(false);
        modeSelectPanel.SetActive(false);
        difficultyPanel.SetActive(false);
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

    // ---------- base de datos (API) ----------

    // Nombre de la categoria tal como esta en la columna categoria de kio_tbPregunta
    private string GetCategoryName(int category)
    {
        if (category == 1) return "Importancia y beneficios";
        if (category == 2) return "Ética y uso responsable";
        if (category == 3) return "Uso práctico";
        return "Pensar primero, potenciar después";
    }

    // En la version Web toma la direccion del servidor de la URL
    private void ReadUrl()
    {
        string url = Application.absoluteURL;
        if (string.IsNullOrEmpty(url))
        {
            return; // en el editor de Unity se usa apiUrl del Inspector
        }

        Uri uri = new Uri(url);
        apiUrl = uri.GetLeftPart(UriPartial.Authority) + "/api";
    }

    // GET /api/iavventura/preguntas?categoria=Uso practico
    private IEnumerator LoadQuestionsFromApi(int category)
    {
        string url = apiUrl + "/iavventura/preguntas?categoria=" + UnityWebRequest.EscapeURL(GetCategoryName(category));
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("No se pudo conectar con la API: " + request.error);
            yield break;
        }

        RespuestaPreguntas respuesta = JsonUtility.FromJson<RespuestaPreguntas>(request.downloadHandler.text);

        if (respuesta == null || respuesta.preguntas == null || respuesta.preguntas.Length == 0)
        {
            Debug.LogError("La base de datos no tiene preguntas de esta categoría");
            yield break;
        }

        foreach (PreguntaApi p in respuesta.preguntas)
        {
            questions.Add(new QuestionData(p.pregunta, p.respuesta1, p.respuesta2, p.respuesta3, p.respuesta4, p.respuestaCorrecta));
        }

        Debug.Log("Preguntas cargadas de la base de datos: " + questions.Count);

        // Mezcla las preguntas: cada una cambia de lugar con otra al azar
        for (int i = 0; i < questions.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, questions.Count);
            QuestionData temp = questions[i];
            questions[i] = questions[j];
            questions[j] = temp;
        }

        // Deja solo las primeras 5 para esta partida
        if (questions.Count > questionsPerGame)
        {
            questions.RemoveRange(questionsPerGame, questions.Count - questionsPerGame);
        }

        Debug.Log("Preguntas en esta partida: " + questions.Count);
    }
}