using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField]
    private Slider _musicSlider;

    private void OnEnable()
    {
        _musicSlider.onValueChanged.AddListener(SetVolume);

        if (PlayerPrefs.HasKey("MusicVolume"))
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
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(newValue) * 20);
       SaveVolume(newValue);
    }

    private void SaveVolume(float newValue)
    {
        PlayerPrefs.SetFloat("MusicVolume", newValue);
    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }
}
