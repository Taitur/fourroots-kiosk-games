using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0; // Variables private for encapsulation
    private int correctAnswers = 0;
    private int incorrectAnswers = 0;

    public void RegisterCorrectAnswer() // Following C# uppercase standard for methods
    {
        ++correctAnswers;
        score += 100;
    }

    public void RegisterIncorrectAnswer()
    {
        ++incorrectAnswers;
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
