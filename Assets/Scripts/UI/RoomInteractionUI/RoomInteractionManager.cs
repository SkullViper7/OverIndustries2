using UnityEngine;
using UnityEngine.UI;

public class RoomInteractionManager : MonoBehaviour
{
    /// <summary>
    /// Button to show informations about a room.
    /// </summary>
    [SerializeField]
    private Button _infoButton;

    /// <summary>
    /// Button to upgrade a room.
    /// </summary>
    [SerializeField]
    private Button _upgradeButton;

    /// <summary>
    /// Button to start a production in a room.
    /// </summary>
    [SerializeField]
    private Button _productionButton;

    /// <summary>
    /// A reference to the interaction manager.
    /// </summary>
    private InteractionManager _interactionManager;

    private void Start()
    {
        _interactionManager = InteractionManager.Instance;

        _interactionManager.RoomInteraction += ShowButtons;
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
                _infoButton.gameObject.SetActive(true);
                break;

            case RoomType.Delivery:
            case RoomType.Storage:
            case RoomType.Rest:
            case RoomType.Director:
                DesactivateAllButtons();
                _infoButton.gameObject.SetActive(true);
                _upgradeButton.gameObject.SetActive(true);
                break;

            case RoomType.Machining:
            case RoomType.Assembly:
            case RoomType.Research:
                DesactivateAllButtons();
                _infoButton.gameObject.SetActive(true);
                _upgradeButton.gameObject.SetActive(true);
                _productionButton.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Called to desactivate all buttons.
    /// </summary>
    private void DesactivateAllButtons()
    {
        _infoButton.GetComponent<RoomInfoButton>().DesactivateButton();
        _upgradeButton.gameObject.SetActive(false);
        _productionButton.gameObject.SetActive(false);
    }
}
