using UnityEngine;
using UnityEngine.UI;

public class SFXSetting : MonoBehaviour
{
    [SerializeField]
    private Slider _SFXSlider;

    private void Start()
    {
        _SFXSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSFXVolume);
    }

    private void OnEnable()
    {
        _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
