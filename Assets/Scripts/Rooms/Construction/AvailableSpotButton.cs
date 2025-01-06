using UnityEngine;
using UnityEngine.UI;

public class AvailableSpotButton : MonoBehaviour
{
    /// <summary>
    /// Prefab of a room without datas.
    /// </summary>
    [SerializeField]
    private GameObject _emptyRoom;

    /// <summary>
    /// Button component.
    /// </summary>
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void InitButton(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 position, AvailableSpotsUI availableSpotsUI)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 worldPos = GridManager.Instance.ConvertGridPosIntoWorldPos(position);
        rectTransform.position = new Vector3(worldPos.x, worldPos.y, -0.2f);
        rectTransform.sizeDelta = new Vector2(roomData.Size * 3, 4);

        _button.onClick.AddListener(() =>
        {
            ConstructRoom(roomData, roomBehaviourData, position);
            availableSpotsUI.CloseUI();
        });
    }

    /// <summary>
    /// Called to construct a room
    /// </summary>
    /// <param name="roomData"> Data of the room. </param>
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="gridPos"> Position in the grid. </param>
    private void ConstructRoom(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 gridPos)
    {
        GameObject newRoom = Instantiate(_emptyRoom);
        GridManager.Instance.AddARoomInTheGrid(newRoom.GetComponent<Room>(), roomData, roomBehaviourData, gridPos);

        // Special case with elevator
        if (roomData.RoomType == RoomType.Elevator)
        {
            if (gridPos.y - 1 >= 0)
            {
                if (GridManager.Instance.TryGetRoomAtPosition(gridPos - new Vector2(0, 1), out Room roomUnder))
                {
                    if (roomUnder.RoomData.RoomType != RoomType.Elevator)
                    {
                        if (gridPos.y + 1 <= GridManager.Instance.GridSize.y - 1)
                        {
                            if (!GridManager.Instance.CheckOccupiedSpots(gridPos + new Vector2(0, 1)))
                            {
                                GameObject newElevator = Instantiate(_emptyRoom);
                                GridManager.Instance.AddARoomInTheGrid(newElevator.GetComponent<Room>(), roomData, roomBehaviourData, gridPos + new Vector2(0, 1));
                            }
                        }
                    }
                }
            }
            else
            {
                if (gridPos.y + 1 <= GridManager.Instance.GridSize.y - 1)
                {
                    if (!GridManager.Instance.CheckOccupiedSpots(gridPos + new Vector2(0, 1)))
                    {
                        GameObject newElevator = Instantiate(_emptyRoom);
                        GridManager.Instance.AddARoomInTheGrid(newElevator.GetComponent<Room>(), roomData, roomBehaviourData, gridPos + new Vector2(0, 1));
                    }
                }
            }
        }
    }
}
