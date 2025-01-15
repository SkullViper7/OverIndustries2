using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    /// <summary>
    /// Time of the game (in minutes).
    /// </summary>
    [field : SerializeField, Tooltip("Time of the game (in minutes).")]
    public int GameTime { get; private set; }

    /// <summary>
    /// Time of the game (in minutes).
    /// </summary>
    [field : SerializeField, Tooltip("Time of the Tutoral (in minutes).")]
    public int TutorialGameTime { get; private set; }

    /// <summary>
    /// Gets if the game is in pause.
    /// </summary>
    public bool GameIsInPause { get; private set; }

    /// <summary>
    /// Gets if a UI is opened.
    /// </summary>
    public bool UIIsOpen { get; private set; }

    /// <summary>
    /// True if the player drag and drop an employee
    /// </summary>
    public bool InDragAndDrop { get; private set; }

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
        ChronoManager.Instance.StartChronometer(TutorialGameTime);
    }

    public void PauseGame()
    {
        ChronoManager.Instance.PauseChrono();
    }

    public void ResumeGame()
    {
        ChronoManager.Instance.ResumeChrono();
    }

    public void OpenUI()
    {
        UIIsOpen = true;
        PauseGame();
    }

    public void CloseUI()
    {
        UIIsOpen = false;
        ResumeGame();
    }

    public void StartDragAndDrop()
    {
        InDragAndDrop = true;
    }

    public void StopDragAndDrop()
    {
        InDragAndDrop = false;
    }
}
