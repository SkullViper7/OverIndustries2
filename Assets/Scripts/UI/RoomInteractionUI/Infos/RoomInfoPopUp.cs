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
    [Space, Header("Infos"), SerializeField]
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
    public void DisplayDatas()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            if (currentRoomSelected.RoomData.RoomType == RoomType.Elevator || currentRoomSelected.RoomData.RoomType == RoomType.Recycling)
            {
                _nameLvl.text = currentRoomSelected.RoomData.Name;
                _description.text = currentRoomSelected.RoomData.Description;
            }
            else
            {
                _nameLvl.text = currentRoomSelected.RoomData.Name + " (Niveau " + currentRoomSelected.CurrentLvl.ToString() + ")";
                _description.text = currentRoomSelected.RoomData.Description;
            }
        }
    }

    /// <summary>
    /// Called to close the pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _nameLvl.text = "";
        _description.text = "";
        gameObject.SetActive(false);

        GameManager.Instance.CloseUI();
    }
}
