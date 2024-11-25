using UnityEngine;

public class CreateNewRoom : MonoBehaviour
{
    [SerializeField] SpotChecker _spotChecker;
    GameObject _newRoom;

    public Vector2 CoordinatesNewRoom;
    public int SizeRoomPast;
    public bool RightRoom;

    public void CreateRoom(GameObject Room)
    {
        _newRoom = Instantiate(Room, new Vector3(gameObject.GetComponentInParent<Transform>().position.x, gameObject.GetComponentInParent<Transform>().position.y, 0), Quaternion.identity);
        if (RightRoom)
        {
            _newRoom.GetComponent<Room>().Coordinates = new Vector3(CoordinatesNewRoom.x, CoordinatesNewRoom.y + SizeRoomPast, 0);
        }
        else
        {
            _newRoom.GetComponent<Room>().Coordinates = new Vector3(CoordinatesNewRoom.x, CoordinatesNewRoom.y - _newRoom.GetComponent<Room>().Size, 0);
            Debug.Log(new Vector3(CoordinatesNewRoom.x, CoordinatesNewRoom.y - _newRoom.GetComponent<Room>().Size, 0));
        }

        _spotChecker.AddRoomToList(_newRoom.GetComponent<Room>());
    }
}