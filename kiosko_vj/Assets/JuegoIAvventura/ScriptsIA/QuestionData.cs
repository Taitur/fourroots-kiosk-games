using UnityEngine;

public class QuestionData
{
    private string question;
    private string answer1;
    private string answer2;
    private string answer3;
    private string answer4;
    private int correctAnswer;
    public QuestionData(string question, string answer1, string answer2, string answer3, string answer4, int correctAnswer)
    {
        this.question = question;
        this.answer1 = answer1;
        this.answer2 = answer2;
        this.answer3 = answer3;
        this.answer4 = answer4;
        this.correctAnswer = correctAnswer;
    }

    public string GetQuestion()
    {
        return question;
    }

    public string GetAnswer1()
    {
        return answer1;
    }

     public string GetAnswer2()
    {
        return answer2;
    }

     public string GetAnswer3()
    {
        return answer3;
    }
    
     public string GetAnswer4()
    {
        return answer4;
    }

    public int GetCorrectAnswer()
    {
        return correctAnswer;
    }
}
