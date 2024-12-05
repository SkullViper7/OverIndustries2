using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new machining room data")]
public class MachiningRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Internal storage at lvl 1.
    /// </summary>
    [Space, Header("Internal storage"), SerializeField, Tooltip("Internal storage at lvl 1.")]
    private int _internalStorageAtLvl1;

    /// <summary>
    /// Internal storage at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Internal storage at lvl 2.")]
    private int _internalStorageAtLvl2;

    /// <summary>
    /// Internal storage at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Internal storage at lvl 3.")]
    private int _internalStorageAtLvl3;

    /// <summary>
    /// Components which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Components which can be manufactured at the start of the game.")]
    private List<ComponentData> _manufacturableComponentsByDefault;

    /// <summary>
    /// Gets the internal storage at lvl 1.
    /// </summary>
    public int InternalStorageAtLvl1 { get { return _internalStorageAtLvl1; } private set { } }

    /// <summary>
    /// Gets the internal storage at lvl 2.
    /// </summary>
    public int InternalStorageAtLvl2 { get { return _internalStorageAtLvl2; } private set { } }

    /// <summary>
    /// Gets the internal storage at lvl 3.
    /// </summary>
    public int InternalStorageAtLvl3 { get { return _internalStorageAtLvl3; } private set { } }

    /// <summary>
    /// Gets components which can be manufactured at the start of the game.
    /// </summary>
    public List<ComponentData> ManufacturableComponentsByDefault { get { return _manufacturableComponentsByDefault; } private set { } }
}