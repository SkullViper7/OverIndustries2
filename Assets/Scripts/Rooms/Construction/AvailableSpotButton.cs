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

    public void InitButton(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 position)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = GridManager.Instance.ConvertGridPosIntoWorldPos(position);
        rectTransform.sizeDelta = new Vector2(roomData.Size * 3, 4);

        _button.onClick.AddListener(() => ConstructRoom(roomData, roomBehaviourData, transform.position));
    }

    /// <summary>
    /// Called to construct a room
    /// </summary>
    /// <param name="roomData"></param>
    /// <param name="roomBehaviourData"></param>
    /// <param name="worldPos"></param>
    private void ConstructRoom(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 worldPos)
    {
        GameObject newRoom = Instantiate(_emptyRoom);
        newRoom.GetComponent<Room>().InitRoom(roomData, roomBehaviourData, worldPos);
    }
}
