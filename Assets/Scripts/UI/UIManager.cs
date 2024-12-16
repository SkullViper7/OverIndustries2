using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Main UI of the game.
    /// </summary>
    [field : SerializeField, Header("Main"), Tooltip("Main UI of the game.")]
    public GameObject HUD { get; private set; }

    /// <summary>
    /// Button to show informations about a room.
    /// </summary>
    [field : SerializeField, Space, Header("Interactions"), Tooltip("Button to show informations about a room.")]
    public GameObject InfoRoomButton { get; private set; }
    
    /// <summary>
    /// Button to upgrade a room.
    /// </summary>
    [field : SerializeField, Tooltip("Button to upgrade a room.")]
    public GameObject UpgradeButton { get; private set; }

    /// <summary>
    /// Button to start a production in a room.
    /// </summary>
    [field : SerializeField, Tooltip("Button to start a production in a room.")]
    public GameObject ProductionButton { get; private set; }

    /// <summary>
    /// Button to stop a production in a room.
    /// </summary>
    [field: SerializeField, Tooltip("Button to stop a production in a room.")]
    public GameObject StopProductionButton { get; private set; }

    /// <summary>
    /// Button to start a research in a room.
    /// </summary>
    [field: SerializeField, Tooltip("Button to start a research in a room.")]
    public GameObject ResearchButton { get; private set; }
    
    /// <summary>
    /// Button to embauche employee in director room.
    /// </summary>
    [field: SerializeField, Tooltip(" Button to embauche employee in director room.")]
    public GameObject EmbaucheEmployee { get; private set; }

    /// <summary>
    /// Button to show informations about a employee.
    /// </summary>
    [field: SerializeField, Tooltip("Button to show informations about a employee.")]
    public GameObject InfoEmployeeButton { get; private set; }
    
    /// <summary>
    /// Button to move this employee.
    /// </summary>
    [field: SerializeField, Tooltip("Button to move this employee.")]
    public GameObject MoveEmployeeButton { get; private set; }
    
    /// <summary>
    /// The pop up which displays room infos.
    /// </summary>
    [field : SerializeField, Space, Tooltip("The pop up which displays room infos.")]
    public GameObject RoomInfoPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can launch a production.
    /// </summary>
    [field : SerializeField, Tooltip("The pop up where player can launch a production.")]
    public GameObject RoomProductionPopUp { get; private set; }

    /// <summary>
    /// Pop up to warn the player that he will loose ressources if he stops the production.
    /// </summary>
    [field: SerializeField, Tooltip("Pop up to warn the player that he will loose ressources if he stops the production.")]
    public GameObject StopProductionPopUp { get; private set; }

    /// <summary>
    /// Pop up to warn the player that he will loose ressources if he upgrade a delivery room.
    /// </summary>
    [field: SerializeField, Tooltip("Pop up to warn the player that he will loose ressources if he upgrade a delivery room.")]
    public GameObject DeliveryUpgradeWarningPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can choose which research to launch.
    /// </summary>
    [field: SerializeField, Tooltip("The pop up where player can choose which research to launch.")]
    public GameObject RoomResearchPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can launch the choosen research.
    /// </summary>
    [field: SerializeField, Tooltip("The pop up where player can launch the choosen research.")]
    public GameObject ItemToResearchPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can construct a new room.
    /// </summary>
    [field: SerializeField, Space, Header("Construction"), Tooltip("The pop up where player can construct a new room.")]
    public GameObject ConstructionPopUp { get; private set; }

    /// <summary>
    /// The UI on screen whene player is constructing a room.
    /// </summary>
    [field: SerializeField, Tooltip("The UI on screen whene player is constructing a room.")]
    public GameObject ConstructionUI { get; private set; }

    /// <summary>
    /// Called when a UI is open.
    /// </summary>
    public void OpenUI()
    {
        GameManager.Instance.OpenUI();
    }

    /// <summary>
    /// Called when a UI is closed.
    /// </summary>
    public void CloseUI()
    {
        GameManager.Instance.CloseUI();
    }
}
