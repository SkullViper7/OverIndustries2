using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;

    /// <summary>
    /// Main UI of the game.
    /// </summary>
    [field: SerializeField, Header("Main"), Tooltip("Main UI of the game.")]
    public GameObject HUD { get; private set; }

    /// <summary>
    /// Button group for interactions.
    /// </summary>
    [field: SerializeField, Tooltip("Button group for interactions.")]
    public InteractionButtonGroup InteractionButtonGroup { get; private set; }


    /// <summary>
    /// Global volume to do a blur effect in background when UI is open.
    /// </summary>
    [field: SerializeField, Tooltip("Global volume to do a blur effect in background when UI is open.")]
    public Volume Blur { get; private set; }

    private DepthOfField _depthOfField;

    /// <summary>
    /// Button to show informations about a room.
    /// </summary>
    [field: SerializeField, Space, Header("Interactions"), Tooltip("Button to show informations about a room.")]
    public GameObject InfoRoomButton { get; private set; }

    /// <summary>
    /// Button to upgrade a room.
    /// </summary>
    [field: SerializeField, Tooltip("Button to upgrade a room.")]
    public GameObject UpgradeButton { get; private set; }

    /// <summary>
    /// Button to start a production in a room.
    /// </summary>
    [field: SerializeField, Tooltip("Button to start a production in a room.")]
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
    public GameObject EmbaucheEmployeeButton { get; private set; }

    /// <summary>
    /// Button to show informations about a employee.
    /// </summary>
    [field: SerializeField, Tooltip("Button to show informations about a employee.")]
    public GameObject InfoEmployeeButton { get; private set; }

    /// <summary>
    /// The pop up which displays room infos.
    /// </summary>
    [field: SerializeField, Space, Tooltip("The pop up which displays room infos.")]
    public GameObject RoomInfoPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can launch a production.
    /// </summary>
    [field: SerializeField, Tooltip("The pop up where player can launch a production.")]
    public GameObject RoomProductionPopUp { get; private set; }

    /// <summary>
    /// The pop up where player can launch the choosen production.
    /// </summary>
    [field: SerializeField, Tooltip("The pop up where player can launch the choosen production.")]
    public GameObject ItemToProductPopUp { get; private set; }

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
    /// The UI on screen whene player can see the storage.
    /// </summary>
    [field: SerializeField, Tooltip("The UI on screen whene player can see the storage.")]
    public GameObject StoragePopUp { get; private set; }

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
    /// The button to cancel construction.
    /// </summary>
    [field: SerializeField, Tooltip("The button to cancel construction.")]
    public Button CancelConstructionButton { get; private set; }

    /// <summary>
    /// The UI on screen whene player pause the game.
    /// </summary>
    [field: SerializeField, Tooltip("The UI on screen whene player pause the game.")]
    public GameObject PauseUI { get; private set; }

    /// <summary>
    /// The UI on screen whene player want to skip the tutorial.
    /// </summary>
    [field: SerializeField, Tooltip("The UI on screen whene player want to skip the tutorial")]
    public GameObject SkipTutoUI { get; private set; }

    /// <summary>
    /// The UI on screen whene player finish the tutorial.
    /// </summary>
    [field: SerializeField, Tooltip("The UI on screen whene player finish the tutorial")]
    public GameObject ContinueAfterTutoUI { get; private set; }

    /// <summary>
    /// AudioSource to play SFX.
    /// </summary>
    [Space, Header("SFX"), SerializeField, Tooltip("AudioSource to play SFX.")]
    private AudioSource _SFXSource;

    /// <summary>
    /// SFX to play when a button is clicked.
    /// </summary>
    [SerializeField, Tooltip("SFX to play when a button is clicked.")]
    private AudioClip _clickSFX;

    /// <summary>
    /// SFX to play when a button to back from somewhere is clicked.
    /// </summary>
    [SerializeField, Tooltip("SFX to play when a button to back from somewhere is clicked.")]
    private AudioClip _backClickSFX;

    /// <summary>
    /// SFX to play when a UI is open.
    /// </summary>
    [SerializeField, Tooltip("SFX to play when a UI is open.")]
    private AudioClip _openSFX;

    /// <summary>
    /// SFX to play when a UI is open.
    /// </summary>
    [SerializeField, Tooltip("SFX to play when a UI is open.")]
    private AudioClip _closeSFX;

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

    private void Start()
    {
        if (Blur != null && Blur.profile.TryGet(out _depthOfField))
        {
            return;
        }
    }

    public void HideButtons()
    {
        InteractionButtonGroup.HideButtons();
        InfoRoomButton.SetActive(false);
        UpgradeButton.SetActive(false);
        ProductionButton.SetActive(false);
        StopProductionButton.SetActive(false);
        ResearchButton.SetActive(false);
        EmbaucheEmployeeButton.SetActive(false);
        InfoEmployeeButton.SetActive(false);
    }

    public void ClickSFX()
    {
        _SFXSource.PlayOneShot(_clickSFX);
    }

    public void BackClickSFX()
    {
        _SFXSource.PlayOneShot(_backClickSFX);
    }

    public void OpenSFX()
    {
        _SFXSource.PlayOneShot(_openSFX);
    }

    public void CloseSFX()
    {
        _SFXSource.PlayOneShot(_closeSFX);
    }

    /// <summary>
    /// Called when a UI is open.
    /// </summary>
    public void OpenUI()
    {
        GameManager.Instance.OpenUI();
        _depthOfField.aperture.value = 1;
    }

    /// <summary>
    /// Called when a UI is closed.
    /// </summary>
    public void CloseUI()
    {
        GameManager.Instance.CloseUI();
        _depthOfField.aperture.value = 32;
    }
}
