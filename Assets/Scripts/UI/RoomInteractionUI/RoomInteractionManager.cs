using UnityEngine;

public class RoomInteractionManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the interaction manager.
    /// </summary>
    private InteractionManager _interactionManager;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    private UIManager _uiManager;

    private void Start()
    {
        _interactionManager = InteractionManager.Instance;
        _uiManager = UIManager.Instance;

        _interactionManager.RoomInteraction += ShowButtons;
        _interactionManager.NoInteraction += DesactivateAllButtons;
    }

    /// <summary>
    /// Called to show some buttons when an interaction on a room is triggered.
    /// </summary>
    /// <param name="roomMain"> Main component of the room. </param>
    private void ShowButtons(Room roomMain)
    {
        switch (roomMain.RoomData.RoomType)
        {
            case RoomType.Elevator:
            case RoomType.Recycling:
                DesactivateAllButtons();
                _uiManager.InfoRoomButton.SetActive(true);
                break;

            case RoomType.Delivery:
            case RoomType.Storage:
            case RoomType.Rest:
            case RoomType.Director:
                DesactivateAllButtons();
                _uiManager.InfoRoomButton.SetActive(true);

                // Show upgrade button only if the room is not at max lvl
                if(roomMain.CurrentLvl < 3)
                {
                    _uiManager.UpgradeButton.SetActive(true);
                }
                break;

            case RoomType.Machining:
            case RoomType.Assembly:
            case RoomType.Research:
                DesactivateAllButtons();
                _uiManager.InfoRoomButton.SetActive(true);

                // Show upgrade button only if the room is not at max lvl
                if (roomMain.CurrentLvl < 3)
                {
                    _uiManager.UpgradeButton.SetActive(true);
                }

                _uiManager.ProductionButton.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Called to desactivate all buttons.
    /// </summary>
    private void DesactivateAllButtons()
    {
        _uiManager.InfoRoomButton.SetActive(false);
        _uiManager.UpgradeButton.SetActive(false);
        _uiManager.ProductionButton.SetActive(false);
    }
}
