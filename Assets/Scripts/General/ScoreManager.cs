using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // Singleton
    private static ScoreManager _instance = null;
    public static ScoreManager Instance => _instance;

    [Header("Affect Score"), Tooltip("Mutiplacator of information which affect the final score, entre 0 and 1.")]
    [SerializeField] private float _psImportance;
    [SerializeField] private float _employeeImportance;
    [SerializeField] private float _roomLevelMaxImportance;

    private int _totalEmployee;
    private int _totalRoomLevelMax;
    public int TotalPS { get; private set; }
    public float FinalScore { get; private set; }

    [Space, Header("Minimum number score for have a star")]
    [SerializeField] private float _oneStar;
    [SerializeField] private float _twoStars;
    [SerializeField] private float _threeStars;

    public event System.Action<float> ShowScore;
    public event System.Action<float> ShowBestScore;
    public event System.Action<int> ShowPS;
    public event System.Action<int> ShowStars;

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
        if(ChronoManager.Instance != null)
        {
            ChronoManager.Instance.ChronoEnded += CalculateScore;
        }
        CheckBestScore();
    }

    public void AddPS(int _psWin)
    {
        //if (EventManager.Instance.CurrentEvent && EventManager.Instance.ActualEvent.PSMultiplicator > 0)
        //{
        //    TotalPS = TotalPS + (int)Mathf.Round(_psWin * EventManager.Instance.ActualEvent.PSMultiplicator);
        //}
        //else
        //{ 
        TotalPS = TotalPS + _psWin;
        ShowPS?.Invoke(TotalPS);
        //}
    }

    public void AddEmployee()
    { _totalEmployee++; }

    public void AddRoomLevelMax()
    { _totalRoomLevelMax++; }

    public void CalculateScore()
    {
        FinalScore = Mathf.Round((TotalPS * _psImportance) + (_totalEmployee * _employeeImportance) + (_totalRoomLevelMax * _roomLevelMaxImportance));

        ShowScore?.Invoke(FinalScore);
        CheckStars();
    }

    public void CheckStars()
    {
        int numberOfStars = 0;

        if (FinalScore > _oneStar)
        {
            numberOfStars = 1;
        }
        if (FinalScore > _twoStars)
        {
            numberOfStars = 2;
        }
        if (FinalScore > _threeStars)
        {
            numberOfStars = 3;
        }

        ShowStars?.Invoke(numberOfStars);
    }

    public void CheckBestScore()
    {
        Scene _actualScene = SceneManager.GetActiveScene();

        if (FinalScore > PlayerPrefs.GetFloat("BestScore"))
        {
            PlayerPrefs.SetFloat("BestScore", FinalScore);
        }
        if (PlayerPrefs.GetFloat("BestScore") > 0 && _actualScene.name == "Menu")
        {
            ShowBestScore?.Invoke(PlayerPrefs.GetFloat("BestScore"));
        }
    }
}
