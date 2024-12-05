using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNotifiction : MonoBehaviour
{
    /// <summary>
    /// Picto of the notification.
    /// </summary>
    [SerializeField]
    private Image _notificationPicto;

    /// <summary>
    /// Gauge to show when it's a notification for a production or a research.
    /// </summary>
    [SerializeField]
    private GameObject _gaugeObject;

    /// <summary>
    /// Gauge.
    /// </summary>
    [SerializeField]
    private Image _gauge;

    /// <summary>
    /// Picto of the gauge.
    /// </summary>
    [SerializeField]
    private Image _gaugePicto;

    /// <summary>
    /// Count of the production.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionCount;

    /// <summary>
    /// Image component of the notification.
    /// </summary>
    private Image _image;

    /// <summary>
    /// Button component of the notification.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Current room on which the notification is.
    /// </summary>
    private Room _currentRoom;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    /// <summary>
    /// Called to initialize the notification.
    /// </summary>
    /// <param name="room"> Main component of the room who nee this notification. </param>
    public void InitNotification(Room room)
    {
        _currentRoom = room;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = GridManager.Instance.ConvertGridPosIntoWorldPos(_currentRoom.RoomPosition);
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
                break;
            case RoomType.Research:
                break;
        }
    }

    #region Delivery Room
    /// <summary>
    /// Called to initialize the notification especially for a delivery room.
    /// </summary>
    private void InitForDeliveryRoom()
    {
        _image.enabled = true;
        _notificationPicto.gameObject.SetActive(true);
        _button.interactable = true;

        _notificationPicto.sprite = ((DeliveryRoomData)_currentRoom.RoomBehaviourData).RawMaterialPicto;

        RawMaterialStorage rawMaterialStorage = RawMaterialStorage.Instance;

        // Check if raw material storage is already full to change color
        CheckRawMaterialStorage(0);

        rawMaterialStorage.CapacityHasChanged += CheckRawMaterialStorage;

        rawMaterialStorage.AmountHasChanged += CheckRawMaterialStorage;
    }

    /// <summary>
    /// Called to change the color if the raw material storage is full or not.
    /// </summary>
    /// <param name="ignore"> Ignore this parameter. (put 0 in if you have to.) </param>
    private void CheckRawMaterialStorage(int ignore)
    {
        if (RawMaterialStorage.Instance.IsStorageFull())
        {
            _image.color = new Color32(255, 0, 0, 175);
            _button.interactable = false;
        }
        else
        {
            _image.color = new Color32(0, 255, 0, 175);
            _button.interactable = true;
        }
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
        _gauge.fillAmount = 0;
        _productionCount.text = "";

        ItemStorage itemStorage = ItemStorage.Instance;

        // Check if item storage is already full to change color
        CheckItemStorage(0);

        itemStorage.CapacityHasChanged += CheckItemStorage;

        itemStorage.AmountHasChanged += CheckItemStorage;

        machiningRoom.NewChrono += UpdateGauge;
        machiningRoom.NewProductionCount += UpdateProductionCount;
    }

    /// <summary>
    /// Called to update the fill amount of the gauge.
    /// </summary>
    /// <param name="newChrono"> New chrono of the room. </param>
    private void UpdateGauge(int newChrono)
    {
        _gauge.fillAmount = 1f / (float)((MachiningRoom)_currentRoom.RoomBehaviour).CurrentProductionTime * (float)newChrono;
    }

    /// <summary>
    /// Called to update the production count.
    /// </summary>
    /// <param name="newCount"> New count of the production. </param>
    private void UpdateProductionCount(int newCount)
    {
        if (newCount > 0)
        {
            _image.enabled = true;
            CheckItemStorage(0);
        }
        else
        {
            _image.enabled = false;
            _button.interactable = false;
        }

        _productionCount.text = newCount.ToString();
    }

    /// <summary>
    /// Called to change the color if the item storage is full or not.
    /// </summary>
    /// <param name="ignore"> Ignore this parameter. (put 0 in if you have to.) </param>
    private void CheckItemStorage(int ignore)
    {
        if (ItemStorage.Instance.IsStorageFull())
        {
            _image.color = new Color32(255, 0, 0, 175);
            _button.interactable = false;
        }
        else
        {
            _image.color = new Color32(0, 255, 0, 175);
            _button.interactable = true;
        }
    }
    #endregion

    /// <summary>
    /// Called to reset the notification.
    /// </summary>
    public void DesactivateNotification()
    {
        // Desactivate notification components
        _image.enabled = false;
        _notificationPicto.gameObject.SetActive(false);
        _button.interactable = false;
        _gaugeObject.SetActive(false);

        // Remove all listeners
        _button.onClick.RemoveAllListeners();
        RawMaterialStorage.Instance.CapacityHasChanged -= CheckRawMaterialStorage;
        RawMaterialStorage.Instance.AmountHasChanged -= CheckRawMaterialStorage;
        ItemStorage.Instance.CapacityHasChanged += CheckItemStorage;
        ItemStorage.Instance.AmountHasChanged += CheckItemStorage;

        // Remove the notification
        RoomNotificationManager.Instance.RemoveNotification(gameObject);
    }
}