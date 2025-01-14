using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualitySetting : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _text;

    [SerializeField]
    private Button _leftButton;

    [SerializeField]
    private Button _rightButton;

    private void Start()
    {
        _leftButton.onClick.AddListener(() => { SettingsManager.Instance.SetQuality(-1); SetQualityText(); });
        _rightButton.onClick.AddListener(() => { SettingsManager.Instance.SetQuality(1); SetQualityText(); });
    }

    private void OnEnable()
    {
        SetQualityText();
    }

    public void SetQualityText()
    {
        switch(PlayerPrefs.GetInt("QualityLevel"))
        {
            case 1:
                _text.text = "Performance";
                break;
            case 2:
                _text.text = "Équilibré";
                break;
            case 3:
                _text.text = "Qualité";
                break;
        }
    }
}

