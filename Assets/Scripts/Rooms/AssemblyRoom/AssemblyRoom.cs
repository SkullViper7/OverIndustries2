using System;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the assembly room.
    /// </summary>
    public AssemblyRoomData AssemblyRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private ItemStorage _itemStorage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Current object that the room is manufacturing.
    /// </summary>
    public ObjectData CurrentObjectManufactured { get; private set; }

    /// <summary>
    /// Current chrono of the production.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// Current time to product the current object manufactured.
    /// </summary>
    public int CurrentProductionTime { get; private set; }

    /// <summary>
    /// Current amount of objects in internal storage of the room.
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
    /// Called at the start to initialize the assembling room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        AssemblyRoomData = (AssemblyRoomData)behaviourData;
        _itemStorage = ItemStorage.Instance;
        _roomMain = roomMain;

        UpgradeRoom(1);
        _roomMain.NewLvl += UpgradeRoom;
    }

    /// <summary>
    /// Called to start a new production.
    /// </summary>
    /// <param name="objectToProduct"> The object we want to product. </param>
    public void StartNewProduction(ObjectData objectToProduct)
    {
        // Reset production if there is already one
        if (CurrentObjectManufactured != null || _currentChrono != 0)
        {
            StopProduction();
        }

        // Set object manufactured and production time depending on the room lvl
        CurrentObjectManufactured = objectToProduct;

        switch (_roomMain.CurrentLvl)
        {
            case 1:
                CurrentProductionTime = CurrentObjectManufactured.ProductionTimeAtLvl1;
                break;
            case 2:
                CurrentProductionTime = CurrentObjectManufactured.ProductionTimeAtLvl2;
                break;
            case 3:
                CurrentProductionTime = CurrentObjectManufactured.ProductionTimeAtLvl3;
                break;
        }

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(AddObjectsInStorage);
        }

        TryStartProductionCycle();
    }

    /// <summary>
    /// Called to try to start a production cycle.
    /// </summary>
    private void TryStartProductionCycle()
    {
        // Synchronization between instances
        lock (_lockObject)
        {
            // If there is enough components in storage, launch production, else, checks each second if it is possible
            if (CurrentAmountInInternalStorage < CurrentInternalCapacity && _itemStorage.ThereIsEnoughIngredientsInStorage(CurrentObjectManufactured.Ingredients))
            {
                // Remove production on hold if there is one
                if (_productionOnHold != null)
                {
                    ChronoManager.Instance.NewSecondTick -= _productionOnHold;
                    _productionOnHold = null;
                }

                // Remove recipe of the object from the raw material storage
                _itemStorage.SubstractRecipe(CurrentObjectManufactured.Ingredients);

                ChronoManager.Instance.NewSecondTick += ProductionUpdateChrono;
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
            AddObjectInInternalStorage();

            ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
            TryStartProductionCycle();
        }
        else
        {
            _currentChrono++;
            NewChrono?.Invoke(_currentChrono);
        }
    }

    /// <summary>
    /// Called at the end of a production to add an object in the internal storage.
    /// </summary>
    private void AddObjectInInternalStorage()
    {
        CurrentAmountInInternalStorage++;
        Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
        NewAmountInInternalStorage?.Invoke(CurrentAmountInInternalStorage, CurrentInternalCapacity);
    }

    /// <summary>
    /// Called when objects are added to the storage to substracte it from the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to substract from the internal storage. </param>
    private void RemoveObjectsFromInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage -= amount;
        Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
        NewAmountInInternalStorage?.Invoke(CurrentAmountInInternalStorage, CurrentInternalCapacity);
    }

    /// <summary>
    /// Called to add objects to the storage when player interact with the room.
    /// </summary>
    private void AddObjectsInStorage()
    {
        if (!_itemStorage.IsStorageFull())
        {
            int remainingStorage = _itemStorage.GetRemainingStorage();

            if (remainingStorage >= CurrentAmountInInternalStorage)
            {
                _itemStorage.AddObjects(CurrentObjectManufactured.ObjectType, CurrentAmountInInternalStorage);
                RemoveObjectsFromInternalStorage(CurrentAmountInInternalStorage);
            }
            else
            {
                _itemStorage.AddObjects(CurrentObjectManufactured.ObjectType, remainingStorage);
                RemoveObjectsFromInternalStorage(remainingStorage);
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

        _roomNotification.DesactivateNotification();
        _roomNotification = null;

        _currentChrono = 0;
        CurrentObjectManufactured = null;
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
                CurrentInternalCapacity = AssemblyRoomData.InternalStorageAtLvl1;
                break;
            case 2:
                _currentChrono = 0;
                CurrentInternalCapacity = AssemblyRoomData.InternalStorageAtLvl2;
                break;
            case 3:
                _currentChrono = 0;
                CurrentInternalCapacity = AssemblyRoomData.InternalStorageAtLvl3;
                break;
        }
    }
}