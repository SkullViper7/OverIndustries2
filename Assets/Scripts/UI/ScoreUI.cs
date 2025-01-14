using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("UI reference")]
    [SerializeField] private GameObject _scorePopUp;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Start()
    {
        ScoreManager.Instance.ShowScore += ShowScore;
    }

    public void ShowScore(float score)
    {
        _scorePopUp.SetActive(true);
        _scoreText.text = score.ToString();
    }
}
