using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    // Singleton
    private static SettingsManager _instance = null;
    public static SettingsManager Instance => _instance;

    [SerializeField]
    private AudioMixer _audioMixer;

    void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            LoadQuality();
        }
        else
        {
            PlayerPrefs.SetInt("QualityLevel", 1);
            SetQuality(0);
        }

        if (PlayerPrefs.HasKey("BatterySavingSetting"))
        {
            LoadLowSetting();
        }
        else
        {
            ActivateLowSetting(false);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume(0.75f);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume(0.75f);
        }
    }

    public void SetQuality(int valueToAdd)
    {
        int qualityLvl = PlayerPrefs.GetInt("QualityLevel");
        qualityLvl += valueToAdd;

        if (qualityLvl < 1)
        {
            qualityLvl = 3;
        }
        else if (qualityLvl > 3)
        {
            qualityLvl = 1;
        }

        switch (qualityLvl)
        {
            case 1:
                QualitySettings.SetQualityLevel(0);
                break;
            case 2:
                QualitySettings.SetQualityLevel(1);
                break;
            case 3:
                QualitySettings.SetQualityLevel(2);
                break;
        }

        SaveQuality(qualityLvl);
    }

    private void SaveQuality(int qualityLevel)
    {
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
    }

    private void LoadQuality()
    {
        switch (PlayerPrefs.GetInt("QualityLevel"))
        {
            case 1:
                QualitySettings.SetQualityLevel(0);
                break;
            case 2:
                QualitySettings.SetQualityLevel(1);
                break;
            case 3:
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }

    public void ActivateLowSetting(bool isOn)
    {
        Application.targetFrameRate = isOn ? 30 : 60;
        SaveLowSetting(isOn);
    }

    private void SaveLowSetting(bool isOn)
    {
        PlayerPrefs.SetInt("BatterySavingSetting", isOn ? 1 : 0);
    }

    private void LoadLowSetting()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("BatterySavingSetting") == 1 ? 30 : 60;
    }

    public void SetMusicVolume(float newValue)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(newValue) * 20);
        SaveMusicVolume(newValue);
    }

    private void SaveMusicVolume(float newValue)
    {
        PlayerPrefs.SetFloat("MusicVolume", newValue);
    }

    private void LoadMusicVolume()
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }

    public void SetSFXVolume(float newValue)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(newValue) * 20);
        SaveSFXVolume(newValue);
    }

    private void SaveSFXVolume(float newValue)
    {
        PlayerPrefs.SetFloat("SFXVolume", newValue);
    }

    private void LoadSFXVolume()
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }
}
