using UnityEngine;

public class RawMaterialStorageRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the storage room.
    /// </summary>
    [field: SerializeField]
    public RawMaterialStorageRoomData RawMaterialStorageRoomData { get; private set; }

    /// <summary>
    /// A reference to the raw material storage.
    /// </summary>
    private RawMaterialStorage _storage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Called at the start to initialize the storage room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        RawMaterialStorageRoomData = (RawMaterialStorageRoomData)behaviourData;
        _storage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

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
                _storage.IncreaseCapacity(RawMaterialStorageRoomData.CapacityBonusAtLvl1);
                break;
            case 2:
                _storage.IncreaseCapacity(RawMaterialStorageRoomData.CapacityBonusAtLvl2);
                break;
            case 3:
                _storage.IncreaseCapacity(RawMaterialStorageRoomData.CapacityBonusAtLvl3);
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}
