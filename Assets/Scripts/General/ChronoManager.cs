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
    public event TickDelegate NewCentisecondTick, NewSecondTick, NewMinuteTick, ChronoEnded;

    // Time variables
    private float _elapsedTime = 0f;
    private int _lastCentisecond = 0;
    private int _lastSecond = 0;
    private int _lastMinute = 0;

    /// <summary>
    /// Value to control the main chrono.
    /// </summary>
    private bool _mainChronoIsRunning = false;

    /// <summary>
    /// Value to control the tutorial chrono.
    /// </summary>
    private bool _tutorialChronoIsRunning = false;

    /// <summary>
    /// Variable to check if the chrono is paused.
    /// </summary>
    private bool _isPaused = false;

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
        if (_mainChronoIsRunning && !_isPaused)
        {
            // Decrement the elapsed time
            _elapsedTime -= Time.deltaTime;

            // Check if the timer has reached 0
            if (_elapsedTime <= 0f)
            {
                _elapsedTime = 0f;
                _mainChronoIsRunning = false;

                ChronoEnded?.Invoke();

                NewSecondTick?.Invoke();
                NewSecond?.Invoke(0);
                NewMinuteTick?.Invoke();
                NewMinute?.Invoke(0);
                return;
            }

            // Check centiseconds
            int currentCentisecond = (int)(_elapsedTime * 100) % 100;
            if (currentCentisecond != _lastCentisecond)
            {
                _lastCentisecond = currentCentisecond;
                NewCentisecondTick?.Invoke();
                NewCentisecond?.Invoke(currentCentisecond);
            }

            // Check seconds
            int currentSecond = (int)_elapsedTime % 60;
            if (currentSecond != _lastSecond)
            {
                _lastSecond = currentSecond;
                NewSecondTick?.Invoke();
                NewSecond?.Invoke(currentSecond);
            }

            // Check minutes
            int currentMinute = (int)(_elapsedTime / 60);
            if (currentMinute != _lastMinute)
            {
                _lastMinute = currentMinute;
                NewMinuteTick?.Invoke();
                NewMinute?.Invoke(currentMinute);
            }
        }
        else if (_tutorialChronoIsRunning && !_isPaused)
        {
            // Increment the elapsed time
            _elapsedTime += Time.deltaTime;

            // Check seconds
            int currentSecond = (int)_elapsedTime;
            if (currentSecond != _lastSecond)
            {
                _lastSecond = currentSecond;
                NewSecondTick?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Called to start the tutorial chrono.
    /// </summary>
    public void StartTutorialChrono()
    {
        ResetChronos();

        _elapsedTime = 0f;

        _tutorialChronoIsRunning = true;
    }

    /// <summary>
    /// Called to stop the tutorial chrono.
    /// </summary>
    public void StopTutorialChrono()
    {
        _mainChronoIsRunning = false;
    }

    /// <summary>
    /// Called to start the main chrono.
    /// </summary>
    /// <param name="startTime"> Started time of the chrono (in minutes). </param>
    public void StartMainChrono(int startTime)
    {
        ResetChronos();

        _elapsedTime = startTime * 60f;

        _mainChronoIsRunning = true;
    }

    /// <summary>
    /// Called to stop the main chrono.
    /// </summary>
    public void StopMainChrono()
    {
        _mainChronoIsRunning = false;
    }

    /// <summary>
    /// Called to reset chronos.
    /// </summary>
    public void ResetChronos()
    {
        _mainChronoIsRunning = false;
        _tutorialChronoIsRunning = false;
        _isPaused = false;
        _elapsedTime = 0f;
        _lastCentisecond = 0;
        _lastSecond = 0;
        _lastMinute = 0;
    }

    /// <summary>
    /// Called to pause the chrono.
    /// </summary>
    public void PauseChrono()
    {
        _isPaused = true;
    }

    /// <summary>
    /// Called to resume the chrono.
    /// </summary>
    public void ResumeChrono()
    {
        _isPaused = false;
    }
}