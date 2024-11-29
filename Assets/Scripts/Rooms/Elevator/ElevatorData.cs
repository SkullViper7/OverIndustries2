using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new elevator data")]
public class ElevatorData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Prefab of an empty room.
    /// </summary>
    [SerializeField, Tooltip("Prefab of an empty room.")]
    private GameObject _emptyRoomPrefab;

    /// <summary>
    /// Gets the prefab of an empty room.
    /// </summary>
    public GameObject EmptyRoomPrefab { get { return _emptyRoomPrefab; } private set { } }
}
