using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("UI reference")]
    [SerializeField] private GameObject _scorePopUp;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _psText;
    [SerializeField] private List<GameObject> _starsList;

    private void Start()
    {
        ScoreManager.Instance.ShowScore += ShowScore;
        ScoreManager.Instance.ShowBestScore += ShowBestScore;
        ScoreManager.Instance.ShowPS += ShowPS;
        ScoreManager.Instance.ShowStars += ShowStars;

        for (int i = 0; i < _starsList.Count; i++)
        {
            _starsList[i].SetActive(false);
        }
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
    
    public void ShowStars(int stars)
    {
        for (int i = 0; i < stars; i++)
        {
            _starsList[i].SetActive(true);
        }
    }

    public void ShowBestScore(float bestScore)
    {
        _scorePopUp.SetActive(true);
        _scoreText.text = bestScore.ToString();
    }
}
