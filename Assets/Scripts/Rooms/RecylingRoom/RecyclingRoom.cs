using UnityEngine;

public class RecyclingRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the recycling room.
    /// </summary>
    [field: SerializeField]
    public RecyclingRoomData RecyclingRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private ItemStorage _itemStorage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private RoomTemp _roomMain;



    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, RoomTemp roomMain)
    {
        RecyclingRoomData = (RecyclingRoomData)behaviourData;
        _itemStorage = ItemStorage.Instance;
        _roomMain = roomMain;
    }

}
