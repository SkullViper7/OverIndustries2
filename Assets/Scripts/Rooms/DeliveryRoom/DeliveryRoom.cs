using UnityEngine;

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
    /// Called at the start to initialize the delivery room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        DeliveryRoomData = (DeliveryRoomData)behaviourData;
        _storage = RawMaterialStorage.Instance;
        _roomMain = roomMain;

        IncreaseStorageCapacity(1);

        ChronoManager.Instance.NewSecondTick += DeliveryUpdateChrono;
        _roomMain.NewLvl += IncreaseStorageCapacity;
    }

    /// <summary>
    /// Called each second to update the delivery chrono.
    /// </summary>
    private void DeliveryUpdateChrono()
    {
        switch (_roomMain.CurrentLvl)
        {
            case 1:
                if (_currentChrono + 1 >= DeliveryRoomData.DeliveryTimeAtLvl1)
                {
                    _currentChrono = 0;
                    ProductRawMaterials(DeliveryRoomData.ProductionPerDeliveryAtLvl1);
                }
                else
                {
                    _currentChrono++;
                }
                break;

            case 2:
                if (_currentChrono + 1 >= DeliveryRoomData.DeliveryTimeAtLvl2)
                {
                    _currentChrono = 0;
                    ProductRawMaterials(DeliveryRoomData.ProductionPerDeliveryAtLvl2);
                }
                else
                {
                    _currentChrono++;
                }
                break;

            case 3:
                if (_currentChrono + 1 >= DeliveryRoomData.DeliveryTimeAtLvl3)
                {
                    _currentChrono = 0;
                    ProductRawMaterials(DeliveryRoomData.ProductionPerDeliveryAtLvl3);
                }
                else
                {
                    _currentChrono++;
                }
                break;
        }
    }

    /// <summary>
    /// Called to add raw materials to the storage.
    /// </summary>
    /// <param name="amountToProduct"> Amount to add. </param>
    private void ProductRawMaterials(int amountToProduct)
    {
        _storage.AddRawMaterials(amountToProduct);
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
                _storage.IncreaseCapacity(DeliveryRoomData.CapacityBonusAtLvl1);
                break;
            case 2:
                _storage.IncreaseCapacity(DeliveryRoomData.CapacityBonusAtLvl2);
                break;
            case 3:
                _storage.IncreaseCapacity(DeliveryRoomData.CapacityBonusAtLvl3);
                break;
        }
    }
}