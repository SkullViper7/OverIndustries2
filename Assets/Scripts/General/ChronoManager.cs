using System;
using UnityEngine;

public class ChronoManager : MonoBehaviour
{
    // Singleton
    private static ChronoManager _instance = null;
    public static ChronoManager Instance => _instance;

    /// <summary>
    /// Started time of the game (in minutes).
    /// </summary>
    private int _startedTime;

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
    public event TickDelegate NewCentisecondTick, NewSecondTick, NewMinuteTick, ChronoEnded;

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

    private void Update()
    {
        if (!isRunning || isPaused) return;

        // Decrement the elapsed time
        elapsedTime -= Time.deltaTime;

        // Check if the timer has reached 0
        if (elapsedTime <= 0f)
        {
            elapsedTime = 0f;
            isRunning = false;

            ChronoEnded?.Invoke();

            NewSecondTick?.Invoke();
            NewSecond?.Invoke(0);
            NewMinuteTick?.Invoke();
            NewMinute?.Invoke(0);
            return;
        }

        // Check centiseconds
        int currentCentisecond = (int)(elapsedTime * 100) % 100;
        if (currentCentisecond != lastCentisecond)
        {
            lastCentisecond = currentCentisecond;
            NewCentisecondTick?.Invoke();
            NewCentisecond?.Invoke(currentCentisecond);
        }

        // Check seconds
        int currentSecond = (int)elapsedTime % 60;
        if (currentSecond != lastSecond)
        {
            lastSecond = currentSecond;
            NewSecondTick?.Invoke();
            NewSecond?.Invoke(currentSecond);
        }

        // Check minutes
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
    /// <param name="startTime"> Started time of the chrono (in minutes). </param>
    public void StartChronometer(int startTime)
    {
        _startedTime = startTime;
        lastCentisecond = 0;
        lastSecond = 0;
        lastMinute = 0;
        elapsedTime = _startedTime * 60f;

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
        _startedTime = 0;
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