using TMPro;
using UnityEngine;

public class QualitySet : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    public void RightArrow()
    {
        switch (_text.text)
        {
            case "Low":
                _text.text = "Medium";
                QualitySettings.SetQualityLevel(1);
                break;

            case "Medium":
                _text.text = "High";
                QualitySettings.SetQualityLevel(2);
                break;

            case "High":
                _text.text = "Low";
                QualitySettings.SetQualityLevel(0);
                break;
        }
    }

    public void LeftArrow()
    {
        switch (_text.text)
        {
            case "Low":
                _text.text = "High";
                QualitySettings.SetQualityLevel(2);
                break;

            case "Medium":
                _text.text = "Low";
                QualitySettings.SetQualityLevel(0);
                break;

            case "High":
                _text.text = "Medium";
                QualitySettings.SetQualityLevel(1);
                break;
        }
    }
}

