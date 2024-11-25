using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void Switch()
    {
        if (_toggle.isOn)
            _panel.SetActive(true);
        else
            _panel.SetActive(false);
    }
}
