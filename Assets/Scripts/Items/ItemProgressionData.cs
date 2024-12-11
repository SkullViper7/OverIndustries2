using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new item progression data")]
public class ItemProgressionData : ScriptableObject
{
    /// <summary>
    /// The list of components researchable by a research room at lvl 1.
    /// </summary>
    [Header("Components"), SerializeField, Tooltip("The list of components researchable by a research room at lvl 1.")]
    private List<ComponentData> _componentsResearchableAtLvl1 = new();

    /// <summary>
    /// The list of components researchable by a research room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("The list of components researchable by a research room at lvl 2.")]
    private List<ComponentData> _componentsResearchableAtLvl2 = new();

    /// <summary>
    /// The list of components researchable by a research room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("The list of components researchable by a research room at lvl 3.")]
    private List<ComponentData> _componentsResearchableAtLvl3 = new();

    /// <summary>
    /// Components which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Components which can be manufactured at the start of the game.")]
    private List<ComponentData> _manufacturableComponentsByDefault;

    /// <summary>
    /// The list of objects researchable by a research room at lvl 1.
    /// </summary>
    [Space, Header("Objects"), SerializeField, Tooltip("The list of objects researchable by a research room at lvl 1.")]
    private List<ObjectData> _objectsResearchableAtLvl1 = new();

    /// <summary>
    /// The list of objects researchable by a research room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("The list of objects researchable by a research room at lvl 2.")]
    private List<ObjectData> _objectsResearchableAtLvl2 = new();

    /// <summary>
    /// The list of objects researchable by a research room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("The list of objects researchable by a research room at lvl 3.")]
    private List<ObjectData> _objectsResearchableAtLvl3 = new();

    /// <summary>
    /// Objects which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Objects which can be manufactured at the start of the game.")]
    private List<ObjectData> _manufacturableObjectsByDefault;

    /// <summary>
    /// Gets the list of components researchable by a research room at lvl 1.
    /// </summary>
    public List<ComponentData> ComponentsResearchableAtLvl1 { get { return _componentsResearchableAtLvl1; } private set { } }

    /// <summary>
    /// Gets the list of components researchable by a research room at lvl 2.
    /// </summary>
    public List<ComponentData> ComponentsResearchableAtLvl2 { get { return _componentsResearchableAtLvl2; } private set { } }

    /// <summary>
    /// Gets the list of components researchable by a research room at lvl 3.
    /// </summary>
    public List<ComponentData> ComponentsResearchableAtLvl3 { get { return _componentsResearchableAtLvl3; } private set { } }

    /// <summary>
    /// Gets components which can be manufactured at the start of the game.
    /// </summary>
    public List<ComponentData> ManufacturableComponentsByDefault { get { return _manufacturableComponentsByDefault; } private set { } }

    /// <summary>
    /// Gets the list of objects researchable by a research room at lvl 1.
    /// </summary>
    public List<ObjectData> ObjectsResearchableAtLvl1 { get { return _objectsResearchableAtLvl1; } private set { } }

    /// <summary>
    /// Gets the list of objects researchable by a research room at lvl 2.
    /// </summary>
    public List<ObjectData> ObjectsResearchableAtLvl2 { get { return _objectsResearchableAtLvl2; } private set { } }

    /// <summary>
    /// Gets the list of objects researchable by a research room at lvl 3.
    /// </summary>
    public List<ObjectData> ObjectsResearchableAtLvl3 { get { return _objectsResearchableAtLvl3; } private set { } }

    /// <summary>
    /// Gets objects which can be manufactured at the start of the game.
    /// </summary>
    public List<ObjectData> ManufacturableObjectsByDefault { get { return _manufacturableObjectsByDefault; } private set { } }
}
