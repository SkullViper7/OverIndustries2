using UnityEngine;
using UnityEngine.UI;

public class LowPower : MonoBehaviour
{
    [SerializeField] Sprite _enabled;
    [SerializeField] Sprite _disabled;

    Image _image;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
    }

    public void ToggleLowPower()
    {
        Application.targetFrameRate = Application.targetFrameRate == 30 ? 60 : 30;
        _image.sprite = Application.targetFrameRate == 30 ? _enabled : _disabled;
        Debug.Log(Application.targetFrameRate);
    }
}
