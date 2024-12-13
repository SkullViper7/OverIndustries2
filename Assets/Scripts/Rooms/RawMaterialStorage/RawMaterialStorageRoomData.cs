using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new raw material storage room data")]
public class RawMaterialStorageRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Picto of the raw material.
    /// </summary>
    [Header("Picto"), SerializeField, Tooltip("Picto of the raw material.")]
    private Sprite _rawMaterialPicto;

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
    /// Gets the picto of the raw material.
    /// </summary>
    public Sprite RawMaterialPicto { get { return _rawMaterialPicto; } private set { } }

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
}
