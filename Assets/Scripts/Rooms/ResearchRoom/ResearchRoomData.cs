using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new research room data")]

public class ResearchRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// The job needed to make this room work.
    /// </summary>
    [SerializeField, Tooltip("The job needed to make this room work.")]
    private JobData _jobNeeded;

    /// <summary>
    /// Gets the job needed to make this room work.
    /// </summary>
    public JobData JobNeeded { get { return _jobNeeded; } private set { } }
}
