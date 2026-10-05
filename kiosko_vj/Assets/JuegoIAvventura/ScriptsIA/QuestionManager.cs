using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

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

    [SerializeField]
    private GameManager gameManager;
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


    void Start()
    {
        Debug.Log("QuestionManager START");
        //questions.Add(new QuestionData("Funcionalidad de la IA", "Copiarse", "Aprender", "Dejar de pensar", "No resumir", 1));
        questions.Add(new QuestionData(
        "¿Cuál es una funcionalidad de la IA?",
        "Copiarse",
        "Aprender",
        "Dejar de pensar",
        "No resumir",
        1
    ));

    questions.Add(new QuestionData(
        "¿Cuál es un beneficio de la Inteligencia Artificial?",
        "Automatizar tareas repetitivas",
        "Eliminar todos los empleos",
        "Evitar el uso de datos",
        "Reemplazar siempre a las personas",
        0
    ));

    questions.Add(new QuestionData(
        "¿Cuál es una práctica responsable al usar IA?",
        "Compartir datos personales",
        "Confiar siempre en sus respuestas",
        "Verificar la información que genera",
        "Usarla sin supervisión",
        2
    ));

        ShowQuestion();

    }

    [SerializeField] // Allows to keep a variable as private but to see it in the Inspector
    private TMP_Text questionText, answerText1, answerText2, answerText3, answerText4;  // TMP_Text references to objects in Unity

    public void Play()
    {
        menu.SetActive(false);
        modeSelectPanel.SetActive(true);
    }

    public void SelectMode()
    {
        modeSelectPanel.SetActive(false);
        questionPanel.SetActive(true);
    }

    private void ShowQuestion()
    {
        Debug.Log("Mostrando pregunta: " + currentQuestion);
        questionText.text = questions[currentQuestion].GetQuestion();

        answerText1.text = questions[currentQuestion].GetAnswer1();
        answerText2.text = questions[currentQuestion].GetAnswer2();
        answerText3.text = questions[currentQuestion].GetAnswer3();
        answerText4.text = questions[currentQuestion].GetAnswer4();
    }

    public void AnswerQuestion(int selectedAnswer)
    {
        if (currentQuestion >= questions.Count)
        {
            return;
        }

        if(selectedAnswer == questions[currentQuestion].GetCorrectAnswer())
        {
            gameManager.RegisterCorrectAnswer();

        }
        else
        {
            gameManager.RegisterIncorrectAnswer();
        }

        currentQuestion++;

        if(currentQuestion < questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            questionPanel.SetActive(false);
            finalResultPanel.SetActive(true);
            scoreText.text = gameManager.GetScore().ToString();
            correctAnswersText.text = gameManager.GetCorrectAnswers().ToString();
            incorrectAnswersText.text = gameManager.GetIncorrectAnswers().ToString();
        }
    }

    public void RestartGame()
    {
        currentQuestion = 0;
        finalResultPanel.SetActive(false);
        questionPanel.SetActive(true);
        gameManager.ResetGame();
        ShowQuestion();
    }

    public void GoToMainMenu()
    {
        finalResultPanel.SetActive(false);
        menu.SetActive(true);
        gameManager.ResetGame();
        currentQuestion = 0;
    }
    

    

}
