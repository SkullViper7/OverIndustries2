using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Room/Create new generic room data")]
public class RoomData : ScriptableObject
{
    /// <summary>
    /// Type of the room.
    /// </summary>
    [SerializeField, Tooltip("Type of the room.")]
    private RoomType _roomType;

    /// <summary>
    /// Name of the room.
    /// </summary>
    [SerializeField, Tooltip("Name of the room.")]
    private string _name;

    /// <summary>
    /// Preview of the room at lvl 1.
    /// </summary>
    [SerializeField, Tooltip("Preview of the room at lvl 1.")]
    private Sprite _roomLvl1Preview;

    /// <summary>
    /// Preview of the room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Preview of the room at lvl 2.")]
    private Sprite _roomLvl2Preview;

    /// <summary>
    /// Preview of the room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Preview of the room at lvl 3.")]
    private Sprite _roomLvl3Preview;

    /// <summary>
    /// Description of the room.
    /// </summary>
    [SerializeField, Tooltip("Description of the room."), TextArea]
    private string _description;

    /// <summary>
    /// Size of the room.
    /// </summary>
    [SerializeField, Tooltip("Size of the room.")]
    private int _size;

    /// <summary>
    /// Capacity of the room.
    /// </summary>
    [SerializeField, Tooltip("Capacity of the room.")]
    private int _capacity;

    /// <summary>
    /// Cost (in raw material) to build the room.
    /// </summary>
    [SerializeField, Tooltip("Cost (in raw material) to build the room.")]
    private int _constructionCost;

    /// <summary>
    /// Cost (in raw material) to upgrade the room to lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Cost (in raw material) to upgrade the room to lvl 2.")]
    private int _upgradeCostToLvl2;


    /// <summary>
    /// Cost (in raw material) to upgrade the room to lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Cost (in raw material) to upgrade the room to lvl 3.")]
    private int _upgradeCostToLvl3;

    /// <summary>
    /// Prefab of the room at lvl 1.
    /// </summary>
    [Space, SerializeField, Tooltip("Prefab of the room at lvl 1.")]
    private GameObject _roomLvl1;

    /// <summary>
    /// Prefab of the room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Prefab of the room at lvl 2.")]
    private GameObject _roomLvl2;

    /// <summary>
    /// Prefab of the room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Prefab of the room at lvl 3.")]
    private GameObject _roomLvl3;


    /// <summary>
    /// Gets the type of the room.
    /// </summary>
    public RoomType RoomType { get { return _roomType; } private set { } }

    /// <summary>
    /// Gets the name of the room.
    /// </summary>
    public string Name { get { return _name; } private set { } }

    /// <summary>
    /// Gets the preview of the room at lvl 1.
    /// </summary>
    public Sprite RoomLvl1Preview { get { return _roomLvl1Preview; } private set { } }

    /// <summary>
    /// Gets the preview of the room at lvl 2.
    /// </summary>
    public Sprite RoomLvl2Preview { get { return _roomLvl2Preview; } private set { } }

    /// <summary>
    /// Gets the preview of the room at lvl 3.
    /// </summary>
    public Sprite RoomLvl3Preview { get { return _roomLvl3Preview; } private set { } }

    /// <summary>
    /// Gets the description of the room.
    /// </summary>
    public string Description { get { return _description; } private set { } }

    /// <summary>
    /// Gets the name of the room.
    /// </summary>
    public int Size { get { return _size; } private set { } }

    /// <summary>
    /// Gets the capacity of the room.
    /// </summary>
    public int Capacity { get { return _capacity; } private set { } }

    /// <summary>
    /// Gets the cost (in raw material) to build the room.
    /// </summary>
    public int ConstructionCost { get { return _constructionCost; } private set { } }

    /// <summary>
    /// Gets the cost (in raw material) to upgrade the room to lvl 2.
    /// </summary>
    public int UpgradeCostToLvl2 { get { return _upgradeCostToLvl2; } private set { } }

    /// <summary>
    /// Gets the cost (in raw material) to upgrade the room to lvl 3.
    /// </summary>
    public int UpgradeCostToLvl3 { get { return _upgradeCostToLvl3; } private set { } }

    /// <summary>
    /// Gets the prefab of the room.
    /// </summary>
    public GameObject RoomLvl1 { get { return _roomLvl1; } private set { } }

    /// <summary>
    /// Gets the prefab of the room.
    /// </summary>
    public GameObject RoomLvl2 { get { return _roomLvl2; } private set { } }

    /// <summary>
    /// Gets the prefab of the room.
    /// </summary>
    public GameObject RoomLvl3 { get { return _roomLvl3; } private set { } }

    [SerializeField, Header("Director room only"), Tooltip("employee prefab")] public GameObject Employee;
}
