using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

public class RoomTemp : MonoBehaviour
{
    /// <summary>
    /// Data of the room.
    /// </summary>
    [field : SerializeField]
    public RoomData RoomData { get; private set; }

    /// <summary>
    /// Data of the room.
    /// </summary>
    [field: SerializeField, InterfaceType(typeof(IRoomBehaviourData))]
    public ScriptableObject RoomBehaviourData { get; private set; }

    /// <summary>
    /// Current lvl of the room.
    /// </summary>
    public int CurrentLvl { get; private set; }

    /// <summary>
    /// Events to get the lvl when the room is upgraded.
    /// </summary>
    public delegate void UpgradeDelegate(int newLvl);
    public event UpgradeDelegate NewLvl;

    private void Start()
    {
        InitRoom(RoomData, (IRoomBehaviourData)RoomBehaviourData);
    }

    /// <summary>
    /// Called at the start to initialize the room.
    /// </summary>
    /// <param name="roomData"> Generic data of the room. </param>
    /// <param name="roomBehaviourData"> Specific data of the room's behaviour. </param>
    public void InitRoom(RoomData roomData, IRoomBehaviourData roomBehaviourData)
    {
        CurrentLvl = 1;
        //RoomData = roomData;

        switch (roomData.RoomType)
        {
            case RoomType.Delivery:
                DeliveryRoom deliveryRoom = (DeliveryRoom)gameObject.AddComponent(typeof(DeliveryRoom));
                deliveryRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)gameObject.AddComponent(typeof(MachiningRoom));
                machiningRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Storage:
                StorageRoom storageRoom = (StorageRoom)gameObject.AddComponent(typeof(StorageRoom));
                storageRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
            case RoomType.Assembly:
                AssemblyRoom assemblyRoom = (AssemblyRoom)gameObject.AddComponent(typeof(AssemblyRoom));
                assemblyRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
        }
    }

    /// <summary>
    /// Called to update the room.
    /// </summary>
    public void UpdateRoom()
    {
        CurrentLvl++;
        NewLvl?.Invoke(CurrentLvl);
    }
}