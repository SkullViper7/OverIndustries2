using UnityEngine;
using UnityEngine.UI;

public class FPSSetting : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(SettingsManager.Instance.ActivateLowSetting);
    }

    private void OnEnable()
    {
        _toggle.isOn = PlayerPrefs.GetInt("BatterySavingSetting") == 1;
    }
}
