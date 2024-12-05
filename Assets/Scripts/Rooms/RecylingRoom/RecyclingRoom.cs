using UnityEngine;

public class RecyclingRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the recycling room.
    /// </summary>
    [field: SerializeField] public RecyclingRoomData RecyclingRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private RawMaterialStorage _rawMaterialStorage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        RecyclingRoomData = (RecyclingRoomData)behaviourData;
        _rawMaterialStorage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

        _rawMaterialStorage.RawMaterialToRecycle += Recycle;
    }

    public void Recycle(int _rawMaterialToRecycle)
    {
        //Reclycling event
        if (EventManager.Instance.CurrentEvent && EventManager.Instance.ActualEvent.RoomTypeNeed == RoomType.Recycling)
        {
            _rawMaterialStorage.AddRawMaterials(_rawMaterialToRecycle * 2);
            _rawMaterialStorage.TotalRecyclingRawMaterial += _rawMaterialToRecycle * 2;
        }
        else
        {
            _rawMaterialStorage.AddRawMaterials(_rawMaterialToRecycle);
            _rawMaterialStorage.TotalRecyclingRawMaterial += _rawMaterialToRecycle;
        }
    }
}