using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new delivery room data")]
public class DeliveryRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Capacity to add to the storage at lvl 1.
    /// </summary>
    [Header("Capacity bonus"), SerializeField, Tooltip("Capacity to add to the storage at lvl 1. (not the total)")]
    private int _capacityBonusAtLvl1;

    /// <summary>
    /// Capacity to add to the storage at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Capacity to add to the storage at lvl 2. (not the total)")]
    private int _capacityBonusAtLvl2;

    /// <summary>
    /// Capacity to add to the storage at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Capacity to add to the storage at lvl 3. (not the total)")]
    private int _capacityBonusAtLvl3;

    /// <summary>
    /// Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 1.
    /// </summary> 
    [Space, Header("Delivery Time"), SerializeField, Tooltip("Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 1.")]
    private int _deliveryTimeAtLvl1;

    /// <summary>
    /// Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 2.
    /// </summary> 
    [SerializeField, Tooltip("Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 2.")]
    private int _deliveryTimeAtLvl2;

    /// <summary>
    /// Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Time needed by the room to deliver an amount of raw materials (in seconds) at lvl 3.")]
    private int _deliveryTimeAtLvl3;

    /// <summary>
    /// Raw material producted in one delivery at lvl 1.
    /// </summary> 
    [Space, Header("Production per delivery"), SerializeField, Tooltip("Raw material producted in one delivery at lvl 1.")]
    private int _productionPerDeliveryAtLvl1;

    /// <summary>
    /// Raw material producted in one delivery at lvl 2.
    /// </summary> 
    [SerializeField, Tooltip("Raw material producted in one delivery at lvl 2.")]
    private int _productionPerDeliveryAtLvl2;

    /// <summary>
    /// Raw material producted in one delivery at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Raw material producted in one delivery at lvl 3.")]
    private int _productionPerDeliveryAtLvl3;

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 1.
    /// </summary>
    public int CapacityBonusAtLvl1 { get { return _capacityBonusAtLvl1; } private set { } }

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 2.
    /// </summary>
    public int CapacityBonusAtLvl2 { get { return _capacityBonusAtLvl2; } private set { } }

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 3.
    /// </summary>
    public int CapacityBonusAtLvl3 { get { return _capacityBonusAtLvl3; } private set { } }

    /// <summary>
    /// Gets the time needed by the room to deliver an amount of raw materials (in seconds) at lvl 1.
    /// </summary>
    public int DeliveryTimeAtLvl1 { get { return _deliveryTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time needed by the room to deliver an amount of raw materials (in seconds) at lvl 2.
    /// </summary>
    public int DeliveryTimeAtLvl2 { get { return _deliveryTimeAtLvl2; } private set { } }

    /// <summary>
    /// Gets the time needed by the room to deliver an amount of raw materials (in seconds) at lvl 3.
    /// </summary>
    public int DeliveryTimeAtLvl3 { get { return _deliveryTimeAtLvl3; } private set { } }

    /// <summary>
    /// Gets the raw material producted in one delivery at lvl 1.
    /// </summary>
    public int ProductionPerDeliveryAtLvl1 { get { return _productionPerDeliveryAtLvl1; } private set { } }

    /// <summary>
    /// Gets the raw material producted in one delivery at lvl 2.
    /// </summary>
    public int ProductionPerDeliveryAtLvl2 { get { return _productionPerDeliveryAtLvl2; } private set { } }

    /// <summary>
    /// Gets the raw material producted in one delivery at lvl 3.
    /// </summary>
    public int ProductionPerDeliveryAtLvl3 { get { return _productionPerDeliveryAtLvl3; } private set { } }
}
