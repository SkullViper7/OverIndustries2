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

    /// <summary>
    /// Cost of the room.
    /// </summary>
    [SerializeField]
    private TMP_Text _cost;

    private void Start()
    {
        _uiManager = UIManager.Instance;

        _name.text = _roomData.Name;
        _roomPreview.sprite = _roomData.RoomLvl1Preview;
        _description.text = _roomData.Description;
        _cost.text = _roomData.ConstructionCost.ToString();

        UpdateCostAvailability(RawMaterialStorage.Instance.AmoutOfRawMaterial);
        RawMaterialStorage.Instance.AmountHasChanged += UpdateCostAvailability;
    }

    /// <summary>
    /// Called to update the availability of the cost in the raw material storage.
    /// </summary>
    /// <param name="newAmount"> New amount in the raw material storage. </param>
    private void UpdateCostAvailability(int newAmount)
    {
        _constructionButton.onClick.RemoveAllListeners();

        if (newAmount >= _roomData.ConstructionCost)
        {
            _cost.color = Color.black;
            _constructionButton.onClick.AddListener(() =>
            {
                RawMaterialStorage.Instance.SubstractRawMaterials(_roomData.ConstructionCost);
                StartSearchingForAnAvailableSpot();
                _uiManager.ConstructionUI.SetActive(true);
                _uiManager.CloseUI();
                _uiManager.CloseSFX();
                _uiManager.ConstructionPopUp.SetActive(false);
            });
        }
        else
        {
            _cost.color = Color.red;
        }
    }

    /// <summary>
    /// Called when button is clicked and start launching a research.
    /// </summary>
    private void StartSearchingForAnAvailableSpot()
    {
        GridManager.Instance.LaunchAResearch(_roomData, (IRoomBehaviourData)_roomBehaviourData);
    }
}
