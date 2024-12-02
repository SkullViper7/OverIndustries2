using UnityEngine;

public class Elevator : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Data of the elevator.
    /// </summary>
    public ElevatorData ElevatorData { get; private set; }

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        ElevatorData = (ElevatorData)roomBehaviourData;
        _roomMain = roomMain;
    }
}
