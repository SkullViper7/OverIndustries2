using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageButton : MonoBehaviour
{
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
    /// Datas of the component attached to this button.
    /// </summary>
    private int _amount;

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
        _image.color = Color.red;
    }

    /// <summary>
    /// Called to initialize the button with datas of a component.
    /// </summary>
    /// <param name="componentData"> Datas of the component. </param>
    /// <param name="amount"> Amount of the component. </param>
    public void InitButtonForComponent(ComponentData componentData, int amount)
    {
        _button.interactable = true;
        _componentData = componentData;
        _picto.sprite = _componentData.ComponentPicto;
        _picto.enabled = true;
        _amount = amount;
        _amountTxt.text = amount.ToString();
        _amountTxt.enabled = true;
        _image.color = Color.green;
    }

    /// <summary>
    /// Called to initialize the button with datas of an object.
    /// </summary>
    /// <param name="objectData"> Datas of the object. </param>
    /// <param name="amount"> Amount of the object. </param>
    public void InitButtonForObject(ObjectData objectData, int amount)
    {
        _button.interactable = true;
        _objectData = objectData;
        _picto.sprite = _componentData.ComponentPicto;
        _picto.enabled = true;
        _amount = amount;
        _amountTxt.text = amount.ToString();
        _amountTxt.enabled = true;
        _image.color = Color.green;
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
        _image.color = Color.red;
    }
}
