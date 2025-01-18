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
    /// A value indicating if a production cycle has started.
    /// </summary>
    public bool ProductionCycleHasStarted { get; private set; }

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
    /// A reference to the lambda who tries to launch a production if employees change or items storage changes.
    /// </summary>
    private Room.EmployeeDelegate _productionOnHoldCauseOfEmployee;
    private ItemStorage.ItemsDelegate _productionOnHoldCauseOfComponents;
    private AssemblyRoom.InternalStorageDelegate _productionOnHoldCauseOfInternalStorage;

    // Lock object shared between all instances
    private static readonly object _lockObject = new object();

    public event Action<int> NewChrono, NewProductionCount;

    public event Action ThereIsAnEmployee, ThereIsComponents, NoEmployeeInTheRoom, NoComponents;

    public delegate void InternalStorageDelegate();
    public event InternalStorageDelegate ProductionGet;

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

    public void TryStartProductionCycle(ObjectData objectToProduct)
    {
        // Synchronization between instances
        lock (_lockObject)
        {
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

            // Remove production on hold if there is one
            if (_productionOnHoldCauseOfInternalStorage != null)
            {
                this.ProductionGet -= _productionOnHoldCauseOfInternalStorage;
                _productionOnHoldCauseOfInternalStorage = null;
            }
            if (_productionOnHoldCauseOfComponents != null)
            {
                _itemStorage.ItemStorageHasChanged -= _productionOnHoldCauseOfComponents;
                _productionOnHoldCauseOfComponents = null;
            }
            if (_productionOnHoldCauseOfEmployee != null)
            {
                _roomMain.EmployeesHaveChanged -= _productionOnHoldCauseOfEmployee;
                _productionOnHoldCauseOfEmployee = null;
            }

            // Add listeners
            _productionOnHoldCauseOfInternalStorage = () => TryStartProductionCycle(CurrentObjectManufactured);
            this.ProductionGet += _productionOnHoldCauseOfInternalStorage;

            _productionOnHoldCauseOfComponents = () => TryStartProductionCycle(CurrentObjectManufactured);
            _itemStorage.ItemStorageHasChanged += _productionOnHoldCauseOfComponents;

            _productionOnHoldCauseOfEmployee = () => TryStartProductionCycle(CurrentObjectManufactured);
            _roomMain.EmployeesHaveChanged += _productionOnHoldCauseOfEmployee;

            // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
            if (_roomNotification == null)
            {
                _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
                _roomNotification.GetComponent<Button>().onClick.AddListener(AddObjectsInStorage);
            }

            if (_roomMain.EmployeeAssign.Count > 0)
            {
                // If there is the good employee in the room launch the production
                for (int i = 0; i < _roomMain.EmployeeAssign.Count; i++)
                {
                    if (_roomMain.EmployeeAssign[i].EmployeeJob[0].JobType == CurrentObjectManufactured.JobNeeded.JobType)
                    {
                        ThereIsAnEmployee?.Invoke();

                        // If there is enough components in storage, launch production, else, checks if there is a change
                        if (_itemStorage.ThereIsEnoughIngredientsInStorage(CurrentObjectManufactured.Ingredients) && CurrentAmountInInternalStorage < CurrentInternalCapacity)
                        {
                            ThereIsComponents?.Invoke();

                            if (!ProductionCycleHasStarted)
                            {
                                ProductionCycleHasStarted = true;

                                // Remove cost of the object from the item storage
                                _itemStorage.SubstractRecipe(CurrentObjectManufactured.Ingredients);

                                // Launch production
                                ChronoManager.Instance.NewSecondTick += ProductionUpdateChrono;
                            }

                            break;
                        }
                        else
                        {
                            NoComponents?.Invoke();

                            ProductionCycleHasStarted = false;
                        }
                    }
                    else
                    {
                        ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
                        _currentChrono = 0;

                        NoEmployeeInTheRoom?.Invoke();

                        ProductionCycleHasStarted = false;
                    }
                }
            }
            else
            {
                ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
                _currentChrono = 0;

                NoEmployeeInTheRoom?.Invoke();

                ProductionCycleHasStarted = false;
            }
        }
    }

    /// <summary>
    /// Called each second to update the production chrono.
    /// </summary>
    private void ProductionUpdateChrono()
    {
        if (CurrentAmountInInternalStorage < CurrentInternalCapacity)
        {
            if (_currentChrono + 1 >= CurrentProductionTime)
            {
                _currentChrono = 0;
                NewChrono?.Invoke(_currentChrono);
                AddObjectInInternalStorage();

                ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
                ProductionCycleHasStarted = false;
                TryStartProductionCycle(CurrentObjectManufactured);
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
            ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;
        }
    }

    /// <summary>
    /// Called at the end of a production to add an object in the internal storage.
    /// </summary>
    private void AddObjectInInternalStorage()
    {
        CurrentAmountInInternalStorage++;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
    }

    /// <summary>
    /// Called when objects are added to the storage to substracte it from the internal storage.
    /// </summary>
    /// <param name="amount"> Amount to substract from the internal storage. </param>
    private void RemoveObjectsFromInternalStorage(int amount)
    {
        CurrentAmountInInternalStorage -= amount;
        CurrentAmountInInternalStorage = Mathf.Clamp(CurrentAmountInInternalStorage, 0, CurrentInternalCapacity);
        NewProductionCount?.Invoke(CurrentAmountInInternalStorage);
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
                _itemStorage.AddObjects(CurrentObjectManufactured, CurrentAmountInInternalStorage);
                RemoveObjectsFromInternalStorage(CurrentAmountInInternalStorage);
                ProductionGet?.Invoke();
            }
            else
            {
                _itemStorage.AddObjects(CurrentObjectManufactured, remainingStorage);
                RemoveObjectsFromInternalStorage(remainingStorage);
                ProductionGet?.Invoke();
            }
        }
    }

    /// <summary>
    /// Called to stop the current production.
    /// </summary>
    public void StopProduction()
    {
        ChronoManager.Instance.NewSecondTick -= ProductionUpdateChrono;

        // Remove production on hold if there is one
        if (_productionOnHoldCauseOfEmployee != null)
        {
            _roomMain.EmployeesHaveChanged -= _productionOnHoldCauseOfEmployee;
            _productionOnHoldCauseOfEmployee = null;
        }

        if (_productionOnHoldCauseOfComponents != null)
        {
            _itemStorage.ItemStorageHasChanged -= _productionOnHoldCauseOfComponents;
            _productionOnHoldCauseOfComponents = null;
        }

        if (_productionOnHoldCauseOfInternalStorage != null)
        {
            this.ProductionGet -= _productionOnHoldCauseOfInternalStorage;
            _productionOnHoldCauseOfInternalStorage = null;
        }

        if (ProductionCycleHasStarted)
        {
            ProductionCycleHasStarted = false;

            // Refund player
            _itemStorage.AddRecipe(CurrentObjectManufactured.Ingredients);
        }

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
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}