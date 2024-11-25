using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new recycling room data")]
public class RecyclingRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Dictionnary wich stocks components to recycle.
    /// </summary>
    [SerializeField, Tooltip("Components which can be recycle.")]
    private Dictionary<ComponentType, int> _componentToRecycle = new();

    /// <summary>
    /// Dictionnary wich stocks components to recycle.
    /// </summary>
    public Dictionary<ComponentType, int> ComponentToRecycle { get { return _componentToRecycle; } private set { } }
}
