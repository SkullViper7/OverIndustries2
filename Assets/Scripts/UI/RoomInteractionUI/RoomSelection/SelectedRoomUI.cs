using UnityEngine;
using UnityEngine.UI;

public class SelectedRoomUI : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Image _image;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    private void Start()
    {
        InteractionManager.Instance.RoomSelected += ShowRoomSelected;
        InteractionManager.Instance.RoomUnselected += HideRoomSelected;
    }

    private void ShowRoomSelected(Room room)
    {
        Vector2 worldPos = GridManager.Instance.ConvertGridPosIntoWorldPos(room.RoomPosition);
        _rectTransform.position = new Vector3(worldPos.x, worldPos.y, -0.3f);
        _rectTransform.sizeDelta = new Vector2(room.RoomData.Size * 3, 4);
        _image.enabled = true;
    }

    private void HideRoomSelected()
    {
        _image.enabled = false;
        _rectTransform.position = new Vector3(0f, 0f, -0.3f);
        _rectTransform.sizeDelta = new Vector2(1f, 1f);
    }
}
