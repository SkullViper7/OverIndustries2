using UnityEngine;
using UnityEngine.UI;

public class SwitchButtons : MonoBehaviour
{
    [SerializeField]
    private Sprite _selected;

    [SerializeField]
    private Sprite _unselected;

    private Image _image;

    private Button _button;

    public void Select()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _image.sprite = _selected;
        _button.interactable = false;
    }

    public void Unselect()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _image.sprite = _unselected;
        _button.interactable = true;
    }
}
