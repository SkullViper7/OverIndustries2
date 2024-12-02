using TMPro;
using UnityEngine;

public class RoomInfoPopUp : MonoBehaviour
{
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

    private void OnEnable()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            if (currentRoomSelected.RoomData.RoomType == RoomType.Elevator || currentRoomSelected.RoomData.RoomType == RoomType.Recycling)
            {
                _nameLvl.text = currentRoomSelected.RoomData.Name;
            }
            else
            {
                _nameLvl.text = currentRoomSelected.RoomData.Name + " (Niveau " + currentRoomSelected.CurrentLvl.ToString() + ")";
            }
            _description.text = currentRoomSelected.RoomData.Description;
        }
    }
}
