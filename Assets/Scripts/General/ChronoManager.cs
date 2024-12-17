using System;
using UnityEngine;

public class ChronoManager : MonoBehaviour
{
    // Singleton
    private static ChronoManager _instance = null;
    public static ChronoManager Instance => _instance;

    /// <summary>
    /// Events to indicate that there is a new time.
    /// </summary>
    /// <param name="newTime"> New time to declare. </param>
    public delegate void ChronoDelegate(int newTime);
    public event ChronoDelegate NewCentisecond, NewSecond, NewMinute;

    /// <summary>
    /// Events to indicate new ticks.
    /// </summary>
    public delegate void TickDelegate();
    public event TickDelegate NewCentisecondTick, NewSecondTick, NewMinuteTick;

    // Time variables
    private float elapsedTime = 0f;
    private int lastCentisecond = 0;
    private int lastSecond = 0;
    private int lastMinute = 0;

    /// <summary>
    /// Value to control the chrono.
    /// </summary>
    private bool isRunning = false;

    /// <summary>
    /// Variable to check if the chrono is paused.
    /// </summary>
    private bool isPaused = false;

    // Get if the chrono is running and the current time
    public bool IsRunning => isRunning && !isPaused;
    public TimeSpan CurrentTime => TimeSpan.FromSeconds(elapsedTime);

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        StartChronometer();
    }

    private void Update()
    {
        if (!isRunning || isPaused) return;

        elapsedTime += Time.deltaTime;

        // Check for centiseconds
        int currentCentisecond = (int)(elapsedTime * 100) % 100;
        if (currentCentisecond != lastCentisecond)
        {
            lastCentisecond = currentCentisecond;
            NewCentisecondTick?.Invoke();
            NewCentisecond?.Invoke(currentCentisecond);
        }

        // Check for seconds
        int currentSecond = (int)elapsedTime % 60;
        if (currentSecond != lastSecond)
        {
            lastSecond = currentSecond;
            NewSecondTick?.Invoke();
            NewSecond?.Invoke(currentSecond);
        }

        // Check for minutes
        int currentMinute = (int)(elapsedTime / 60);
        if (currentMinute != lastMinute)
        {
            lastMinute = currentMinute;
            NewMinuteTick?.Invoke();
            NewMinute?.Invoke(currentMinute);
        }
    }

    /// <summary>
    /// Called to start the chrono.
    /// </summary>
    public void StartChronometer()
    {
        isRunning = true;
        isPaused = false;
    }

    /// <summary>
    /// Called to stop the chrono.
    /// </summary>
    public void StopChronometer()
    {
        isRunning = false;
        isPaused = false;
    }

    /// <summary>
    /// Called to reset the chrono.
    /// </summary>
    public void ResetChronometer()
    {
        isRunning = false;
        isPaused = false;
        elapsedTime = 0f;
        lastCentisecond = 0;
        lastSecond = 0;
        lastMinute = 0;
    }

    /// <summary>
    /// Called to pause the chrono.
    /// </summary>
    public void PauseChrono()
    {
        if (!isRunning) return;
        isPaused = true;
    }

    /// <summary>
    /// Called to resume the chrono.
    /// </summary>
    public void ResumeChrono()
    {
        if (!isRunning) return;
        isPaused = false;
    }
}