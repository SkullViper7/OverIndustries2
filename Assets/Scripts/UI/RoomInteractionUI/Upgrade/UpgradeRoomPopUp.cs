using TMPro;
using UnityEngine;

public class UpgradeRoomPopUp : MonoBehaviour
{
    /// <summary>
    /// The text where name and next lvl are displayed.
    /// </summary>
    [Space, Header("Infos"), SerializeField]
    private TMP_Text _nameLvl;

    private void OnEnable()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            _nameLvl.text = "Améliorer " + currentRoomSelected.RoomData.Name + " au niveau " + (currentRoomSelected.CurrentLvl + 1).ToString();
        }
    }

    /// <summary>
    /// Called to upgrade the current selected room.
    /// </summary>
    public void UpgradeCurrentSelectedRoom()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            currentRoomSelected.UpgradeRoom();
        }
    }
}
