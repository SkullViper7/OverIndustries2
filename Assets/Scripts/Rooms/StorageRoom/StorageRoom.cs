using UnityEngine;

public class StorageRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the storage room.
    /// </summary>
    [field: SerializeField]
    public StorageRoomData StorageRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private ItemStorage _storage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private RoomTemp _roomMain;

    /// <summary>
    /// Called at the start to initialize the storage room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, RoomTemp roomMain)
    {
        StorageRoomData = (StorageRoomData)behaviourData;
        _storage = ItemStorage.Instance;
        _roomMain = roomMain;

        IncreaseStorageCapacity(1);

        _roomMain.NewLvl += IncreaseStorageCapacity;
    }

    /// <summary>
    /// Called to increase the storage when the room is upgraded.
    /// </summary>
    /// <param name="newLvl"> New lvl of the room. </param>
    private void IncreaseStorageCapacity(int newLvl)
    {
        switch (newLvl)
        {
            case 1:
                _storage.IncreaseCapacity(StorageRoomData.CapacityBonusAtLvl1);
                break;
            case 2:
                _storage.IncreaseCapacity(StorageRoomData.CapacityBonusAtLvl2);
                break;
            case 3:
                _storage.IncreaseCapacity(StorageRoomData.CapacityBonusAtLvl3);
                break;
        }
    }
}
