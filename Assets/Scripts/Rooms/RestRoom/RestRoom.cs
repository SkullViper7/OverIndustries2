using UnityEngine;

public class RestRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Data of the rest room.
    /// </summary>
    public RestRoomData RestRoomData { get; private set; }

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Called at the start to initialize the break room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        RestRoomData = (RestRoomData)roomBehaviourData;
        _roomMain = roomMain;

        _roomMain.NewLvl += UpgradeRoom;
    }

    /// <summary>
    /// Called to upgrad some values when the room is upgraded.
    /// </summary>
    /// <param name="newLvl"> New lvl of the room. </param>
    private void UpgradeRoom(int newLvl)
    {
        switch (newLvl)
        {
            case 1:
                EmployeeManager.Instance.AddCapacity(RestRoomData.CapacityBonusAtLvl1);
                break;
            case 2:
                EmployeeManager.Instance.AddCapacity(RestRoomData.CapacityBonusAtLvl2);
                break;
            case 3:
                EmployeeManager.Instance.AddCapacity(RestRoomData.CapacityBonusAtLvl3);
                break;
        }
    }
}
