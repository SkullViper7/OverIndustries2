using TMPro;
using UnityEngine;

public class AssignEmployeeWarningUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject _popUp;

    [SerializeField] 
    private TextMeshProUGUI _warningText;

    [SerializeField, TextArea] 
    private string _maxEmployeeNotAssign;

    [SerializeField, TextArea] 
    private string _assignRoomIsFull;

    [SerializeField, TextArea]
    private string _elevatorTxt;

    private void Awake()
    {
        _popUp.SetActive(false);
    }

    public void ShowWarningMessage(Room room)
    {
        if(room.RoomData.RoomType == RoomType.Director) 
        {
            _warningText.text = _maxEmployeeNotAssign;
        }
        else if (room.RoomData.RoomType == RoomType.Elevator)
        {
            _warningText.text = _elevatorTxt;
        }
        else
        {
            _warningText.text = _assignRoomIsFull;
        }

        _popUp.SetActive(true);
    }

    private void OnDisable()
    {
        _popUp.SetActive(false);
    }
}
