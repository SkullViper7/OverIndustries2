using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StorageButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _noItem;

    [SerializeField]
    private Sprite _itemUnselected;

    [SerializeField]
    private Sprite _itemSelected;

    /// <summary>
    /// Picto of the component or object.
    /// </summary>
    [SerializeField]
    private Image _picto;

    /// <summary>
    /// Amount of the component or object.
    /// </summary>
    [SerializeField]
    private TMP_Text _amountTxt;

    /// <summary>
    /// Image component of the button.
    /// </summary>
    private Image _image;

    /// <summary>
    /// Button Component.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Datas of the component attached to this button.
    /// </summary>
    public ComponentData ComponentData { get; private set; }

    /// <summary>
    /// Datas of the object attached to this button.
    /// </summary>
    public ObjectData ObjectData { get; private set; }

    public event Action<StorageButton> ButtonClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectButton);
    }

    /// <summary>
    /// Called to initialize the button with datas of a component.
    /// </summary>
    /// <param name="componentData"> Datas of the component. </param>
    public void InitButtonForComponent(ComponentData componentData)
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        ComponentData = componentData;
        _picto.sprite = ComponentData.ComponentPicto;
        _picto.enabled = true;
        _amountTxt.text = "x" + ItemStorage.Instance.ReturnNumberOfThisComponent(ComponentData).ToString();
        _amountTxt.enabled = true;
        _image.sprite = _itemUnselected;
        _button.interactable = true;
    }

    /// <summary>
    /// Called to initialize the button with datas of an object.
    /// </summary>
    /// <param name="objectData"> Datas of the object. </param>
    public void InitButtonForObject(ObjectData objectData)
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        ObjectData = objectData;
        _picto.sprite = ObjectData.ObjectPicto;
        _picto.enabled = true;
        _amountTxt.text = "x" + ItemStorage.Instance.ReturnNumberOfThisObject(ObjectData).ToString();
        _amountTxt.enabled = true;
        _image.sprite = _itemUnselected;
        _button.interactable = true;
    }

    public void ReloadItemValue()
    {
        if (ComponentData != null)
        {
            _amountTxt.text = "x" + ItemStorage.Instance.ReturnNumberOfThisComponent(ComponentData).ToString();
        }
        else if (ObjectData != null)
        {
            _amountTxt.text = "x" + ItemStorage.Instance.ReturnNumberOfThisObject(ObjectData).ToString();
        }
    }

    /// <summary>
    /// Called to reset the button.
    /// </summary>
    public void ResetButton()
    {
        _button.interactable = false;
        ComponentData = null;
        ObjectData = null;
        _picto.enabled = false;
        _picto.sprite = null;
        _picto.sprite = null;
        _amountTxt.text = "";
        _amountTxt.enabled = false;
        _image.sprite = _noItem;
    }

    public void SelectButton()
    {
        _button.interactable = false;
        _image.sprite = _itemSelected;

        ButtonClicked?.Invoke(this);
    }

    public void UnselectButton()
    {
        _button.interactable = true;
        _image.sprite = _itemUnselected;
    }
}
