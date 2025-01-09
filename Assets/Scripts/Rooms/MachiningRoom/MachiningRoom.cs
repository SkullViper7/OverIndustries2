using System;
using UnityEngine;
using UnityEngine.UI;

public class MachiningRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the machining room.
    /// </summary>
    [field: SerializeField]
    public MachiningRoomData MachiningRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private ItemStorage _itemStorage;

    /// <summary>
    /// A reference to the raw material storage.
    /// </summary>
    private RawMaterialStorage _rawMaterialStorage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Current component that the room is manufacturing.
    /// </summary>
    public ComponentData CurrentComponentManufactured { get; private set; }

    /// <summary>
    /// Current chrono of the production.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// Current time to product the current component manufactured.
    /// </summary>
    public int CurrentProductionTime { get; private set; }

    /// <summary>
    /// A value indicating if a production cycle has started.
    /// </summary>
    public bool ProductionCycleHasStarted { get; private set; }

    /// <summary>
    /// Current amount of components in internal storage of the room.
    /// </summary>
    public int CurrentAmountInInternalStorage { get; private set; }

    /// <summary>
    /// Current internal capacity of the room.
    /// </summary>
    public int CurrentInternalCapacity { get; private set; }

    /// <summary>
    /// Notification of the room when a production is launched.
    /// </summary>
    private RoomNotifiction _roomNotification;

    /// <summary>
    /// A reference to the lambda who tries to launch a production each second.
    /// </summary>
    private ChronoManager.TickDelegate _productionOnHold;

    // Lock object shared between all instances
    private static readonly object _lockObject = new object();

    public event Action<int> NewChrono, NewProductionCount;
    public event Action<int, int> NewAmountInInternalStorage;

    /// <summary>
    /// Called at the start to initialize the machining room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        MachiningRoomData = (MachiningRoomData)behaviourData;
        _itemStorage = ItemStorage.Instance;
        _rawMaterialStorage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

        UpgradeRoom(1);
        _roomMain.NewLvl += UpgradeRoom;
    }

    /// <summary>
    /// Called to try to start a new production.
    /// </summary>
    /// <param name="componentToProduct"> The component we want to product. </param>
    public void StartNewProduction(ComponentData componentToProduct)
    {
        // Reset production if there is already one
        if (CurrentComponentManufactured != null || _currentChrono != 0)
        {
            StopProduction();
        }

        // Set component manufactured and production time depending on the room lvl
        CurrentComponentManufactured = componentToProduct;

        switch (_roomMain.CurrentLvl)
        {
            case 1:
                CurrentProductionTime = CurrentComponentManufactured.ProductionTimeAtLvl1;
                break;
            case 2:
                CurrentProductionTime = CurrentComponentManufactured.ProductionTimeAtLvl2;
                break;
            case 3:
                CurrentProductionTime = CurrentComponentManufactured.ProductionTimeAtLvl3;
                break;
        }

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(AddComponentsInStorage);
        }

        TryStartProductionCycle();
    }

    private void TryStartProductionCycle()
    {
        // Synchronization between instances
        lock (_lockObject)
        {
            // If there is enough raw material in storage, launch production, else, checks each second if it is possible
            if (CurrentAmountInInternalStorage < CurrentInternalCapacity && _rawMaterialStorage.ThereIsEnoughRawMaterialInStorage(CurrentComponentManufactured.Cost))
            {
                // Remove production on hold if there is one
                if (_productionOnHold != null)
                {
                    ChronoManager.Instance.NewSecondTick -= _productionOnHold;
                    _productionOnHold = null;
                }

                // Remove cost of the component from the raw material storage
                _rawMaterialStorage.SubstractRawMaterials(CurrentComponentManufactured.Cost);

                ChronoManager.Instance.NewSecondTick += ProductionUpdateChrono;
                ProductionCycleHasStarted = true;
            }
            else
            {
                if (_productionOnHold == null)
                {
                    _productionOnHold = () => TryStartProductionCycle();
                    ChronoManager.Instance.NewSecondTick += _productionOnHold;
                }
            }
        }
    }

    /// <summary>
    /// Called each second to update the production chrono.
    /// </summary>
    private void ProductionUpdateChrono()
    {
        if (_currentChrono + 1 >= CurrentProductionTime)
        {
            _currentChrono = 0;
            NewChrono?.Invoke(_currentChrono);
            AddComponentInInternalStorage();

            ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
            ProductionCycleHasStarted = false;
            TryStartProductionCycle();
        }
        else
        {
            _currentChrono++;
            NewChrono?.Invoke(_currentChrono);
        }
    }

    /// <summary>
    /// Called at the end of a production to add a component in the internal storage.
    /// </summary>
    private void AddComponentInInternalStorage()
    {
        CurrentAmountInInternalStorage++;
        Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
        NewAmountInInternalStorage?.Invoke(CurrentAmountInInternalStorage, CurrentInternalCapacity);
    }

    /// <summary>
    /// Called when components are added to the storage to substracte it from the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to substract from the internal storage. </param>
    private void RemoveComponentsFromInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage -= amount;
        Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
        NewAmountInInternalStorage?.Invoke(CurrentAmountInInternalStorage, CurrentInternalCapacity);
    }

    /// <summary>
    /// Called to add components to the storage when player interact with the room.
    /// </summary>
    private void AddComponentsInStorage()
    {
        if (!_itemStorage.IsStorageFull())
        {
            int remainingStorage = _itemStorage.GetRemainingStorage();

            if (remainingStorage >= CurrentAmountInInternalStorage)
            {
                _itemStorage.AddComponents(CurrentComponentManufactured, CurrentAmountInInternalStorage);
                RemoveComponentsFromInternalStorage(CurrentAmountInInternalStorage);
            }
            else
            {
                _itemStorage.AddComponents(CurrentComponentManufactured, remainingStorage);
                RemoveComponentsFromInternalStorage(remainingStorage);
            }
        }
    }

    /// <summary>
    /// Called to stop the current production.
    /// </summary>
    public void StopProduction()
    {
        // Remove production on hold if there is one
        if (_productionOnHold != null)
        {
            ChronoManager.Instance.NewSecondTick -= _productionOnHold;
            _productionOnHold = null;
        }

        ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;

        if (ProductionCycleHasStarted)
        {
            ProductionCycleHasStarted = false;

            // Refund player
            _rawMaterialStorage.AddRawMaterials(CurrentComponentManufactured.Cost);
        }

        _roomNotification.DesactivateNotification();
        _roomNotification = null;

        _currentChrono = 0;
        CurrentComponentManufactured = null;
        CurrentAmountInInternalStorage = 0;
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
                _currentChrono = 0;
                CurrentInternalCapacity = MachiningRoomData.InternalStorageAtLvl1;
                break;
            case 2:
                _currentChrono = 0;
                CurrentInternalCapacity = MachiningRoomData.InternalStorageAtLvl2;
                break;
            case 3:
                _currentChrono = 0;
                CurrentInternalCapacity = MachiningRoomData.InternalStorageAtLvl3;
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}