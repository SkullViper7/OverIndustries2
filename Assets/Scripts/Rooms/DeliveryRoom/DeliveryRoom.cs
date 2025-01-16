using System;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Data of the delivery room.
    /// </summary>
    public DeliveryRoomData DeliveryRoomData { get; private set; }

    /// <summary>
    /// A reference to the raw material storage.
    /// </summary>
    private RawMaterialStorage _storage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Current chrono of the delivery.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// Current time to product an amount of raw material.
    /// </summary>
    public int CurrentDeliveryTime { get; private set; }

    /// <summary>
    /// Current amount producted per delivery.
    /// </summary>
    public int CurrentProductionPerDelivery { get; private set; }

    /// <summary>
    /// Current amount of raw material in internal storage of the room.
    /// </summary>
    public int CurrentAmountInInternalStorage { get; private set; }

    /// <summary>
    /// Current internal capacity of the room.
    /// </summary>
    public int CurrentInternalCapacity { get; private set; }

    /// <summary>
    /// A value indicating if a production cycle has started.
    /// </summary>
    public bool ProductionCycleHasStarted { get; private set; }

    /// <summary>
    /// Notification of the room when raw material is recoverable.
    /// </summary>
    private RoomNotifiction _roomNotification;

    public event Action<int> NewChrono, NewProductionCount;

    public event Action ProductionStart, ProductionCantStart, ProductionGet;

    /// <summary>
    /// Called at the start to initialize the delivery room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        DeliveryRoomData = (DeliveryRoomData)behaviourData;
        _storage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

        UpgradeRoom(1);
        _roomMain.NewLvl += UpgradeRoom;

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(AddRawMaterialInStorage);
        }

        _roomMain.UpgradeEnd += TryStartProductionCycle;
        _roomMain.EmployeesHaveChanged += TryStartProductionCycle;
    }

    private void TryStartProductionCycle()
    {
        ProductionGet -= TryStartProductionCycle;

        if (_roomMain.EmployeeAssign.Count > 0)
        {
            // If there is the good employee in the room launch a cycle
            for (int i = 0; i < _roomMain.EmployeeAssign.Count; i++)
            {
                if (_roomMain.EmployeeAssign[i].EmployeeJob[0].JobType == DeliveryRoomData.JobNeeded.JobType)
                {
                    if (!ProductionCycleHasStarted)
                    {
                        ProductionStart?.Invoke();

                        _roomMain.EmployeesHaveChanged -= TryStartProductionCycle;
                        _roomMain.EmployeesHaveChanged += TryStartProductionCycle;

                        ProductionCycleHasStarted = true;

                        ChronoManager.Instance.NewSecondTick += DeliveryUpdateChrono;
                    }

                    break;
                }
                else
                {
                    ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
                    _currentChrono = 0;

                    ProductionCantStart?.Invoke();

                    ProductionCycleHasStarted = false;

                    _roomMain.EmployeesHaveChanged += TryStartProductionCycle;
                }
            }
        }
        else
        {
            ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
            _currentChrono = 0;

            ProductionCantStart?.Invoke();

            ProductionCycleHasStarted = false;

            _roomMain.EmployeesHaveChanged += TryStartProductionCycle;
            ProductionGet += TryStartProductionCycle;
        }
    }

    /// <summary>
    /// Called each second to update the delivery chrono.
    /// </summary>
    private void DeliveryUpdateChrono()
    {
        if (CurrentAmountInInternalStorage < CurrentInternalCapacity)
        {
            if (_currentChrono + 1 >= CurrentDeliveryTime)
            {
                _currentChrono = 0;
                NewChrono?.Invoke(_currentChrono);
                AddRawMaterialInInternalStorage(CurrentProductionPerDelivery);
            }
            else
            {
                _currentChrono++;
                NewChrono?.Invoke(_currentChrono);
            }
        }
        else
        {
            ProductionCycleHasStarted = false;
            ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
            ProductionGet += TryStartProductionCycle;
        }
    }

    /// <summary>
    /// Called at the end of a delivery to add an amount in the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to add in the internal storage. </param>
    private void AddRawMaterialInInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage += amount;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);

        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
    }

    /// <summary>
    /// Called when raw material is added to the storage to substracte it from the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to substract from the internal storage. </param>
    private void RemoveRawMaterialFromInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage -= amount;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);

        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
    }

    /// <summary>
    /// Called to add raw materials to the storage when player interact with the room.
    /// </summary>
    private void AddRawMaterialInStorage()
    {
        if (!_storage.IsStorageFull())
        {
            int remainingStorage = _storage.GetRemainingStorage();

            if (remainingStorage >= CurrentAmountInInternalStorage)
            {
                _storage.AddRawMaterials(CurrentAmountInInternalStorage);
                RemoveRawMaterialFromInternalStorage(CurrentAmountInInternalStorage);
                ProductionGet?.Invoke();
            }
            else
            {
                _storage.AddRawMaterials(remainingStorage);
                RemoveRawMaterialFromInternalStorage(remainingStorage);
                ProductionGet?.Invoke();
            }
        }
    }

    /// <summary>
    /// Called to upgrad some values when the room is upgraded.
    /// </summary>
    /// <param name="newLvl"> New lvl of the room. </param>
    private void UpgradeRoom(int newLvl)
    {
        switch (newLvl)
        {
            case 1:
                ProductionCycleHasStarted = false;
                ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
                _currentChrono = 0;
                NewChrono?.Invoke(_currentChrono);
                RemoveRawMaterialFromInternalStorage(CurrentAmountInInternalStorage);
                CurrentDeliveryTime = DeliveryRoomData.DeliveryTimeAtLvl1;
                CurrentProductionPerDelivery = DeliveryRoomData.ProductionPerDeliveryAtLvl1;
                CurrentInternalCapacity = DeliveryRoomData.InternalStorageAtLvl1;
                break;
            case 2:
                ProductionCycleHasStarted = false;
                ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
                _currentChrono = 0;
                NewChrono?.Invoke(_currentChrono);
                RemoveRawMaterialFromInternalStorage(CurrentAmountInInternalStorage);
                CurrentDeliveryTime = DeliveryRoomData.DeliveryTimeAtLvl2;
                CurrentProductionPerDelivery = DeliveryRoomData.ProductionPerDeliveryAtLvl2;
                CurrentInternalCapacity = DeliveryRoomData.InternalStorageAtLvl2;
                break;
            case 3:
                ProductionCycleHasStarted = false;
                ChronoManager.Instance.NewSecondTick -= DeliveryUpdateChrono;
                _currentChrono = 0;
                NewChrono?.Invoke(_currentChrono);
                RemoveRawMaterialFromInternalStorage(CurrentAmountInInternalStorage);
                CurrentDeliveryTime = DeliveryRoomData.DeliveryTimeAtLvl3;
                CurrentProductionPerDelivery = DeliveryRoomData.ProductionPerDeliveryAtLvl3;
                CurrentInternalCapacity = DeliveryRoomData.InternalStorageAtLvl3;
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}