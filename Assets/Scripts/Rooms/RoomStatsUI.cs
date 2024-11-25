using UnityEngine;

public class RoomStatsUI : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _roomStatsUI;

    public void Interaction()
    {
        if (_roomStatsUI.activeInHierarchy) _roomStatsUI.SetActive(false);
        else _roomStatsUI.SetActive(true);
    }
}
