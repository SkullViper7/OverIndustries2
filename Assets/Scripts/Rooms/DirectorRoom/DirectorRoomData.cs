using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new director room data")]
public class DirectorRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Capacity to add to the director room at lvl 1.
    /// </summary>
    [Header("Capacity bonus"), SerializeField, Tooltip("Capacity to add to the director room at lvl 1.")]
    private int _capacityBonusAtLvl1;

    /// <summary>
    /// Capacity to add to the  director room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Capacity to add to the  director room at lvl 2.")]
    private int _capacityBonusAtLvl2;

    /// <summary>
    /// Capacity to add to the  director room at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Capacity to add to the  director room at lvl 3.")]
    private int _capacityBonusAtLvl3;
}
