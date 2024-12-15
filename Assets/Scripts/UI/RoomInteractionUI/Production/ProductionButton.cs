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

        _button.onClick.AddListener(ComponentButtonClicked);
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

        _button.onClick.AddListener(ObjectButtonClicked);
    }

    /// <summary>
    /// Called when button is clicked to launch a component production.
    /// </summary>
    private void ComponentButtonClicked()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;
            machiningRoom.StartNewProduction(_componentData);
            UIManager.Instance.RoomProductionPopUp.GetComponent<RoomProductionPopUp>().ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }

    /// <summary>
    /// Called when button is clicked to launch an object production.
    /// </summary>
    private void ObjectButtonClicked()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;
            assemblyRoom.StartNewProduction(_objectData);
            UIManager.Instance.RoomProductionPopUp.GetComponent<RoomProductionPopUp>().ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }
}
