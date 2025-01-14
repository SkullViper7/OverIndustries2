using UnityEngine;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;

    private void Start()
    {
        _musicSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
    }

    private void OnEnable()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
}
