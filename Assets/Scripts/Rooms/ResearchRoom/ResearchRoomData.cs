using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new research room data")]

public class ResearchRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// List wich stocks objectType with there time to unlock.
    /// </summary>
    [SerializeField, Tooltip("Object Type to research and here time to unlock.")]
    private List<ObjectData> _objectToUnlock = new();

    /// <summary>
    /// List wich stocks objectType with there time to unlock.
    /// </summary>
    public List<ObjectData> ObjectToUnlock { get { return _objectToUnlock; } private set { } }
}
