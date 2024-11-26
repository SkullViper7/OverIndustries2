using UnityEngine;
using UnityEngine.UI;

public class RoomInfoButton : MonoBehaviour
{
    /// <summary>
    /// The pop up which displays room's infos.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// A reference to the script which manage the pop up.
    /// </summary>
    private RoomInfoPopUp _roomInfoPopUp;

    /// <summary>
    /// Current main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Current data of the room.
    /// </summary>
    private RoomData _roomData;

    /// <summary>
    /// Current behaviour data of the room.
    /// </summary>
    private IRoomBehaviourData _roomBehaviourData;

    private void Start()
    {
        _roomInfoPopUp = _popUp.GetComponent<RoomInfoPopUp>();
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    /// <summary>
    /// Called to initialize data of the room in the button.
    /// </summary>
    ///  <param name="roomMain"> Main component of the room. </param>
    /// <param name="roomData"> Generic data of the room. </param>
    /// <param name="roomBehaviourData"> Data of the room's behaviour. </param>
    public void IniButton(Room roomMain, RoomData roomData, IRoomBehaviourData roomBehaviourData)
    {
        _roomMain = roomMain;
        _roomData = roomData;
        _roomBehaviourData = roomBehaviourData;
    }

    /// <summary>
    /// Called to desactivate the button.
    /// </summary>
    public void DesactivateButton()
    {
        _roomData = null;
        _roomBehaviourData = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the button is clicked.
    /// </summary>
    private void OnButtonClicked()
    {
        _popUp.SetActive(true);
        _roomInfoPopUp.DisplayDatas(_roomMain, _roomData, _roomBehaviourData);
    }
}
