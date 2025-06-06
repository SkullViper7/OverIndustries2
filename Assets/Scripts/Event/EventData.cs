using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Create new event")]
public class EventData : ScriptableObject
{
    /// <summary>
    /// Profile of the event
    /// </summary>
    [Space, Header("Profile"), Tooltip("Profile with event information.")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    /// <summary>
    /// Duration of the event
    /// </summary>
    [Space, Header("Stats"), SerializeField, Tooltip("Duration of this event, in second.")]
    private int _duration;

    /// <summary>
    /// Event with positive action
    /// </summary>
    [Header("Positif Rewards"), SerializeField, Tooltip("Total points de satisfaction win when this event end")]
    private int _psWin;
    [SerializeField, Tooltip("Multiplicator value of PS win in this event")]
    private float _psMultiplicator;
    [SerializeField, Tooltip("Total raw material win when this event end")]
    private int _rawMaterialWin;
    [SerializeField, Tooltip("Multiplicator of raw material product during this event")]
    private float _rawMaterialMultiplicator;

    /// <summary>
    /// Event with negative action
    /// </summary>
    [Header("Negatif Rewards"), SerializeField, Tooltip("Total raw material lost when this event end")]
    private int _rawMaterialLost;
    [SerializeField, Tooltip("Divider of raw material product during this event")]
    private float _rawMaterialDivider;

    /// <summary>
    /// Condition need to start this event
    /// </summary>
    [Space, Header("Condition")]
    [SerializeField, Tooltip("This event have condition to start ?")]
    private bool _condition;
    [SerializeField, Tooltip("Number of Room need to start this event, if(condition)")] 
    private int _numberOfRoom;
    [SerializeField, Tooltip("Minimum level of room need to start this event, if(condition)")] 
    private int _minRoomLevel;
    [SerializeField, Tooltip("Minimum of point de satisfaction need to start this event, if(condition)")] 
    private int _minNumberPS;
    [SerializeField, Tooltip("This event need to create a quest, if(condition)")] 
    private bool _canCreateQuest ;
    [Space,SerializeField, Tooltip("This event need a special room type instanciates to start, if(condition)")] 
    private bool _specialRoomType;
    [SerializeField, Tooltip("Instantiates room type need to start this event, if(specialRoomType)")] 
    private RoomType _roomTypeNeed;

    /// <summary>
    /// Get event name
    /// </summary>
    public string Name { get { return _name; } }

    /// <summary>
    /// Get event description
    /// </summary>
    public string Description { get { return _description; } }

    /// <summary>
    /// Get event duration
    /// </summary>
    public int Duration { get { return _duration; } }

    /// <summary>
    /// Get event PS win at the event end
    /// </summary>
    public int PSWin { get { return _psWin; } }

    /// <summary>
    /// Get multiplicator value of PS win in this event
    /// </summary>
    public float PSMultiplicator { get { return _psMultiplicator; } }
    
    /// <summary>
    /// Get raw material win at event end
    /// </summary>
    public int RawMaterialWin { get { return _rawMaterialWin; } }

    /// <summary>
    /// Get multiplicator of raw material win during this event
    /// </summary>
    public float RawMaterialMultiplicator { get { return _rawMaterialMultiplicator; } }

    /// <summary>
    /// Get raw material lost at event end
    /// </summary>
    public int RawMaterialLost { get { return _rawMaterialLost; } }
    
    /// <summary>
    /// Get divider of raw material lost during this event
    /// </summary>
    public float RawMaterialDivider { get { return _rawMaterialDivider; } }
    
    /// <summary>
    /// Get if this event need condition to start
    /// </summary>
    public bool Condition { get { return _condition; } }

    /// <summary>
    /// Get the condition, number of room need to start this event
    /// </summary>
    public int NumberOfRoom { get { return _numberOfRoom; } }

    /// <summary>
    /// Get the condition, min room level need to start this event
    /// </summary>
    public int MinRoomLevel { get { return _minRoomLevel; } }

    /// <summary>
    /// Get the condition, min PS need to start this event
    /// </summary>
    public int MinNumberPS { get { return _minNumberPS; } }

    /// <summary>
    /// Get if this event need to create a quest
    /// </summary>
    public bool CanCreateQuest { get { return _canCreateQuest; } }
    
    /// <summary>
    /// Get if this event need a special room type instanciate to start
    /// </summary>
    public bool SpecialRoomType { get { return _specialRoomType; } }
    
    /// <summary>
    /// Get the condition, instantiates room type need to start this event
    /// </summary>
    public RoomType RoomTypeNeed { get { return _roomTypeNeed; } }
}