using UnityEngine;

public class DoorCreator : MonoBehaviour
{
    [SerializeField] GameObject _smallDoor;
    [SerializeField] GameObject _mediumDoor;
    [SerializeField] GameObject _bigDoor;

    Room _room;

    Animator _animator;

    private void Start()
    {
        _room = GetComponent<Room>();
        _room.OnInitialized += () => CreateDoor(_room.RoomData);
    }

    public void CreateDoor(RoomData roomData)
    {
        GameObject doorPrefab = null;

        switch (roomData.Size)
        {
            case 1:
                doorPrefab = _smallDoor;
                break;
            case 2:
                doorPrefab = _mediumDoor;
                break;
            case 4:
                doorPrefab = _bigDoor;
                break;
            default:
                Debug.LogWarning("Invalid room size, no door created.");
                return;
        }

        if (doorPrefab != null)
        {
            Instantiate(doorPrefab, transform.position, Quaternion.identity, transform);
            _animator = doorPrefab.GetComponent<Animator>();
            _room.InitAnimator(_animator);
        }
    }
}
