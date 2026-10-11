using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CA_Temporizador : MonoBehaviour
{
    [SerializeField] private float durationSeconds = 60f;
    [SerializeField] private bool startOnAwake = true;

    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private UnityEvent onTimeUp;

    private float timeLeft;
    private bool running;
    private bool finished;

    private void Start()
    {
        timeLeft = durationSeconds;
        running = startOnAwake;
        UpdateText();
    }

    private void Update()
    {
        if (!running || finished) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateText();
            finished = true;
            running = false;

            onTimeUp?.Invoke();
            SceneManager.LoadScene("CA_Calificar");
            return;
        }

        UpdateText();
    }

    public void StartTimer() { running = true; }
    public void PauseTimer() { running = false; }

    private void UpdateText()
    {
        int total = Mathf.CeilToInt(timeLeft);
        string text = (total / 60).ToString("00") + ":" + (total % 60).ToString("00");

        if (tmpText != null) tmpText.text = text;
    }
}