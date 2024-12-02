using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableSpotsUI : MonoBehaviour
{
    /// <summary>
    /// HUD.
    /// </summary>
    [SerializeField]
    private GameObject _HUD;

    /// <summary>
    /// UI displayed when player is constructing.
    /// </summary>
    [SerializeField]
    private GameObject _constructionUI;

    /// <summary>
    /// Prefab of the button.
    /// </summary>
    [SerializeField]
    private GameObject _spotButton;

    /// <summary>
    /// Id of the pool where buttons are stocked.
    /// </summary>
    private int _buttonsPoolID;

    /// <summary>
    /// A list to stock buttons used.
    /// </summary>
    private List<GameObject> _spotButtons = new();

    private void Start()
    {
        // Create the pool for buttons
        _buttonsPoolID = ObjectPoolManager.Instance.NewObjectPool(_spotButton);

        GridManager.Instance.AvailableSpotsResult += ShowAvailableSpots;
    }

    /// <summary>
    /// Called to display buttons where player can construct a room.
    /// </summary>
    /// <param name="roomData"> Data of the room. </param>
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="availableSpots"> Position where the room is placable. </param>
    private void ShowAvailableSpots(RoomData roomData, IRoomBehaviourData roomBehaviourData, List<Vector2> availableSpots)
    {
        for (int i = 0; i < availableSpots.Count; i++)
        {
            // Get a button
            GameObject button = ObjectPoolManager.Instance.GetObjectInPool(_buttonsPoolID);
            _spotButtons.Add(button);
            button.GetComponent<RectTransform>().SetParent(transform);

            button.SetActive(true);
            button.GetComponent<AvailableSpotButton>().InitButton(roomData, roomBehaviourData, availableSpots[i], this);
        }
    }

    /// <summary>
    /// Called to close UI.
    /// </summary>
    public void CloseUI()
    {
        for (int i = 0; i < _spotButtons.Count; i++)
        {
            _spotButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            ObjectPoolManager.Instance.ReturnObjectToThePool(_buttonsPoolID, _spotButtons[i]);
            _spotButtons[i].SetActive(false);
        }

        _spotButtons.Clear();

        _constructionUI.SetActive(false);
        _HUD.SetActive(true);
    }
}
