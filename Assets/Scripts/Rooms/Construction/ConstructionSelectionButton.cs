using TMPro;
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
    /// Name of the room.
    /// </summary>
    [Space, Header("Informations"), SerializeField]
    private TMP_Text _name;

    /// <summary>
    /// Preview of the room.
    /// </summary>
    [SerializeField]
    private Image _roomPreview;

    /// <summary>
    /// Description of the room.
    /// </summary>
    [SerializeField]
    private TMP_Text _description;

    /// <summary>
    /// Button to construct the room.
    /// </summary>
    [SerializeField]
    private Button _constructionButton;

    private void Start()
    {
        _uiManager = UIManager.Instance;

        _name.text = _roomData.name;
        _roomPreview.sprite = _roomData.RoomPreview;
        _description.text = _roomData.Description;

        _constructionButton.onClick.AddListener(() =>
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
