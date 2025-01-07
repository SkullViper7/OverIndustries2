using TMPro;
using UnityEngine;

public class AssignEmployeeWarningUI : MonoBehaviour
{
    [SerializeField] private GameObject _assignWarningUI;
    [SerializeField] private TextMeshProUGUI _warningText;

    [SerializeField] private string _maxEmployeeNotAssign;
    [SerializeField] private string _assignRoomIsFull;

    public void Start()
    {
        DragAndDrop.Instance.RoomAssignIsFull += ShowWarningMessage;
        DirectorRoom.Instance.MaxAssignEmployee += ShowWarningMessage;
    }

    public void ShowWarningMessage(Room room)
    {
        _assignWarningUI.SetActive(true);

        if(room.RoomData.RoomType == RoomType.Director) 
        {
            _warningText.text = _maxEmployeeNotAssign;
        }
        else
        {
            _warningText.text = _assignRoomIsFull;
        }
    }
}
