using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("UI reference")]
    [SerializeField] private GameObject _scorePopUp;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _psText;

    private void Start()
    {
        ScoreManager.Instance.ShowScore += ShowScore;
        ScoreManager.Instance.ShowPS += ShowPS;
    }

    public void ShowScore(float score)
    {
        _scorePopUp.SetActive(true);
        _scoreText.text = score.ToString();
    }

    public void ShowPS(int ps)
    {
        _psText.text = ps.ToString();
    }
}
