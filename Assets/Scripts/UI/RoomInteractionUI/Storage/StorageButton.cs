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
    private ComponentData _componentData;

    /// <summary>
    /// Datas of the object attached to this button.
    /// </summary>
    private ObjectData _objectData;

    /// <summary>
    /// Amount of items attached to this button.
    /// </summary>
    private int _amount;

    public event Action<StorageButton> ButtonClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _button.interactable = false;
        _componentData = null;
        _objectData = null;
        _picto.enabled = false;
        _picto.sprite = null;
        _amount = 0;
        _amountTxt.text = "";
        _amountTxt.enabled = false;
        _image.sprite = _noItem;

        _button.onClick.AddListener(SelectButton);
    }

    /// <summary>
    /// Called to initialize the button with datas of a component.
    /// </summary>
    /// <param name="componentData"> Datas of the component. </param>
    /// <param name="amount"> Amount of the component. </param>
    public void InitButtonForComponent(ComponentData componentData, int amount)
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _componentData = componentData;
        _picto.sprite = _componentData.ComponentPicto;
        _picto.enabled = true;
        _amount = amount;
        _amountTxt.text = "x" + amount.ToString();
        _amountTxt.enabled = true;
        _image.sprite = _itemUnselected;
        _button.interactable = true;
    }

    /// <summary>
    /// Called to initialize the button with datas of an object.
    /// </summary>
    /// <param name="objectData"> Datas of the object. </param>
    /// <param name="amount"> Amount of the object. </param>
    public void InitButtonForObject(ObjectData objectData, int amount)
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _objectData = objectData;
        _picto.sprite = _objectData.ObjectPicto;
        _picto.enabled = true;
        _amount = amount;
        _amountTxt.text = "x" + amount.ToString();
        _amountTxt.enabled = true;
        _image.sprite = _itemUnselected;
        _button.interactable = true;
    }

    /// <summary>
    /// Called to reset the button.
    /// </summary>
    public void ResetButton()
    {
        _button.interactable = false;
        _componentData = null;
        _objectData = null;
        _picto.enabled = false;
        _picto.sprite = null;
        _amount = 0;
        _amountTxt.text = "";
        _amountTxt.enabled = false;
        _image.sprite = _noItem;
    }

    private void SelectButton()
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
