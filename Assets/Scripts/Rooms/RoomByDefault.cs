using UnityEngine;

public class RoomByDefault : MonoBehaviour
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
    /// Position of the room in the grid.
    /// </summary>
    [SerializeField]
    private Vector2 _roomPosition;

    public void Start()
    {
        GetComponent<Room>().InitRoom(_roomData, (IRoomBehaviourData)_roomBehaviourData, GridManager.Instance.ConvertGridPosIntoWorldPos(_roomPosition));
    }
}
