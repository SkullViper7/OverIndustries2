using System.Collections.Generic;
using UnityEngine;

public class AssemblyRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the assembly room.
    /// </summary>
    [field: SerializeField]
    public AssemblyRoomData AssemblyRoomData { get; private set; }

    /// <summary>
    /// A reference to the item storage.
    /// </summary>
    private ItemStorage _itemStorage;

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private RoomTemp _roomMain;

    /// <summary>
    /// All objects that the room can assembled.
    /// </summary>
    public List<ObjectData> AssemblableObjects { get; private set; } = new();

    /// <summary>
    /// Current object that the room is assembling.
    /// </summary>
    private ObjectData _currentObjectAssembled;

    /// <summary>
    /// Current chrono of the production.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// A reference to the lambda who tries to launch a production each second.
    /// </summary>
    private ChronoManager.TickDelegate _productionOnHold;

    /// <summary>
    /// Called at the start to initialize the assembling room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, RoomTemp roomMain)
    {
        AssemblyRoomData = (AssemblyRoomData)behaviourData;
        _itemStorage = ItemStorage.Instance;
        _roomMain = roomMain;

        // Init manufacturable components
        for (int i = 0; i < AssemblyRoomData.AssemblableObjectsByDefault.Count; i++)
        {
            AssemblableObjects.Add(AssemblyRoomData.AssemblableObjectsByDefault[i]);
        }

        StartNewProduction(AssemblableObjects[0]);
    }

    /// <summary>
    /// Called to start a new production.
    /// </summary>
    /// <param name="objectToProduct"> The object we want to product. </param>
    public void StartNewProduction(ObjectData objectToProduct)
    {
        // Reset production if there is already one
        if (_currentObjectAssembled != null || _currentChrono != 0)
        {
            StopProduction();
        }

        // If there is enough components in storage, launch production, else, checks each second if it is possible
        if (_itemStorage.ThereIsEnoughIngredientsInStorage(objectToProduct.Ingredients))
        {
            if (_productionOnHold != null)
            {
                ChronoManager.Instance.NewSecondTick -= _productionOnHold;
                _productionOnHold = null;
            }

            _currentObjectAssembled = objectToProduct;
            ChronoManager.Instance.NewSecondTick += ProductionUpdateChrono;
        }
        else
        {
            if (_productionOnHold == null)
            {
                _productionOnHold = () => StartNewProduction(objectToProduct);
                ChronoManager.Instance.NewSecondTick += _productionOnHold;
            }
        }
    }

    /// <summary>
    /// Called each second to update the production chrono.
    /// </summary>
    private void ProductionUpdateChrono()
    {
        switch (_roomMain.CurrentLvl)
        {
            case 1:
                if (_currentChrono + 1 >= _currentObjectAssembled.ProductionTimeAtLvl1)
                {
                    _currentChrono = 0;
                    ProductComponent();
                }
                else
                {
                    _currentChrono++;
                }
                break;

            case 2:
                if (_currentChrono + 1 >= _currentObjectAssembled.ProductionTimeAtLvl2)
                {
                    _currentChrono = 0;
                    ProductComponent();
                }
                else
                {
                    _currentChrono++;
                }
                break;

            case 3:
                if (_currentChrono + 1 >= _currentObjectAssembled.ProductionTimeAtLvl3)
                {
                    _currentChrono = 0;
                    ProductComponent();
                }
                else
                {
                    _currentChrono++;
                }
                break;
        }
    }

    /// <summary>
    /// Called to stop the current production.
    /// </summary>
    public void StopProduction()
    {
        _currentChrono = 0;
        _currentObjectAssembled = null;
    }

    /// <summary>
    /// Called to add a object to the storage.
    /// </summary>
    private void ProductComponent()
    {
        if (!_itemStorage.WillExceedCapacity(1))
        {
            _itemStorage.AddObjects(_currentObjectAssembled.ObjectType, 1);
        }
    }
}
