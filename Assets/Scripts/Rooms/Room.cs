using UnityEngine;

public class Room : MonoBehaviour
{
    /// <summary>
    /// Data of the room.
    /// </summary>
    public RoomData RoomData { get; private set; }

    /// <summary>
    /// Data of the room behaviour.
    /// </summary>
    public IRoomBehaviourData RoomBehaviourData { get; private set; }

    /// <summary>
    /// Component of the room behaviour.
    /// </summary>
    public IRoomBehaviour RoomBehaviour { get; private set; }

    /// <summary>
    /// Current lvl of the room.
    /// </summary>
    public int CurrentLvl { get; private set; }

    /// <summary>
    /// Position of the room in the grid (leftmost slot of the room)
    /// </summary>
    public Vector2 RoomPosition { get; private set; }

    /// <summary>
    /// Events to get the lvl when the room is upgraded.
    /// </summary>
    public delegate void UpgradeDelegate(int newLvl);
    public event UpgradeDelegate NewLvl;

    /// <summary>
    /// Called at the start to initialize the room.
    /// </summary>
    /// <param name="roomData"> Generic data of the room. </param>
    /// <param name="roomBehaviourData"> Specific data of the room's behaviour. </param>
    /// <param name="gridPosition"> Position of the room in the world. </param>
    public void InitRoom(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 gridPosition)
    {
        CurrentLvl = 1;
        RoomData = roomData;
        RoomBehaviourData = roomBehaviourData;
        RoomPosition = gridPosition;

        transform.position = GridManager.Instance.ConvertGridPosIntoWorldPos(RoomPosition);

        switch (roomData.RoomType)
        {
            case RoomType.Delivery:
                DeliveryRoom deliveryRoom = (DeliveryRoom)gameObject.AddComponent(typeof(DeliveryRoom));
                RoomBehaviour = deliveryRoom;
                deliveryRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)gameObject.AddComponent(typeof(MachiningRoom));
                RoomBehaviour = machiningRoom;
                machiningRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Storage:
                StorageRoom storageRoom = (StorageRoom)gameObject.AddComponent(typeof(StorageRoom));
                RoomBehaviour = storageRoom;
                storageRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Assembly:
                AssemblyRoom assemblyRoom = (AssemblyRoom)gameObject.AddComponent(typeof(AssemblyRoom));
                RoomBehaviour = assemblyRoom;
                assemblyRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
        }
    }

    /// <summary>
    /// Called to upgrade the room.
    /// </summary>
    public void UpgradeRoom()
    {
        CurrentLvl++;
        NewLvl?.Invoke(CurrentLvl);
    }
}