using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public float timeRemaining = 20f;
    public TMP_Text timerText;
    public bool isRunning = false;

    // sonido de "tic" en los últimos segundos
    public SoundManager soundManager;

    // el número se pone rojo en los últimos 5 segundos (animación)
    public Color warningColor = Color.red;
    private Color normalColor = Color.white;
    private int lastSecondShown = -1;

    // Avisa cuando se acaba el tiempo
    public event Action OnTimeUp;

    void Start()
    {
        if (timerText != null)
        {
            normalColor = timerText.color; // Guarda el color original del texto
        }
        UpdateTimerText();
    }

    void Update()
    {
        if (timeRemaining > 0 && isRunning)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0, timeRemaining);

            UpdateTimerText();

            if (timeRemaining <= 0)
            {
                isRunning = false;
                OnTimeUp?.Invoke();
            }
        }
    }

    // Inicia o reinicia el reloj
    public void StartTimer()
    {
        timeRemaining = 20f;
        isRunning = true;
        lastSecondShown = -1;
        UpdateTimerText();
    }

    // Detiene el reloj
    public void StopTimer()
    {
        isRunning = false;
    }

    // pausa sin reiniciar el tiempo (para Settings)
    public void PauseTimer()
    {
        isRunning = false;
    }

    // continúa desde donde se quedó
    public void ResumeTimer()
    {
        if (timeRemaining > 0)
        {
            isRunning = true;
        }
    }

    // Actualiza el numero en pantalla
    void UpdateTimerText()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = seconds.ToString();

            // Rojo en los últimos 5 segundos
            if (seconds <= 5)
            {
                timerText.color = warningColor;
            }
            else
            {
                timerText.color = normalColor;
            }

            // Un "tic" cada vez que cambia el número en los últimos 5 segundos
            if (isRunning && seconds <= 5 && seconds > 0 && seconds != lastSecondShown)
            {
                if (soundManager != null)
                {
                    soundManager.PlayTick();
                }
            }

            lastSecondShown = seconds;
        }
    }
}