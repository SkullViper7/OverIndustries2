using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Create new item progression data")]
public class ItemProgressionData : ScriptableObject
{
    /// <summary>
    /// The list of components to unlock in the research room at lvl 1.
    /// </summary>
    [Header("Components"), SerializeField, Tooltip("The list of components to unlock in the research room at lvl 1.")]
    private List<ComponentData> _componentsToUnlockAtLvl1 = new();

    /// <summary>
    /// The list of components to unlock in the research room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("The list of components to unlock in the research room at lvl 2.")]
    private List<ComponentData> _componentsToUnlockAtLvl2 = new();

    /// <summary>
    /// The list of components to unlock in the research room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("The list of components to unlock in the research room at lvl 3.")]
    private List<ComponentData> _componentsToUnlockAtLvl3 = new();

    /// <summary>
    /// Components which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Components which can be manufactured at the start of the game.")]
    private List<ComponentData> _manufacturableComponentsByDefault;

    /// <summary>
    /// The list of objects to unlock in the research room at lvl 1.
    /// </summary>
    [Space, Header("Objects"), SerializeField, Tooltip("The list of objects to unlock in the research room at lvl 1.")]
    private List<ObjectData> _objectsToUnlockAtLvl1 = new();

    /// <summary>
    /// The list of objects to unlock in the research room at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("The list of objects to unlock in the research room at lvl 2.")]
    private List<ObjectData> _objectsToUnlockAtLvl2 = new();

    /// <summary>
    /// The list of objects to unlock in the research room at lvl 3.
    /// </summary>
    [SerializeField, Tooltip("The list of objects to unlock in the research room at lvl 3.")]
    private List<ObjectData> _objectsToUnlockAtLvl3 = new();

    /// <summary>
    /// Objects which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Objects which can be manufactured at the start of the game.")]
    private List<ObjectData> _manufacturableObjectsByDefault;

    /// <summary>
    /// Gets the list of components to unlock in the research room at lvl 1.
    /// </summary>
    public List<ComponentData> ComponentsToUnlockAtLvl1 { get { return _componentsToUnlockAtLvl1; } private set { } }

    /// <summary>
    /// Gets the list of components to unlock in the research room at lvl 2.
    /// </summary>
    public List<ComponentData> ComponentsToUnlockAtLvl2 { get { return _componentsToUnlockAtLvl2; } private set { } }

    /// <summary>
    /// Gets the list of components to unlock in the research room at lvl 3.
    /// </summary>
    public List<ComponentData> ComponentsToUnlockAtLvl3 { get { return _componentsToUnlockAtLvl3; } private set { } }

    /// <summary>
    /// Gets components which can be manufactured at the start of the game.
    /// </summary>
    public List<ComponentData> ManufacturableComponentsByDefault { get { return _manufacturableComponentsByDefault; } private set { } }

    /// <summary>
    /// Gets the list of objects to unlock in the research room at lvl 1.
    /// </summary>
    public List<ObjectData> ObjectsToUnlockAtLvl1 { get { return _objectsToUnlockAtLvl1; } private set { } }

    /// <summary>
    /// Gets the list of objects to unlock in the research room at lvl 2.
    /// </summary>
    public List<ObjectData> ObjectsToUnlockAtLvl2 { get { return _objectsToUnlockAtLvl2; } private set { } }

    /// <summary>
    /// Gets the list of objects to unlock in the research room at lvl 3.
    /// </summary>
    public List<ObjectData> ObjectsToUnlockAtLvl3 { get { return _objectsToUnlockAtLvl3; } private set { } }

    /// <summary>
    /// Gets objects which can be manufactured at the start of the game.
    /// </summary>
    public List<ObjectData> ManufacturableObjectsByDefault { get { return _manufacturableObjectsByDefault; } private set { } }
}
