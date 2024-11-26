using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoPopUp : MonoBehaviour
{
    /// <summary>
    /// The button to close the pop up.
    /// </summary>
    [SerializeField]
    private Button _closeButton;

    /// <summary>
    /// The background which close the pop up when clicked.
    /// </summary>
    [SerializeField]
    private Button _background;

    /// <summary>
    /// The text where name and lvl are displayed.
    /// </summary>
    [SerializeField]
    private TMP_Text _nameLvl;

    /// <summary>
    /// The text where description is displayed.
    /// </summary>
    [SerializeField]
    private TMP_Text _description;

    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePopUp);
        _background.onClick.AddListener(ClosePopUp);
    }

    /// <summary>
    /// Called to display data of the room.
    /// </summary>
    /// <param name="roomMain"> Main component of the room. </param>
    /// <param name="roomData"> Generic data of the room. </param>
    /// <param name="roomBehaviourData"> Data of the room's behaviour. </param>
    public void DisplayDatas(Room roomMain, RoomData roomData, IRoomBehaviourData roomBehaviourData)
    {
        _nameLvl.text = roomData.Name + " (Niveau " + roomMain.CurrentLvl.ToString() + ")";
        _description.text = roomData.Description;
    }

    /// <summary>
    /// Called to close the pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _nameLvl.text = "";
        _description.text = "";
        gameObject.SetActive(false);
    }
}
