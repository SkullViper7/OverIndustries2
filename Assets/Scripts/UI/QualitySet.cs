using TMPro;
using UnityEngine;

public class QualitySet : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    public void RightArrow()
    {
        switch (_text.text)
        {
            case "Faible":
                _text.text = "Moyen";
                QualitySettings.SetQualityLevel(1);
                break;

            case "Moyen":
                _text.text = "Elevé";
                QualitySettings.SetQualityLevel(2);
                break;

            case "Elevé":
                _text.text = "Faible";
                QualitySettings.SetQualityLevel(0);
                break;
        }
    }

    public void LeftArrow()
    {
        switch (_text.text)
        {
            case "Faible":
                _text.text = "Elevé";
                QualitySettings.SetQualityLevel(2);
                break;

            case "Moyen":
                _text.text = "Faible";
                QualitySettings.SetQualityLevel(0);
                break;

            case "Elevé":
                _text.text = "Moyen";
                QualitySettings.SetQualityLevel(1);
                break;
        }
    }
}

