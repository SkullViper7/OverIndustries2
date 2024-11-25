using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField]
    AudioMixer _audioMixer;
    [SerializeField]
    Slider _musicSlider;
    [SerializeField]
    Slider _SFXSlider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        // Check if music volume is already stored in PlayerPrefs
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            // If it is, load the volume settings
            LoadVolume();
        }
        else
        {
            // If it isn't, set the default music volume
            SetMusicVolume();
        }

        // Check if SFX volume is already stored in PlayerPrefs
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            // If it is, load the volume settings
            LoadVolume();
        }
        else
        {
            // If it isn't, set the default SFX volume
            SetSFXVolume();
        }
    }

    /// <summary>
    /// Set the music volume.
    /// </summary>
    /// <remarks>
    /// This function gets the value from the music slider, sets it as the music volume
    /// using the audio mixer, and stores it in PlayerPrefs. The music volume is set using
    /// a logarithmic scale where the volume is raised to the power of 10 and then divided
    /// by 20 to get the decibel value.
    /// </remarks>
    public void SetMusicVolume()
    {
        // Get the value from the music slider
        float volume = _musicSlider.value;

        // Set the music volume using the audio mixer
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume)*20);

        // Store the music volume in PlayerPrefs
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    /// <summary>
    /// Set the SFX volume.
    /// </summary>
    /// <remarks>
    /// This function gets the value from the SFX slider, sets it as the SFX volume
    /// using the audio mixer, and stores it in PlayerPrefs. The SFX volume is set using
    /// a logarithmic scale where the volume is raised to the power of 10 and then divided
    /// by 20 to get the decibel value.
    /// </remarks>
    public void SetSFXVolume()
    {
        // Get the value from the SFX slider
        float volume = _SFXSlider.value;

        // Set the SFX volume using the audio mixer
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume)*20);

        // Store the SFX volume in PlayerPrefs
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// Load the volume settings from PlayerPrefs and apply them to the sliders and audio mixer.
    /// </summary>
    /// <remarks>
    /// This function gets the volume values from PlayerPrefs and sets them to the corresponding sliders and audio mixer
    /// using the SetMusicVolume and SetSFXVolume functions.
    /// </remarks>
    void LoadVolume()
    {
        // Get the music volume from PlayerPrefs and set it to the music slider
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        // Get the SFX volume from PlayerPrefs and set it to the SFX slider
        _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // Set the music volume using the audio mixer
        SetMusicVolume();

        // Set the SFX volume using the audio mixer
        SetSFXVolume();
    }
}