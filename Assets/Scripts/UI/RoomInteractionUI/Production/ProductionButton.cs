using UnityEngine;
using UnityEngine.UI;

public class ProductionButton : MonoBehaviour
{
    /// <summary>
    /// Image component of the button.
    /// </summary>
    [SerializeField]    
    private Image _image;

    /// <summary>
    /// Button Component.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Datas of the component to product with this button.
    /// </summary>
    private ComponentData _componentData;

    /// <summary>
    /// Datas of the object to product with this button.
    /// </summary>
    private ObjectData _objectData;

    /// <summary>
    /// Called to initialize the button to product a component.
    /// </summary>
    /// <param name="componentData"> Datas of the component. </param>
    public void InitButtonForComponent(ComponentData componentData)
    {
        _button = GetComponent<Button>();

        _componentData = componentData;
        _image.sprite = componentData.ComponentPicto;

        _button.onClick.AddListener(OpenComponentToResearchPopUp);
        _button.onClick.AddListener(UIManager.Instance.ClickSFX);
    }

    /// <summary>
    /// Called to initialize the button to product an object.
    /// </summary>
    /// <param name="objectData"> Datas of the object. </param>
    public void InitButtonForObject(ObjectData objectData)
    {
        _button = GetComponent<Button>();

        _objectData = objectData;
        _image.sprite = objectData.ObjectPicto;

        _button.onClick.AddListener(OpenObjectToResearchPopUp);
        _button.onClick.AddListener(UIManager.Instance.ClickSFX);
    }

    /// <summary>
    /// Called when button is clicked to launch a component research.
    /// </summary>
    private void OpenComponentToResearchPopUp()
    {
        UIManager.Instance.ItemToProductPopUp.SetActive(true);
        UIManager.Instance.ItemToProductPopUp.GetComponent<ItemToProductPopUp>().InitPopUpForComponent(_componentData);
    }

    /// <summary>
    /// Called when button is clicked to launch an object research.
    /// </summary>
    private void OpenObjectToResearchPopUp()
    {
        UIManager.Instance.ItemToProductPopUp.SetActive(true);
        UIManager.Instance.ItemToProductPopUp.GetComponent<ItemToProductPopUp>().InitPopUpForObject(_objectData);
    }
}
