using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Create new Quest data")]
public class QuestData : ScriptableObject
{
    /// <summary>
    /// Name of the quest.
    /// </summary>
    [Header("Quest"), SerializeField, Tooltip("Name of the quest.")]
    private string _name;
    
    /// <summary>
    /// Description of the quest.
    /// </summary>
    [SerializeField, Tooltip("Description of the quest.")]
    private string _description;

    /// <summary>
    /// Object needed to completed this quest.
    /// </summary>
    [Space, Header("Stats"), SerializeField, Tooltip("Object needed to completed this quest.")]
    private List<ObjectType> _object;
    
    /// <summary>
    /// Number of object needed to completed this quest.
    /// </summary>
    [SerializeField, Tooltip("Number of object needed to completed this quest.")]
    private List<int> _numberOfObject;

    /// <summary>
    /// Number of PS add to player PS if quest are complited.    
    /// </summary>
    [Space, SerializeField, Tooltip("Number of PS add to player PS.")]
    private int _psWin;

    /// <summary>
    /// Gets the name of the quest.
    /// </summary>
    public string Name { get { return _name; } private set { } }
    
    /// <summary>
    /// Gets the description of the quest.
    /// </summary>
    public string Description { get { return _description; } private set { } }

    /// <summary>
    /// Gets object needed to complete this quest.
    /// </summary>
    public List<ObjectType> Object { get { return _object; } private set { } }

    /// <summary>
    /// Number of object needed to completed this quest.
    /// </summary>
    public List<int> NumberOfObject { get { return _numberOfObject; } private set { } }

    /// <summary>
    /// Number of PS add to player PS if quest are complited.    
    /// </summary>
    public int PSWin { get { return _psWin; } private set { } }
}
