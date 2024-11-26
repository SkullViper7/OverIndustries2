using System.Collections.Generic;
using UnityEngine;

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
    /// All components that the room can manufacture.
    /// </summary>
    private List<ComponentData> _manufacturableComponents = new();

    /// <summary>
    /// Current component that the room is manufacturing.
    /// </summary>
    private ComponentData _currentComponentManufactured;

    /// <summary>
    /// Current chrono of the production.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// A reference to the lambda who tries to launch a production each second.
    /// </summary>
    private ChronoManager.TickDelegate _productionOnHold;

    /// <summary>
    /// Called at the start to initialize the machining room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        MachiningRoomData = (MachiningRoomData)behaviourData;
        _itemStorage = ItemStorage.Instance;
        _rawMaterialStorage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

        // Init manufacturable components
        for (int i = 0; i < MachiningRoomData.ManufacturableComponentsByDefault.Count; i++)
        {
            _manufacturableComponents.Add(MachiningRoomData.ManufacturableComponentsByDefault[i]);
        }

        StartNewProduction(_manufacturableComponents[0]);
    }

    /// <summary>
    /// Called to start a new production.
    /// </summary>
    /// <param name="componentToProduct"> The component we want to product. </param>
    public void StartNewProduction(ComponentData componentToProduct)
    {
        // Reset production if there is already one
        if (_currentComponentManufactured != null || _currentChrono != 0)
        {
            StopProduction();
        }

        // If there is enough raw material in storage, launch production, else, checks each second if it is possible
        if (_rawMaterialStorage.ThereIsEnoughRawMaterialInStorage(componentToProduct.Cost))
        {
            if (_productionOnHold != null)
            {
                ChronoManager.Instance.NewSecondTick -= _productionOnHold;
                _productionOnHold = null;
            }

            _currentComponentManufactured = componentToProduct;
            ChronoManager.Instance.NewSecondTick += ProductionUpdateChrono;
        }
        else
        {
            if (_productionOnHold == null)
            {
                _productionOnHold = () => StartNewProduction(componentToProduct);
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
                if (_currentChrono + 1 >= _currentComponentManufactured.ProductionTimeAtLvl1)
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
                if (_currentChrono + 1 >= _currentComponentManufactured.ProductionTimeAtLvl2)
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
                if (_currentChrono + 1 >= _currentComponentManufactured.ProductionTimeAtLvl3)
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
        _currentComponentManufactured = null;
    }

    /// <summary>
    /// Called to add a component to the storage.
    /// </summary>
    private void ProductComponent()
    {
        if (!_itemStorage.WillExceedCapacity(1))
        {
            _itemStorage.AddComponents(_currentComponentManufactured.ComponentType, 1);
        }
    }


}