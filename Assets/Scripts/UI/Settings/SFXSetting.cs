using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXSetting : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField]
    private Slider _SFXSlider;

    private void OnEnable()
    {
        _SFXSlider.onValueChanged.AddListener(SetVolume);

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume(0.75f);
        }
    }

    private void SetVolume(float newValue)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(newValue) * 20);
        SaveVolume(newValue);
    }

    private void SaveVolume(float newValue)
    {
        PlayerPrefs.SetFloat("SFXVolume", newValue);
    }

    private void LoadVolume()
    {
        _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }
}
