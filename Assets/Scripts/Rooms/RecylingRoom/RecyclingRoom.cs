using System;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the recycling room.
    /// </summary>
    [field: SerializeField] public RecyclingRoomData RecyclingRoomData { get; private set; }

    /// <summary>
    /// Current amount of raw material in internal storage of the room.
    /// </summary>
    public int CurrentAmountInInternalStorage { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private RawMaterialStorage _rawMaterialStorage;

    /// <summary>
    /// Notification of the room when raw material is recoverable.
    /// </summary>
    private RoomNotifiction _roomNotification;

    public event Action<int> NewProductionCount;

    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        RecyclingRoomData = (RecyclingRoomData)behaviourData;
        _rawMaterialStorage = RawMaterialStorage.Instance;

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(GetRawMaterialRecycled);
        }

        _rawMaterialStorage.RawMaterialToRecycle += AddRawMaterialInInternalStorage;
    }

    /// <summary>
    /// Called when an amount of raw material is recycled to add it to the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to add in the internal storage. </param>
    private void AddRawMaterialInInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage += amount;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, int.MaxValue);

        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
    }

    /// <summary>
    /// Called when raw material is added to the storage to substracte it from the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to substract from the internal storage. </param>
    private void RemoveRawMaterialFromInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage -= amount;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, int.MaxValue);

        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
    }

    /// <summary>
    /// Called when the player clicks on the notification to get the maximum amount of raw material.
    /// </summary>
    public void GetRawMaterialRecycled()
    {
        if (!_rawMaterialStorage.IsStorageFull())
        {
            int remainingStorage = _rawMaterialStorage.GetRemainingStorage();

            if (remainingStorage >= CurrentAmountInInternalStorage)
            {
                _rawMaterialStorage.RecycleRawMaterial(CurrentAmountInInternalStorage);
                RemoveRawMaterialFromInternalStorage(CurrentAmountInInternalStorage);
            }
            else
            {
                _rawMaterialStorage.RecycleRawMaterial(remainingStorage);
                RemoveRawMaterialFromInternalStorage(remainingStorage);
            }
        }
    }
}