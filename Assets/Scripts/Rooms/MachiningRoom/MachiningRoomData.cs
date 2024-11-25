using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new machining room data")]
public class MachiningRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Components which can be manufactured at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Components which can be manufactured at the start of the game.")]
    private List<ComponentData> _manufacturableComponentsByDefault;

    /// <summary>
    /// Gets components which can be manufactured at the start of the game.
    /// </summary>
    public List<ComponentData> ManufacturableComponentsByDefault { get { return _manufacturableComponentsByDefault; } private set { } }
}