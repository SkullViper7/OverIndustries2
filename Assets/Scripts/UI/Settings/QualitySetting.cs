using TMPro;
using UnityEngine;

public class QualitySetting : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _text;

    private int _qualityLvl;

    private void Awake()
    {
        _qualityLvl = 0;
        SetQualityLvl(0);
    }

    public void SetQualityLvl(int valueToAdd)
    {
        _qualityLvl += valueToAdd;

        if (_qualityLvl < 1 )
        {
            _qualityLvl = 3;
        }
        else if (_qualityLvl > 3)
        {
            _qualityLvl = 1;
        }

        switch(_qualityLvl)
        {
            case 1:
                _text.text = "Performance";
                QualitySettings.SetQualityLevel(0);
                break;
            case 2:
                _text.text = "Équilibré";
                QualitySettings.SetQualityLevel(1);
                break;
            case 3:
                _text.text = "Qualité";
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }
}

