using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNotifiction : MonoBehaviour
{
    [Header("Notification"), SerializeField]
    private GameObject _notificationPictoObj;

    [SerializeField]
    private Image _notificationPictoImg;

    [SerializeField]
    private Sprite _rawMaterialPicto;

    [SerializeField]
    private Sprite _noEmployeePicto;

    /// <summary>
    /// Gauge to show when it's a notification for a production or a research.
    /// </summary>
    [Space, Header("Gauge"), SerializeField]
    private GameObject _gaugeObject;

    /// <summary>
    /// Gauge.
    /// </summary>
    [SerializeField]
    private Image _gaugeFillAmount;

    /// <summary>
    /// Picto of the gauge.
    /// </summary>
    [SerializeField]
    private Image _gaugePicto;

    /// <summary>
    /// Count of the production.
    /// </summary>
    [Space, Header("Production Count"), SerializeField]
    private GameObject _productionCountObj;

    [SerializeField]
    private TMP_Text _productionCountTxt;

    /// <summary>
    /// Image component of the notification.
    /// </summary>
    [Space, Header("Notification"), SerializeField]
    private Image _notificationOutline;

    /// <summary>
    /// Image component of the notification.
    /// </summary>
    [SerializeField]
    private Image _notificationBG;

    /// <summary>
    /// Button component of the notification.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Current room on which the notification is.
    /// </summary>
    private Room _currentRoom;

    private bool _isBlocked = false;

    private void Awake()
    {
        _button = GetComponent<Button>();

        // Desactivate notification components
        _notificationBG.enabled = false;
        _notificationBG.raycastTarget = true;
        _notificationOutline.enabled = false;
        _notificationPictoObj.SetActive(false);
        _gaugeObject.SetActive(false);
        _productionCountObj.SetActive(false);
        _button.interactable = false;
    }

    /// <summary>
    /// Called to initialize the notification.
    /// </summary>
    /// <param name="room"> Main component of the room who nee this notification. </param>
    public void InitNotification(Room room)
    {
        _currentRoom = room;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 position = GridManager.Instance.ConvertGridPosIntoWorldPos(_currentRoom.RoomPosition);
        rectTransform.position = new Vector3(position.x, position.y, -0.001f);
        rectTransform.sizeDelta = new Vector2(_currentRoom.RoomData.Size * 3, 4);

        switch (_currentRoom.RoomData.RoomType)
        {
            case RoomType.Delivery:
                InitForDeliveryRoom();
                break;
            case RoomType.Machining:
                InitForMachiningRoom();
                break;
            case RoomType.Assembly:
                InitForAssemblyRoom();
                break;
            case RoomType.Research:
                InitForResearchRoom();
                break;
        }
    }

    #region Delivery Room
    /// <summary>
    /// Called to initialize the notification especially for a delivery room.
    /// </summary>
    private void InitForDeliveryRoom()
    {
        DeliveryRoom deliveryRoom = (DeliveryRoom)_currentRoom.RoomBehaviour;

        _gaugePicto.sprite = _rawMaterialPicto;

        RawMaterialStorage rawMaterialStorage = RawMaterialStorage.Instance;

        _currentRoom.UpgradeStart += () => _gaugeObject.SetActive(false);
        _currentRoom.UpgradeEnd += () => _gaugeObject.SetActive(true);

        deliveryRoom.ProductionStart += EmployeInTheRoom;
        deliveryRoom.ProductionCantStart += NoEmployeeInTheRoom;

        rawMaterialStorage.CapacityHasChanged += CheckRawMaterialStorage;

        rawMaterialStorage.AmountHasChanged += CheckRawMaterialStorage;

        deliveryRoom.NewChrono += UpdateGaugeForDelivery;
        deliveryRoom.NewProductionCount += UpdateDeliveryCount;
    }

    private void EmployeInTheRoom()
    {
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateDeliveryCount(((DeliveryRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckRawMaterialStorage(0);
    }

    private void NoEmployeeInTheRoom()
    {
        if (!_isBlocked)
        {
            _gaugeObject.SetActive(false);
            _gaugeFillAmount.fillAmount = 0f;

            _notificationPictoImg.sprite = _noEmployeePicto;
            _notificationPictoObj.SetActive(true);

            if (ColorUtility.TryParseHtmlString("#F76A74", out Color newColor))
            {
                _notificationOutline.color = newColor;
                _notificationOutline.enabled = true;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
                _notificationBG.raycastTarget = false;
                _notificationBG.color = newColor;
                _notificationBG.enabled = true;
            }

            _button.interactable = false;
        }
        else
        {
            _gaugeFillAmount.fillAmount = 0f;
        }
    }

    /// <summary>
    /// Called to change the color if the raw material storage is full or not.
    /// </summary>
    /// <param name="ignore"> Ignore this parameter. (put 0 in if you have to.) </param>
    private void CheckRawMaterialStorage(int ignore)
    {
        if (RawMaterialStorage.Instance.IsStorageFull())
        {
            _isBlocked = true;
            if (ColorUtility.TryParseHtmlString("#F76A74", out Color newColor))
            {
                _notificationOutline.color = newColor;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
                _notificationBG.raycastTarget = false;
                _notificationBG.color = newColor;
            }

            _button.interactable = false;
        }
        else
        {
            _isBlocked = true;
            if (ColorUtility.TryParseHtmlString("#40C1AA", out Color newColor))
            {
                _notificationOutline.color = newColor;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
                _notificationBG.raycastTarget = true;
                _notificationBG.color = newColor;
            }

            _button.interactable = true;
        }
    }

    /// <summary>
    /// Called to update the delivery count.
    /// </summary>
    /// <param name="newCount"> New count of the delivery. </param>
    private void UpdateDeliveryCount(int newCount)
    {
        if (newCount > 0)
        {
            _productionCountObj.SetActive(true);
            _notificationOutline.enabled = true;
            _notificationBG.enabled = true;
            CheckRawMaterialStorage(0);
        }
        else
        {
            _isBlocked = false;
            _productionCountObj.SetActive(false);
            _notificationOutline.enabled = false;
            _notificationBG.enabled = false;
            _button.interactable = false;
        }

        _productionCountTxt.text = newCount.ToString();
    }

    /// <summary>
    /// Called to update the fill amount of the gauge for delivery.
    /// </summary>
    /// <param name="newChrono"> New chrono of the room. </param>
    private void UpdateGaugeForDelivery(int newChrono)
    {
        _gaugeFillAmount.fillAmount = 1f / ((DeliveryRoom)_currentRoom.RoomBehaviour).CurrentDeliveryTime * newChrono;
    }
    #endregion

    #region Machining Room
    /// <summary>
    /// Called to initialize the notification especially for a machining room.
    /// </summary>
    private void InitForMachiningRoom()
    {
        MachiningRoom machiningRoom = (MachiningRoom)_currentRoom.RoomBehaviour;

        _gaugeObject.SetActive(true);
        _gaugePicto.sprite = machiningRoom.CurrentComponentManufactured.ComponentPicto;
        _gaugeFillAmount.fillAmount = 0;

        _productionCountTxt.text = "";

        ItemStorage itemStorage = ItemStorage.Instance;

        // Check if item storage is already full to change color
        CheckItemStorage(0);

        itemStorage.CapacityHasChanged += CheckItemStorage;
        itemStorage.AmountHasChanged += CheckItemStorage;

        machiningRoom.NewChrono += UpdateGaugeForComponent;
        machiningRoom.NewProductionCount += UpdateProductionCount;
    }

    /// <summary>
    /// Called to update the fill amount of the gauge for component.
    /// </summary>
    /// <param name="newChrono"> New chrono of the room. </param>
    private void UpdateGaugeForComponent(int newChrono)
    {
        _gaugeFillAmount.fillAmount = 1f / ((MachiningRoom)_currentRoom.RoomBehaviour).CurrentProductionTime * newChrono;
    }
    #endregion

    #region Assembly Room
    /// <summary>
    /// Called to initialize the notification especially for an assembly room.
    /// </summary>
    private void InitForAssemblyRoom()
    {
        AssemblyRoom assemblyRoom = (AssemblyRoom)_currentRoom.RoomBehaviour;

        _gaugeObject.SetActive(true);
        _gaugePicto.sprite = assemblyRoom.CurrentObjectManufactured.ObjectPicto;
        _gaugeFillAmount.fillAmount = 0;

        _productionCountTxt.text = "";

        ItemStorage itemStorage = ItemStorage.Instance;

        // Check if item storage is already full to change color
        CheckItemStorage(0);

        itemStorage.CapacityHasChanged += CheckItemStorage;
        itemStorage.AmountHasChanged += CheckItemStorage;

        assemblyRoom.NewChrono += UpdateGaugeForObject;
        assemblyRoom.NewProductionCount += UpdateProductionCount;
    }

    /// <summary>
    /// Called to update the fill amount of the gauge for object.
    /// </summary>
    /// <param name="newChrono"> New chrono of the room. </param>
    private void UpdateGaugeForObject(int newChrono)
    {
        _gaugeFillAmount.fillAmount = 1f / ((AssemblyRoom)_currentRoom.RoomBehaviour).CurrentProductionTime * newChrono;
    }
    #endregion

    #region Research Room
    /// <summary>
    /// Called to initialize the notification especially for a research room.
    /// </summary>
    private void InitForResearchRoom()
    {
        ResearchRoom researchRoom = (ResearchRoom)_currentRoom.RoomBehaviour;

        _gaugeObject.SetActive(true);
        if (researchRoom.CurrentComponentResearched != null)
        {
            _gaugePicto.sprite = researchRoom.CurrentComponentResearched.ComponentPicto;
            _notificationPictoImg.sprite = researchRoom.CurrentComponentResearched.ComponentPicto;
        }
        else if (researchRoom.CurrentObjectResearched != null)
        {
            _gaugePicto.sprite = researchRoom.CurrentObjectResearched.ObjectPicto;
            _notificationPictoImg.sprite = researchRoom.CurrentObjectResearched.ObjectPicto;
        }

        _gaugeFillAmount.fillAmount = 0;

        researchRoom.NewChrono += UpdateGaugeForResearch;
        researchRoom.ResearchCompleted += OnResearchCompleted;
    }

    /// <summary>
    /// Called to update the fill amount of the gauge for a research.
    /// </summary>
    /// <param name="newChrono"> New chrono of the room. </param>
    private void UpdateGaugeForResearch(int newChrono)
    {
        _gaugeFillAmount.fillAmount = 1f / ((ResearchRoom)_currentRoom.RoomBehaviour).CurrentResearchTime * newChrono;
    }

    /// <summary>
    /// Called when the research is completed to notify the player.
    /// </summary>
    private void OnResearchCompleted()
    {
        if (ColorUtility.TryParseHtmlString("#40C1AA", out Color newColor))
        {
            _notificationOutline.color = newColor;
            newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
            _notificationBG.raycastTarget = true;
            _notificationBG.color = newColor;
        }
        _notificationBG.enabled = true;
        _gaugeObject.SetActive(false);
        _notificationPictoObj.SetActive(true);
        _button.interactable = true;

        UIManager.Instance.StopProductionButton.SetActive(false);
    }
    #endregion

    /// <summary>
    /// Called to update the production count.
    /// </summary>
    /// <param name="newCount"> New count of the production. </param>
    private void UpdateProductionCount(int newCount)
    {
        if (newCount > 0)
        {
            _productionCountObj.SetActive(true);
            _notificationOutline.enabled = true;
            _notificationBG.enabled = true;
            CheckItemStorage(0);
        }
        else
        {
            _productionCountObj.SetActive(false);
            _notificationOutline.enabled = false;
            _notificationBG.enabled = false;
            _button.interactable = false;
        }

        _productionCountTxt.text = newCount.ToString();
    }

    /// <summary>
    /// Called to change the color if the item storage is full or not.
    /// </summary>
    /// <param name="ignore"> Ignore this parameter. (put 0 in if you have to.) </param>
    private void CheckItemStorage(int ignore)
    {
        if (ItemStorage.Instance.IsStorageFull())
        {
            if (ColorUtility.TryParseHtmlString("#F76A74", out Color newColor))
            {
                _notificationOutline.color = newColor;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
                _notificationBG.raycastTarget = false;
                _notificationBG.color = newColor;
            }
            _button.interactable = false;
        }
        else
        {
            if (ColorUtility.TryParseHtmlString("#40C1AA", out Color newColor))
            {
                _notificationOutline.color = newColor;
                newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
                _notificationBG.raycastTarget = true;
                _notificationBG.color = newColor;
            }
            _button.interactable = true;
        }
    }

    /// <summary>
    /// Called to reset the notification.
    /// </summary>
    public void DesactivateNotification()
    {
        // Desactivate notification components
        _notificationBG.enabled = false;
        _notificationBG.raycastTarget = true;
        _notificationOutline.enabled = false;
        _notificationPictoObj.SetActive(false);
        _gaugeObject.SetActive(false);
        _productionCountObj.SetActive(false);
        _button.interactable = false;

        // Remove all listeners
        switch (_currentRoom.RoomData.RoomType)
        {
            case RoomType.Delivery:
                _button.onClick.RemoveAllListeners();
                RawMaterialStorage.Instance.CapacityHasChanged -= CheckRawMaterialStorage;
                RawMaterialStorage.Instance.AmountHasChanged -= CheckRawMaterialStorage;
                break;

            case RoomType.Machining:
                _button.onClick.RemoveAllListeners();
                ItemStorage.Instance.CapacityHasChanged -= CheckItemStorage;
                ItemStorage.Instance.AmountHasChanged -= CheckItemStorage;
                ((MachiningRoom)_currentRoom.RoomBehaviour).NewChrono -= UpdateGaugeForComponent;
                ((MachiningRoom)_currentRoom.RoomBehaviour).NewProductionCount -= UpdateProductionCount;
                break;

            case RoomType.Assembly:
                _button.onClick.RemoveAllListeners();
                ItemStorage.Instance.CapacityHasChanged -= CheckItemStorage;
                ItemStorage.Instance.AmountHasChanged -= CheckItemStorage;
                ((AssemblyRoom)_currentRoom.RoomBehaviour).NewChrono -= UpdateGaugeForObject;
                ((AssemblyRoom)_currentRoom.RoomBehaviour).NewProductionCount -= UpdateProductionCount;
                break;

            case RoomType.Research:
                _button.onClick.RemoveAllListeners();
                ((ResearchRoom)_currentRoom.RoomBehaviour).NewChrono -= UpdateGaugeForResearch;
                ((ResearchRoom)_currentRoom.RoomBehaviour).ResearchCompleted -= OnResearchCompleted;
                break;
        }

        // Remove the notification
        RoomNotificationManager.Instance.RemoveNotification(gameObject);
    }
}