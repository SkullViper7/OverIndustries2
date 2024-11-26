using UnityEngine;

public class RestRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Data of the Break room.
    /// </summary>
    [field: SerializeField]
    public RestRoomData RestRoomData { get; private set; }
    public Room _rooms;
    private Room _roomMain;

    void Start()
    {
        InitRoomBehaviour(RestRoomData, gameObject.GetComponent<Room>());
        _rooms = gameObject.GetComponent<Room>();
    }

    /// <summary>
    /// Called at the start to initialize the break room.
    /// </summary>
    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        RestRoomData = (RestRoomData)roomBehaviourData;
        _roomMain = roomMain;

        CheckAdjacentRoom();
    }

    private void CheckAdjacentRoom()
    {
        SpotChecker.Instance.CheckOccupiedSpots();

        if (SpotChecker.Instance.OccupiedSpots[(int)_rooms.Coordinates.x, (int)_rooms.Coordinates.y + _rooms.Size] && _rooms.Coordinates.y + _rooms.Size < SpotChecker.Instance.GridSize)
        {
            Debug.Log("Augmente la productivit� de la salle droite");
        }
        if (_rooms.Coordinates.y - _rooms.Size > 0 && SpotChecker.Instance.OccupiedSpots[(int)_rooms.Coordinates.x, (int)_rooms.Coordinates.y - _rooms.Size])
        {
            // augmente la productivit� de la salle
            Debug.Log("Augmente la productivit� de la salle gauche");
        }
    }

    public void UpdateRoomBehaviour()
    {
        switch (_roomMain.CurrentLvl)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
