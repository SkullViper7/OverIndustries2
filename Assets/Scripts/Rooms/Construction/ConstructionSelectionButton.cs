using UnityEngine;
using UnityEngine.UI;

public class ConstructionSelectionButton : MonoBehaviour
{
    /// <summary>
    /// Data of the room.
    /// </summary>
    [SerializeField]
    private RoomData _roomData;

    /// <summary>
    /// Data of the room behaviour.
    /// </summary>
    [SerializeField, InterfaceType(typeof(IRoomBehaviourData))]
    private ScriptableObject _roomBehaviourData;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    private UIManager _uiManager;

    /// <summary>
    /// Button component.
    /// </summary>
    private Button _selectionButton;

    private void Awake()
    {
        _selectionButton = GetComponent<Button>();
    }

    private void Start()
    {
        _uiManager = UIManager.Instance;
        _selectionButton.onClick.AddListener(() =>
        {
            StartSearchingForAnAvailableSpot();
            _uiManager.ConstructionPopUp.SetActive(false);
            _uiManager.ConstructionUI.SetActive(true);
            _uiManager.CloseUI();
        });
    }

    /// <summary>
    /// Called when button is clicked and start launching a research.
    /// </summary>
    private void StartSearchingForAnAvailableSpot()
    {
        GridManager.Instance.LaunchAResearch(_roomData, (IRoomBehaviourData)_roomBehaviourData);
    }
}
