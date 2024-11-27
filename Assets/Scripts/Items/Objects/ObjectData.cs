using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Object", menuName = "Object/Create new object data")]
public class ObjectData : ScriptableObject
{
    /// <summary>
    /// Pictogram of the object.
    /// </summary>
    [SerializeField, Tooltip("Pictogram of the object.")]
    private Sprite _objectPicto;

    /// <summary>
    /// Type of the object.
    /// </summary>
    [Header("Object"), SerializeField, Tooltip("Type of the object.")]
    private ObjectType _objectType;

    /// <summary>
    /// Name of the object.
    /// </summary>
    [SerializeField, Tooltip("Name of the object.")]
    private string _name;

    /// <summary>
    /// Ingredients needed to product this object.
    /// </summary>
    [Space, Header("Stats"), SerializeField, Tooltip("Ingredients needed to product this object.")]
    private List<Ingredient> _ingredients;

    /// <summary>
    /// Time to unlock in the research room.
    /// </summary>
    [SerializeField, Tooltip("Time to unlock in the research room.")]
    private int _timeToUnlock;

    /// <summary>
    /// Cost in raw material to unlock in the research room.
    /// </summary>
    [SerializeField, Tooltip("Cost in raw material to unlock in the research room.")]
    private int _costToUnlock;

    /// <summary>
    /// Time to product the object (in seconds) at lvl 1.
    /// </summary>
    [SerializeField, Tooltip("Time to product the object (in seconds) at lvl 1.")]
    private int _productionTimeAtLvl1;

    /// <summary>
    /// Time to product the object (in seconds) at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Time to product the object (in seconds) at lvl 2.")]
    private int _productionTimeAtLvl2;

    /// <summary>
    /// Time to product the object (in seconds) at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Time to product the object (in seconds) at lvl 3.")]
    private int _productionTimeAtLvl3;

    /// <summary>
    /// Gets the pictogram of the object.
    /// </summary>
    public Sprite ObjectPicto { get { return _objectPicto; } private set { } }

    /// <summary>
    /// Gets the type of the object.
    /// </summary>
    public ObjectType ObjectType { get { return _objectType; } private set { } }

    /// <summary>
    /// Gets the name of the object.
    /// </summary>
    public string Name { get { return _name; } private set { } }

    /// <summary>
    /// Gets ingredients needed to product this object.
    /// </summary>
    public List<Ingredient> Ingredients { get { return _ingredients; } private set { } }

    /// <summary>
    /// Time to unlock in the research room.
    /// </summary>
    public int TimeToUnlock { get { return _timeToUnlock; } private set { } }

    /// <summary>
    /// Cost in raw material to unlock in the research room.
    /// </summary>
    public int CostToUnlock { get { return _costToUnlock; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 1.
    /// </summary>
    public int ProductionTimeAtLvl1 { get { return _productionTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 2.
    /// </summary>
    public int ProductionTimeAtLvl2 { get { return _productionTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 3.
    /// </summary>
    public int ProductionTimeAtLvl3 { get { return _productionTimeAtLvl3; } private set { } }
}
