using TMPro;
using UnityEngine;

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

    [Space, Header("Point de satisfaction")]
    [SerializeField] private TextMeshProUGUI _PSText;

    public event System.Action<float> ShowScore;

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
        ChronoManager.Instance.ChronoEnded += CalculateScore;
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
        //}
    }

    public void AddEmployee()
    { _totalEmployee++; }

    public void AddRoomLevelMax()
    { _totalRoomLevelMax++; }

    public void CalculateScore()
    {
        FinalScore = Mathf.Round((TotalPS * _psImportance) + (_totalEmployee * _employeeImportance) + (_totalRoomLevelMax * _roomLevelMaxImportance));
        Debug.Log("Score : " + FinalScore);
        ShowScore?.Invoke(FinalScore);
    }
}
