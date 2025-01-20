using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new director room data")]
public class DirectorRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Capacity to add to the director room at lvl 1.
    /// </summary>
    [Header("Capacity bonus"), SerializeField, Tooltip("Capacity to add to the director room at lvl 1.")]
    private int _pourcentageHasTwoJobAtLvl1;

    /// <summary>
    /// Capacity to add to the director room at lvl 1.
    /// </summary>
    [SerializeField, Tooltip("Capacity to add to the director room at lvl 1.")]
    private int _pourcentageHasThreeJobAtLvl1;

    /// <summary>
    /// Capacity to add to the  director room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Capacity to add to the director room at lvl 2.")]
    private int _pourcentageHasTwoJobAtLvl2;

    /// <summary>
    /// Capacity to add to the  director room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Capacity to add to the director room at lvl 2.")]
    private int _pourcentageHasThreeJobAtLvl2;

    /// <summary>
    /// Capacity to add to the  director room at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Capacity to add to the director room at lvl 3.")]
    private int _pourcentageHasTwoJobAtLvl3;

    /// <summary>
    /// Capacity to add to the  director room at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Capacity to add to the director room at lvl 3.")]
    private int _pourcentageHasThreeJobAtLvl3;

    /// <summary>
    /// Employee prefab for create employee
    /// </summary>
    [SerializeField, Tooltip("employee prefab")]
    private GameObject _employee;
    
    /// <summary>
    /// Employee prefab for create employee
    /// </summary>
    [SerializeField, Tooltip("employee prefab")]
    private GameObject _maintenanceTechnician;

    /// <summary>
    /// Time to wait before create a new employee
    /// </summary>
    [SerializeField, Tooltip("Time to wait before create a new employee")]
    private int _waitTime;

    /// <summary>
    /// Capacity to add to the factory at lvl 1.
    /// </summary>
    [Space, Header("Employee capacity"), SerializeField, Tooltip("Capacity to add to the factory at lvl 1.")]
    private int _capacityBonusAtLvl1;

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 1.
    /// </summary>
    public int PourcentageHasTwoJobAtLvl1 { get { return _pourcentageHasTwoJobAtLvl1; } private set { } }
    
    /// <summary>
    /// Gets the capacity to add to the storage at lvl 1.
    /// </summary>
    public int PourcentageHasThreeJobAtLvl1 { get { return _pourcentageHasThreeJobAtLvl1; } private set { } }

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 2.
    /// </summary>
    public int PourcentageHasTwoJobAtLvl2 { get { return _pourcentageHasTwoJobAtLvl2; } private set { } }
   
    /// <summary>
    /// Gets the capacity to add to the storage at lvl 2.
    /// </summary>
    public int PourcentageHasThreeJobAtLvl2 { get { return _pourcentageHasThreeJobAtLvl2; } private set { } }

    /// <summary>
    /// Gets the capacity to add to the storage at lvl 3.
    /// </summary>
    public int PourcentageHasTwoJobAtLvl3 { get { return _pourcentageHasTwoJobAtLvl3; } private set { } }
    
    /// <summary>
    /// Gets the capacity to add to the storage at lvl 3.
    /// </summary>
    public int PourcentageHasThreeJobAtLvl3 { get { return _pourcentageHasThreeJobAtLvl3; } private set { } }

    /// <summary>
    /// Employee prefab for create employee
    /// </summary>
    public GameObject Employee { get { return _employee; } }
    
    /// <summary>
    /// Employee prefab for create employee
    /// </summary>
    public GameObject MaintenanceTechnician { get { return _maintenanceTechnician; } }

    /// <summary>
    /// Time to wait before create a new employee
    /// </summary>
    public int WaitTime { get { return _waitTime; } }


    /// <summary>
    /// Gets the capacity to add to the factory at lvl 1.
    /// </summary>
    public int CapacityBonusAtLvl1 { get { return _capacityBonusAtLvl1; } private set { } }
}
