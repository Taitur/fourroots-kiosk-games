using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0; // Variables private for encapsulation
    private int correctAnswers = 0;
    private int incorrectAnswers = 0;

    // puntos por respuesta correcta (cambia según la dificultad)
    private int pointsPerAnswer = 100;

    public void RegisterCorrectAnswer() // Following C# uppercase standard for methods
    {
        ++correctAnswers;
        score += pointsPerAnswer;
    }

    public void RegisterIncorrectAnswer()
    {
        ++incorrectAnswers;
    }

    // Cambia los puntos por respuesta (Fácil = 100, Difícil = 200)
    public void SetPointsPerAnswer(int points)
    {
        pointsPerAnswer = points;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    public int GetIncorrectAnswers()
    {
        return incorrectAnswers;
    }

    public void ResetGame()
    {
        score = 0;
        correctAnswers = 0;
        incorrectAnswers = 0;
    }

}