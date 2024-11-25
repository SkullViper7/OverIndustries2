using UnityEngine;

[CreateAssetMenu(fileName = "Component", menuName = "Component/Create new component")]
public class ComponentData : ScriptableObject
{
    /// <summary>
    /// Type of the component.
    /// </summary>
    [Header("Component"), SerializeField, Tooltip("Type of the component.")]
    private ComponentType _componentType;

    /// <summary>
    /// Name of the component.
    /// </summary>
    [SerializeField, Tooltip("Name of the component.")]
    private string _name;

    /// <summary>
    /// Cost of the component (in raw material).
    /// </summary>
    [Space, Header("Stats"), SerializeField, Tooltip("Cost of the component (in raw material).")]
    private int _cost;

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
}
