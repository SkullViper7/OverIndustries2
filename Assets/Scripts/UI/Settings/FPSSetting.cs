using UnityEngine;
using UnityEngine.UI;

public class FPSSetting : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(ActivateLowSetting);

        if (PlayerPrefs.HasKey("BatterySavingSetting"))
        {
            LoadSetting();
        }
        else
        {
            ActivateLowSetting(true);
        }
    }

    private void ActivateLowSetting(bool isOn)
    {
        Application.targetFrameRate = isOn ? 30 : 60;
        SaveSetting(isOn);
    }

    private void SaveSetting(bool isOn)
    {
        PlayerPrefs.SetInt("BatterySavingSetting", isOn ? 1 : 0);
    }

    private void LoadSetting()
    {
        _toggle.isOn = PlayerPrefs.GetInt("BatterySavingSetting") == 1;
        Application.targetFrameRate = PlayerPrefs.GetInt("BatterySavingSetting") == 1 ? 30 : 60;
    }
}
