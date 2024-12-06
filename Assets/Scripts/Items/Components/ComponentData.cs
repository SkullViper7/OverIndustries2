using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new component")]
public class ComponentData : ScriptableObject
{
    /// <summary>
    /// Pictogram of the component.
    /// </summary>
    [SerializeField, Tooltip("Pictogram of the component.")]
    private Sprite _componentPicto;

    /// <summary>
    /// Type of the component.
    /// </summary>
    [SerializeField, Tooltip("Type of the component.")]
    private ComponentType _componentType;

    /// <summary>
    /// Name of the component.
    /// </summary>
    [SerializeField, Tooltip("Name of the component.")]
    private string _name;

    /// <summary>
    /// Cost of the component (in raw material).
    /// </summary>
    [Space, Header("Production"), SerializeField, Tooltip("Cost of the component (in raw material).")]
    private int _cost;
    
    /// <summary>
    /// Trash of the component (in raw material).
    /// </summary>
    [Space, SerializeField, Tooltip("Trash of the component (in raw material).")]
    private int _trash;

    /// <summary>
    /// Time to product the component (in seconds) at lvl 1.
    /// </summary>
    [SerializeField, Tooltip("Time to product the component (in seconds) at lvl 1.")]
    private int _productionTimeAtLvl1;

    /// <summary>
    /// Time to product the component (in seconds) at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Time to product the component (in seconds) at lvl 2.")]
    private int _productionTimeAtLvl2;

    /// <summary>
    /// Time to product the component (in seconds) at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("Time to product the component (in seconds) at lvl 3.")]
    private int _productionTimeAtLvl3;

    /// <summary>
    /// Raw material needed to unlock this component in the research room.
    /// </summary>
    [Space, Header("Research"), SerializeField, Tooltip("Raw material needed to unlock this component in the research room.")]
    private int _researchCost;

    /// <summary>
    /// Time to unlock this component in the research room.
    /// </summary>
    [SerializeField, Tooltip("Time to unlock this component in the research room.")]
    private int _researchTime;

    /// <summary>
    /// Gets the pictogram of the component.
    /// </summary>
    public Sprite ComponentPicto { get { return _componentPicto; } private set { } }

    /// <summary>
    /// Gets the type of the component.
    /// </summary>
    public ComponentType ComponentType { get { return _componentType; } private set { } }

    /// <summary>
    /// Gets the name of the component.
    /// </summary>
    public string Name { get { return _name; } private set { } }

    /// <summary>
    /// Gets the cost of the component.
    /// </summary>
    public int Cost { get { return _cost; } private set { } }
    
    /// <summary>
    /// Gets the cost of the component.
    /// </summary>
    public int Trash { get { return _trash; } private set { } }

    /// <summary>
    /// Gets the time to product the component for a room at lvl 1.
    /// </summary>
    public int ProductionTimeAtLvl1 { get { return _productionTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time to product the component for a room at lvl 2.
    /// </summary>
    public int ProductionTimeAtLvl2 { get { return _productionTimeAtLvl1; } private set { } }

    /// <summary>
    /// Gets the time to product the component for a room at lvl 3.
    /// </summary>
    public int ProductionTimeAtLvl3 { get { return _productionTimeAtLvl3; } private set { } }

    /// <summary>
    /// Gets raw material needed to unlock this component in the research room.
    /// </summary>
    public int ResearchCost { get { return _researchCost; } private set { } }

    /// <summary>
    /// Gets time to unlock this component in the research room.
    /// </summary>
    public int ResearchTime { get { return _researchTime; } private set { } }
}
