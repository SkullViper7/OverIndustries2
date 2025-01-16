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

    [SerializeField]
    private Sprite _noRawMaterialPicto;

    [SerializeField]
    private Sprite _noComponentsPicto;

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

    private bool _isBlockedCauseOfInternalStorage = false;
    private bool _isBlockedCauseOfEmployee = false;
    private bool _isBlockedCauseOfRessources = false;

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

        deliveryRoom.ProductionStart += EmployeInTheDeliveryRoom;
        deliveryRoom.ProductionCantStart += NoEmployeeInTheRoom;

        rawMaterialStorage.CapacityHasChanged += CheckRawMaterialStorage;

        rawMaterialStorage.AmountHasChanged += CheckRawMaterialStorage;

        deliveryRoom.NewChrono += UpdateGaugeForDelivery;
        deliveryRoom.NewProductionCount += UpdateDeliveryCount;
    }

    private void EmployeInTheDeliveryRoom()
    {
        _isBlockedCauseOfEmployee = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateDeliveryCount(((DeliveryRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckRawMaterialStorage(0);
    }

    /// <summary>
    /// Called to change the color if the raw material storage is full or not.
    /// </summary>
    /// <param name="ignore"> Ignore this parameter. (put 0 in if you have to.) </param>
    private void CheckRawMaterialStorage(int ignore)
    {
        if (!_isBlockedCauseOfEmployee)
        {
            if (RawMaterialStorage.Instance.IsStorageFull())
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
    }

    /// <summary>
    /// Called to update the delivery count.
    /// </summary>
    /// <param name="newCount"> New count of the delivery. </param>
    private void UpdateDeliveryCount(int newCount)
    {
        if (newCount > 0)
        {
            _isBlockedCauseOfInternalStorage = true;
            _productionCountObj.SetActive(true);
            _notificationOutline.enabled = true;
            _notificationBG.enabled = true;
            CheckRawMaterialStorage(0);
        }
        else
        {
            _isBlockedCauseOfInternalStorage = false;
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

        _gaugePicto.sprite = machiningRoom.CurrentComponentManufactured.ComponentPicto;

        ItemStorage itemStorage = ItemStorage.Instance;

        machiningRoom.ThereIsAnEmployee += EmployeInTheMachiningRoom;
        machiningRoom.NoEmployeeInTheRoom += NoEmployeeInTheRoom;
        machiningRoom.ThereIsRawMaterial += RessourcesInTheMachiningRoom;
        machiningRoom.NoRawMaterial += NoRessourcesInTheMachiningRoom;

        itemStorage.CapacityHasChanged += CheckItemStorage;
        itemStorage.AmountHasChanged += CheckItemStorage;

        machiningRoom.NewChrono += UpdateGaugeForComponent;
        machiningRoom.NewProductionCount += UpdateProductionCount;
    }

    private void EmployeInTheMachiningRoom()
    {
        _isBlockedCauseOfEmployee = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateProductionCount(((MachiningRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckItemStorage(0);
    }

    private void RessourcesInTheMachiningRoom()
    {
        _isBlockedCauseOfRessources = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateProductionCount(((MachiningRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckItemStorage(0);
    }

    private void NoRessourcesInTheMachiningRoom()
    {
        _isBlockedCauseOfRessources = true;
        if (!_isBlockedCauseOfInternalStorage && !_isBlockedCauseOfEmployee)
        {
            _gaugeObject.SetActive(false);
            _gaugeFillAmount.fillAmount = 0f;

            _notificationPictoImg.sprite = _noRawMaterialPicto;
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

        _gaugePicto.sprite = assemblyRoom.CurrentObjectManufactured.ObjectPicto;

        ItemStorage itemStorage = ItemStorage.Instance;

        assemblyRoom.ThereIsAnEmployee += EmployeInTheAssemblyRoom;
        assemblyRoom.NoEmployeeInTheRoom += NoEmployeeInTheRoom;
        assemblyRoom.ThereIsComponents += RessourcesInTheAssemblyRoom;
        assemblyRoom.NoComponents += NoRessourcesInTheAssemblyRoom;

        itemStorage.CapacityHasChanged += CheckItemStorage;
        itemStorage.AmountHasChanged += CheckItemStorage;

        assemblyRoom.NewChrono += UpdateGaugeForObject;
        assemblyRoom.NewProductionCount += UpdateProductionCount;
    }

    private void EmployeInTheAssemblyRoom()
    {
        _isBlockedCauseOfEmployee = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateProductionCount(((MachiningRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckItemStorage(0);
    }

    private void RessourcesInTheAssemblyRoom()
    {
        _isBlockedCauseOfRessources = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);

        UpdateProductionCount(((MachiningRoom)_currentRoom.RoomBehaviour).CurrentAmountInInternalStorage);

        CheckItemStorage(0);
    }

    private void NoRessourcesInTheAssemblyRoom()
    {
        _isBlockedCauseOfRessources = true;
        if (!_isBlockedCauseOfInternalStorage && !_isBlockedCauseOfEmployee)
        {
            _gaugeObject.SetActive(false);
            _gaugeFillAmount.fillAmount = 0f;

            _notificationPictoImg.sprite = _noComponentsPicto;
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

        if (researchRoom.CurrentComponentResearched != null)
        {
            _gaugePicto.sprite = researchRoom.CurrentComponentResearched.ComponentPicto;
        }
        else if (researchRoom.CurrentObjectResearched != null)
        {
            _gaugePicto.sprite = researchRoom.CurrentObjectResearched.ObjectPicto;
        }
        _gaugeObject.SetActive(true);

        researchRoom.ResearchStart += EmployeInTheResearchRoom;
        researchRoom.ResearchCantStart += NoEmployeeInTheRoom;

        researchRoom.NewChrono += UpdateGaugeForResearch;
        researchRoom.ResearchCompleted += OnResearchCompleted;
    }

    private void EmployeInTheResearchRoom()
    {
        _isBlockedCauseOfEmployee = false;
        _notificationPictoObj.SetActive(false);
        _gaugeFillAmount.fillAmount = 0f;
        _gaugeObject.SetActive(true);
        _notificationOutline.enabled = false;
        _notificationBG.enabled = false;
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
        _isBlockedCauseOfInternalStorage = true;

        if (ColorUtility.TryParseHtmlString("#40C1AA", out Color newColor))
        {
            _notificationOutline.color = newColor;
            newColor = new Color(newColor.r, newColor.g, newColor.b, 60f / 255f);
            _notificationBG.raycastTarget = true;
            _notificationBG.color = newColor;
        }

        _notificationOutline.enabled = true;
        _notificationBG.enabled = true;
        _gaugeObject.SetActive(false);

        ResearchRoom researchRoom = (ResearchRoom)_currentRoom.RoomBehaviour;
        if (researchRoom.CurrentComponentResearched != null)
        {
            _notificationPictoImg.sprite = researchRoom.CurrentComponentResearched.ComponentPicto;
        }
        else if (researchRoom.CurrentObjectResearched != null)
        {
            _notificationPictoImg.sprite = researchRoom.CurrentObjectResearched.ObjectPicto;
        }
        _notificationPictoObj.SetActive(true);

        _button.interactable = true;

        UIManager.Instance.StopProductionButton.SetActive(false);
    }
    #endregion

    private void NoEmployeeInTheRoom()
    {
        _isBlockedCauseOfEmployee = true;
        if (!_isBlockedCauseOfInternalStorage)
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
    /// Called to update the production count.
    /// </summary>
    /// <param name="newCount"> New count of the production. </param>
    private void UpdateProductionCount(int newCount)
    {
        if (newCount > 0)
        {
            _isBlockedCauseOfInternalStorage = true;
            _productionCountObj.SetActive(true);
            _notificationOutline.enabled = true;
            _notificationBG.enabled = true;
            CheckItemStorage(0);
        }
        else
        {
            _isBlockedCauseOfInternalStorage = false;
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
        if (!_isBlockedCauseOfEmployee && !_isBlockedCauseOfRessources)
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
        else if (_isBlockedCauseOfInternalStorage)
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
        _isBlockedCauseOfInternalStorage = false;
        _isBlockedCauseOfEmployee = false;
        _isBlockedCauseOfRessources = false;

        // Remove all listeners
        switch (_currentRoom.RoomData.RoomType)
        {
            case RoomType.Delivery:
                DeliveryRoom deliveryRoom = (DeliveryRoom)_currentRoom.RoomBehaviour;
                _button.onClick.RemoveAllListeners();
                RawMaterialStorage.Instance.CapacityHasChanged -= CheckRawMaterialStorage;
                RawMaterialStorage.Instance.AmountHasChanged -= CheckRawMaterialStorage;

                deliveryRoom.ProductionStart -= EmployeInTheDeliveryRoom;
                deliveryRoom.ProductionCantStart -= NoEmployeeInTheRoom;

                deliveryRoom.NewChrono -= UpdateGaugeForDelivery;
                deliveryRoom.NewProductionCount -= UpdateDeliveryCount;
                break;

            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)_currentRoom.RoomBehaviour;
                _button.onClick.RemoveAllListeners();
                ItemStorage.Instance.CapacityHasChanged -= CheckItemStorage;
                ItemStorage.Instance.AmountHasChanged -= CheckItemStorage;

                machiningRoom.ThereIsAnEmployee -= EmployeInTheMachiningRoom;
                machiningRoom.NoEmployeeInTheRoom -= NoEmployeeInTheRoom;
                machiningRoom.ThereIsRawMaterial -= RessourcesInTheMachiningRoom;
                machiningRoom.NoRawMaterial -= NoRessourcesInTheMachiningRoom;

                machiningRoom.NewChrono -= UpdateGaugeForComponent;
                machiningRoom.NewProductionCount -= UpdateProductionCount;
                break;

            case RoomType.Assembly:
                AssemblyRoom assemblyRoom = (AssemblyRoom)_currentRoom.RoomBehaviour;
                _button.onClick.RemoveAllListeners();
                ItemStorage.Instance.CapacityHasChanged -= CheckItemStorage;
                ItemStorage.Instance.AmountHasChanged -= CheckItemStorage;

                assemblyRoom.ThereIsAnEmployee -= EmployeInTheAssemblyRoom;
                assemblyRoom.NoEmployeeInTheRoom -= NoEmployeeInTheRoom;
                assemblyRoom.ThereIsComponents -= RessourcesInTheAssemblyRoom;
                assemblyRoom.NoComponents -= NoRessourcesInTheAssemblyRoom;

                assemblyRoom.NewChrono -= UpdateGaugeForObject;
                assemblyRoom.NewProductionCount -= UpdateProductionCount;
                break;

            case RoomType.Research:
                ResearchRoom researchRoom = (ResearchRoom)_currentRoom.RoomBehaviour;
                _button.onClick.RemoveAllListeners();
                researchRoom.NewChrono -= UpdateGaugeForResearch;
                researchRoom.ResearchCompleted -= OnResearchCompleted;

                researchRoom.ResearchStart -= EmployeInTheResearchRoom;
                researchRoom.ResearchCantStart -= NoEmployeeInTheRoom;
                break;
        }

        // Remove the notification
        RoomNotificationManager.Instance.RemoveNotification(gameObject);
    }
}