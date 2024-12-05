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
    /// Preview of the room.
    /// </summary>
    [SerializeField, Tooltip("Preview of the room.")]
    private Sprite _roomPreview;

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
    /// Gets the preview of the room.
    /// </summary>
    public Sprite RoomPreview { get { return _roomPreview; } private set { } }

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
}
