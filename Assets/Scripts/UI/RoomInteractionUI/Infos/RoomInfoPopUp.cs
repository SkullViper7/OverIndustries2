using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoPopUp : MonoBehaviour
{
    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// The text where name and lvl are displayed.
    /// </summary>
    [Space, Header("Infos"), SerializeField]
    private TMP_Text _nameLvl;

    /// <summary>
    /// The image where preview of the room is displayed.
    /// </summary>
    [SerializeField]
    private Image _roomPreview;

    /// <summary>
    /// The text where name is displayed under the preview.
    /// </summary>
    [SerializeField]
    private TMP_Text _name;

    /// <summary>
    /// The container of the description.
    /// </summary>
    [SerializeField]
    private GameObject _descriptionContainer;

    /// <summary>
    /// The text where description is displayed.
    /// </summary>
    [SerializeField]
    private TMP_Text _description;

    /// <summary>
    /// The container of stats.
    /// </summary>
    [Space, Header("Stats"), SerializeField]
    private GameObject _statsContainer;

    /// <summary>
    /// The container of the current production.
    /// </summary>
    [SerializeField]
    private GameObject _production;

    /// <summary>
    /// The header of the current production.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionHeader;

    /// <summary>
    /// The picto of the current production.
    /// </summary>
    [SerializeField]
    private Image _productionImage;

    /// <summary>
    /// The text of the current production.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionTxt;

    /// <summary>
    /// The container of the current production rate.
    /// </summary>
    [SerializeField]
    private GameObject _productionRate;

    /// <summary>
    /// The header of the current production rate.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionRateHeader;

    /// <summary>
    /// The picto of the current production rate.
    /// </summary>
    [SerializeField]
    private Image _productionRateImage;

    /// <summary>
    /// The text of the current production rate.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionRateTxt;

    /// <summary>
    /// The container of the current capacity.
    /// </summary>
    [SerializeField]
    private GameObject _capacity;

    /// <summary>
    /// The header of the current capacity.
    /// </summary>
    [SerializeField]
    private TMP_Text _capacityHeader;

    /// <summary>
    /// The picto of the current capacity.
    /// </summary>
    [SerializeField]
    private Image _capacityImage;

    /// <summary>
    /// The text of the current capacity.
    /// </summary>
    [SerializeField]
    private TMP_Text _capacityTxt;

    /// <summary>
    /// Buttons to switch between stats and description.
    /// </summary>
    [Space, Header("Buttons"), SerializeField]
    private GameObject _switchButtons;

    /// <summary>
    /// The current room selected.
    /// </summary>
    private Room _currentRoomSelected;

    /// <summary>
    /// The current room behaviour selected.
    /// </summary>
    private IRoomBehaviour _currentRoomBehaviour;

    private void Awake()
    {
        _popUp.SetActive(false);
        _descriptionContainer.SetActive(false);
        _switchButtons.SetActive(false);
        _statsContainer.SetActive(false);
        _capacity.SetActive(false);
        _productionRate.SetActive(false);
        _production.SetActive(false);
    }

    private void OnEnable()
    {
        _currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;
        _currentRoomBehaviour = _currentRoomSelected.RoomBehaviour;

        if (_currentRoomSelected != null)
        {
            switch (_currentRoomSelected.CurrentLvl)
            {
                case 1:
                    _roomPreview.sprite = _currentRoomSelected.RoomData.RoomLvl1Preview;
                    break;
                case 2:
                    _roomPreview.sprite = _currentRoomSelected.RoomData.RoomLvl2Preview;
                    break;
                case 3:
                    _roomPreview.sprite = _currentRoomSelected.RoomData.RoomLvl3Preview;
                    break;
            }

            _name.text = _currentRoomSelected.RoomData.Name;
            _description.text = _currentRoomSelected.RoomData.Description;

            switch (_currentRoomSelected.RoomData.RoomType)
            {
                case RoomType.Delivery:
                    DisplayDeliveryRoomData();
                    break;
                case RoomType.Machining:
                    DisplayMachiningRoomData();
                    break;
                case RoomType.Assembly:
                    DisplayAssemblyRoomData();
                    break;
                case RoomType.RawMaterialStorage:
                    DisplayRawMaterialStorageData();
                    break;
                case RoomType.Storage:
                    DisplayItemStorageData();
                    break;
                case RoomType.Research:
                    DisplayResearchRoomData();
                    break;
                case RoomType.Rest:
                    break;
                case RoomType.Director:
                    break;
                case RoomType.Elevator:
                    DisplayElevatorRoomData();
                    break;
                case RoomType.Recycling:
                    break;
            }
        }

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a delivery room.
    /// </summary>
    private void DisplayDeliveryRoomData()
    {
        DeliveryRoom deliveryRoom = (DeliveryRoom)_currentRoomBehaviour;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        _productionHeader.text = "Production en cours :";
        _productionImage.sprite = deliveryRoom.DeliveryRoomData.RawMaterialPicto;
        _productionTxt.text = "Matière première";
        _production.SetActive(true);

        _productionRateHeader.text = "Production par minute :";
        _productionRateImage.sprite = deliveryRoom.DeliveryRoomData.RawMaterialPicto;
        _productionRateTxt.text = ((deliveryRoom.CurrentProductionPerDelivery * 60f) / deliveryRoom.CurrentDeliveryTime).ToString();
        _productionRate.SetActive(true);

        _capacityHeader.text = "Capacité :";
        _capacityImage.sprite = deliveryRoom.DeliveryRoomData.RawMaterialPicto;
        UpdateAmountInInternalStorage(deliveryRoom.CurrentAmountInInternalStorage, deliveryRoom.CurrentInternalCapacity);
        deliveryRoom.NewAmountInInternalStorage += UpdateAmountInInternalStorage;
        _capacity.SetActive(true);
        _statsContainer.SetActive(true);
        _switchButtons.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a machining room.
    /// </summary>
    private void DisplayMachiningRoomData()
    {
        MachiningRoom machiningRoom = (MachiningRoom)_currentRoomBehaviour;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        if (machiningRoom.CurrentComponentManufactured != null)
        {
            _productionHeader.text = "Production en cours :";
            _productionImage.sprite = machiningRoom.CurrentComponentManufactured.ComponentPicto;
            _productionTxt.text = machiningRoom.CurrentComponentManufactured.Name;
            _production.SetActive(true);

            _productionRateHeader.text = "Production par minute :";
            _productionRateImage.sprite = machiningRoom.CurrentComponentManufactured.ComponentPicto; ;
            _productionRateTxt.text = (60f / machiningRoom.CurrentProductionTime).ToString();
            _productionRate.SetActive(true);

            _capacityHeader.text = "Capacité :";
            _capacityImage.sprite = machiningRoom.CurrentComponentManufactured.ComponentPicto; ;
            UpdateAmountInInternalStorage(machiningRoom.CurrentAmountInInternalStorage, machiningRoom.CurrentInternalCapacity);
            machiningRoom.NewAmountInInternalStorage += UpdateAmountInInternalStorage;
            _capacity.SetActive(true);
            _statsContainer.SetActive(true);
            _switchButtons.SetActive(true);
        }
        else
        {
            _descriptionContainer.SetActive(true);
        }
    }

    /// <summary>
    /// Called to display infos when it's an assembly room.
    /// </summary>
    private void DisplayAssemblyRoomData()
    {
        AssemblyRoom assemblyRoom = (AssemblyRoom)_currentRoomBehaviour;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        if (assemblyRoom.CurrentObjectManufactured != null)
        {
            _productionHeader.text = "Assemblage en cours :";
            _productionImage.sprite = assemblyRoom.CurrentObjectManufactured.ObjectPicto;
            _productionTxt.text = assemblyRoom.CurrentObjectManufactured.Name;
            _production.SetActive(true);

            _productionRateHeader.text = "Production par minute :";
            _productionRateImage.sprite = assemblyRoom.CurrentObjectManufactured.ObjectPicto; ;
            _productionRateTxt.text = (60f / assemblyRoom.CurrentProductionTime).ToString();
            _productionRate.SetActive(true);

            _capacityHeader.text = "Capacité :";
            _capacityImage.sprite = assemblyRoom.CurrentObjectManufactured.ObjectPicto; ;
            UpdateAmountInInternalStorage(assemblyRoom.CurrentAmountInInternalStorage, assemblyRoom.CurrentInternalCapacity);
            assemblyRoom.NewAmountInInternalStorage += UpdateAmountInInternalStorage;
            _capacity.SetActive(true);
            _statsContainer.SetActive(true);
            _switchButtons.SetActive(true);
        }
        else
        {
            _descriptionContainer.SetActive(true);
        }
    }

    /// <summary>
    /// Called to display infos when it's a raw material storage.
    /// </summary>
    private void DisplayRawMaterialStorageData()
    {
        RawMaterialStorageRoom rawMaterialStorageRoom = (RawMaterialStorageRoom)_currentRoomBehaviour;
        RawMaterialStorage rawMaterialStorage = RawMaterialStorage.Instance;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        _capacityHeader.text = "Capacité :";
        _capacityImage.sprite = rawMaterialStorageRoom.RawMaterialStorageRoomData.RawMaterialPicto;
        UpdateAmountInInternalStorage(rawMaterialStorage.AmoutOfRawMaterial, rawMaterialStorage.StorageCapacity);
        rawMaterialStorage.NewAmountInInternalStorage += UpdateAmountInInternalStorage;
        _capacity.SetActive(true);
        _statsContainer.SetActive(true);
        _switchButtons.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a storage room.
    /// </summary>
    private void DisplayItemStorageData()
    {
        StorageRoom storageRoom = (StorageRoom)_currentRoomBehaviour;
        ItemStorage itemStorage = ItemStorage.Instance;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        _capacityHeader.text = "Capacité :";
        _capacityImage.sprite = storageRoom.StorageRoomData.ItemsPicto;
        UpdateAmountInInternalStorage(itemStorage.CurrentStorage, itemStorage.StorageCapacity);
        itemStorage.NewAmountInInternalStorage += UpdateAmountInInternalStorage;
        _capacity.SetActive(true);
        _statsContainer.SetActive(true);
        _switchButtons.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a research room.
    /// </summary>
    private void DisplayResearchRoomData()
    {
        ResearchRoom researchRoom = (ResearchRoom)_currentRoomBehaviour;

        _nameLvl.text = _currentRoomSelected.RoomData.Name + " (Niveau " + _currentRoomSelected.CurrentLvl.ToString() + ")";

        if (researchRoom.CurrentComponentResearched != null)
        {
            _productionHeader.text = "Recherche en cours :";
            _productionImage.sprite = researchRoom.CurrentComponentResearched.ComponentPicto;
            _productionTxt.text = researchRoom.CurrentComponentResearched.Name;
            _production.SetActive(true);
            _statsContainer.SetActive(true);
            _switchButtons.SetActive(true);
        }
        else if (researchRoom.CurrentObjectResearched != null)
        {
            _productionHeader.text = "Recherche en cours :";
            _productionImage.sprite = researchRoom.CurrentObjectResearched.ObjectPicto;
            _productionTxt.text = researchRoom.CurrentObjectResearched.Name;
            _production.SetActive(true);
            _statsContainer.SetActive(true);
            _switchButtons.SetActive(true);
        }
        else
        {
            _descriptionContainer.SetActive(true);
        }
    }

    /// <summary>
    /// Called to display infos when it's an elevator.
    /// </summary>
    private void DisplayElevatorRoomData()
    {
        Elevator elevator = (Elevator)_currentRoomBehaviour;

        _nameLvl.text = _currentRoomSelected.RoomData.Name;

        _descriptionContainer.SetActive(true);
    }

    /// <summary>
    /// Called to update the amount in internal storage of a room.
    /// </summary>
    /// <param name="newAmount"> New amount in the storage. </param>
    /// <param name="capacity"> Capacity of the storage. </param>
    private void UpdateAmountInInternalStorage(int newAmount, int capacity)
    {
        _capacityTxt.text = newAmount.ToString() + "/" + capacity.ToString();
    }

    /// <summary>
    /// Called to close the pop up and reset some values.
    /// </summary>
    public void ClosePopUp()
    {
        _popUp.SetActive(false);
        _descriptionContainer.SetActive(false);
        _switchButtons.SetActive(false);
        _statsContainer.SetActive(false);
        _capacity.SetActive(false);
        _productionRate.SetActive(false);
        _production.SetActive(false);

        switch (_currentRoomSelected.RoomData.RoomType)
        {
            case RoomType.Delivery:
                DeliveryRoom deliveryRoom = (DeliveryRoom)_currentRoomBehaviour;
                deliveryRoom.NewAmountInInternalStorage -= UpdateAmountInInternalStorage;
                break;
            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)_currentRoomBehaviour;
                machiningRoom.NewAmountInInternalStorage -= UpdateAmountInInternalStorage;
                break;
        }
    }
}
