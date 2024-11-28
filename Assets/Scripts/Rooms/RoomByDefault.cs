using UnityEngine;

public class RoomByDefault : MonoBehaviour
{
    /// <summary>
    /// Data of the room.
    /// </summary>
    [field : SerializeField]
    public RoomData RoomData { get; private set; }

    /// <summary>
    /// Data of the room behaviour.
    /// </summary>
    [field : SerializeField, InterfaceType(typeof(IRoomBehaviourData))]
    public ScriptableObject RoomBehaviourData { get; private set; }

    /// <summary>
    /// Position of the room in the grid.
    /// </summary>
    [field : SerializeField]
    public Vector2 RoomPosition { get; private set; }
}
