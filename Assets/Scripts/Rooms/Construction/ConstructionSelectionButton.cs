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
    /// Pop up where buttons are.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// UI displayed when player is constructing.
    /// </summary>
    [SerializeField]
    private GameObject _constructionUI;

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
        _selectionButton.onClick.AddListener(() => { StartSearchingForAnAvailableSpot(); _popUp.SetActive(false); _constructionUI.SetActive(true); });
    }

    /// <summary>
    /// Called when button is clicked and start launching a research.
    /// </summary>
    private void StartSearchingForAnAvailableSpot()
    {
        GridManager.Instance.LaunchAResearch(_roomData, (IRoomBehaviourData)_roomBehaviourData);
    }
}
