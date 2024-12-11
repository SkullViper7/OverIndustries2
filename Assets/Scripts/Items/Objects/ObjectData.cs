using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new object data")]
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
    [SerializeField, Tooltip("Type of the object.")]
    private ObjectType _objectType;

    /// <summary>
    /// Name of the object.
    /// </summary>
    [SerializeField, Tooltip("Name of the object.")]
    private string _name;

    /// <summary>
    /// Description of the object.
    /// </summary>
    [SerializeField, Tooltip("Description of the object.")]
    private string _description;

    /// <summary>
    /// Ingredients needed to product this object.
    /// </summary>
    [Space, Header("Production"), SerializeField, Tooltip("Ingredients needed to product this object.")]
    private List<Ingredient> _ingredients;

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
    /// Raw material and components needed to unlock this object in the research room.
    /// </summary>
    [Space, Header("Research"), SerializeField, Tooltip("Raw material and components needed to unlock this object in the research room.")]
    private ObjectResearchCost _researchCost;

    /// <summary>
    /// Time to unlock this object in the research room.
    /// </summary>
    [SerializeField, Tooltip("Time to unlock this object in the research room.")]
    private int _researchTime;

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
    /// Gets the description of the object.
    /// </summary>
    public string Description { get { return _description; } private set { } }

    /// <summary>
    /// Gets ingredients needed to product this object.
    /// </summary>
    public List<Ingredient> Ingredients { get { return _ingredients; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 1.
    /// </summary>
    public int ProductionTimeAtLvl1 { get { return _productionTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 2.
    /// </summary>
    public int ProductionTimeAtLvl2 { get { return _productionTimeAtLvl2; } private set { } }

    /// <summary>
    /// Gets the time to product the object for a room at lvl 3.
    /// </summary>
    public int ProductionTimeAtLvl3 { get { return _productionTimeAtLvl3; } private set { } }

    /// <summary>
    /// Gets raw material and components needed to unlock this object in the research room.
    /// </summary>
    public ObjectResearchCost ResearchCost { get { return _researchCost; } private set { } }

    /// <summary>
    /// Gets time to unlock this object in the research room.
    /// </summary>
    public int ResearchTime { get { return _researchTime; } private set { } }
}
