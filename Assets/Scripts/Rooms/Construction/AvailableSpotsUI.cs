using System.Collections.Generic;
using UnityEngine;

public class AvailableSpotsUI : MonoBehaviour
{
    /// <summary>
    /// Prefab of the button.
    /// </summary>
    [SerializeField]
    private GameObject _spotButton;

    private void Start()
    {
        GridManager.Instance.AvailableSpotsResult += ShowAvailableSpots;
    }

    /// <summary>
    /// Called to display buttons where player can construct a room.
    /// </summary>
    /// <param name="roomData"> Data of the room. </param>
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="availableSpots"> Position where the room is placable. </param>
    private void ShowAvailableSpots(RoomData roomData, IRoomBehaviourData roomBehaviourData, List<Vector2> availableSpots)
    {
        for (int i = 0; i < availableSpots.Count; i++)
        {
            GameObject newButton = Instantiate(_spotButton, transform);
            newButton.GetComponent<AvailableSpotButton>().InitButton(roomData, roomBehaviourData, availableSpots[i]);
        }
    }
}
