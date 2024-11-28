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
    /// Event with positive action, number of PS and raw material win
    /// </summary>
    [Header("Positif"), SerializeField, Tooltip("Total points de satisfaction win when this event end")]
    private int _psWin;
    [SerializeField, Tooltip("Multiplicator value of PS win in this event")]
    private int _psMultiplicator;
    [SerializeField, Tooltip("Total raw material win when this event end")]
    private int _rawMaterialWin;

    /// <summary>
    /// Event with negative action, number of raw material lose
    /// </summary>
    [Header("Negatif"), SerializeField, Tooltip("Total raw material lost during this event")]
    private int _rawMaterialLost;

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
    public int PSMultiplicator { get { return _psMultiplicator; } }
    
    /// <summary>
    /// Get raw material win at event end
    /// </summary>
    public int RawMaterialWin { get { return _rawMaterialWin; } }

    /// <summary>
    /// Get raw material lost during this event
    /// </summary>
    public int RawMaterialLost { get { return _rawMaterialLost; } }
}
