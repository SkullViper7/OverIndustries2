using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRoomPopUp : MonoBehaviour
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
    /// The container of a new research.
    /// </summary>
    [SerializeField]
    private GameObject _newResearch;

    /// <summary>
    /// The list of new research containers.
    /// </summary>
    private List<GameObject> _newResearchContainers = new();

    /// <summary>
    /// The container of the new raw material production rate.
    /// </summary>
    [SerializeField]
    private GameObject _newRawMaterialProductionRate;

    /// <summary>
    /// The text of the new raw material production rate.
    /// </summary>
    [SerializeField]
    private TMP_Text _newProductionRateTxt;

    /// <summary>
    /// The container of the new capacity.
    /// </summary>
    [SerializeField]
    private GameObject _capacity;

    /// <summary>
    /// The picto of the new capacity.
    /// </summary>
    [SerializeField]
    private Image _capacityImage;

    /// <summary>
    /// The text of the new capacity.
    /// </summary>
    [SerializeField]
    private TMP_Text _capacityTxt;

    /// <summary>
    /// The container of the upgraded production time.
    /// </summary>
    [SerializeField]
    private GameObject _upgradedProductionTime;

    /// <summary>
    /// The text of the upgrade cost.
    /// </summary>
    [Space, Header("Upgrade"), SerializeField]
    private TMP_Text _upgradeCostTxt;

    /// <summary>
    /// The button to upgrade the room.
    /// </summary>
    [SerializeField]
    private Button _upgradeButton;

    /// <summary>
    /// The current room selected.
    /// </summary>
    private Room _currentRoomSelected;

    [SerializeField]
    private SwitchButtons _statsButton;

    [SerializeField]
    private SwitchButtons _descriptionButton;

    [SerializeField]
    private ScrollRect _scrollRect;

    /// <summary>
    /// The current room behaviour selected.
    /// </summary>
    private IRoomBehaviour _currentRoomBehaviour;

    private void Awake()
    {
        _popUp.SetActive(false);
        _descriptionContainer.SetActive(false);
        _statsContainer.SetActive(true);
        _capacity.SetActive(false);
        _newRawMaterialProductionRate.SetActive(false);
        _upgradedProductionTime.SetActive(false);
        _scrollRect.enabled = false;
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
                    _roomPreview.sprite = _currentRoomSelected.RoomData.RoomLvl2Preview;
                    _upgradeCostTxt.text = _currentRoomSelected.RoomData.UpgradeCostToLvl2.ToString();
                    break;
                case 2:
                    _roomPreview.sprite = _currentRoomSelected.RoomData.RoomLvl3Preview;
                    _upgradeCostTxt.text = _currentRoomSelected.RoomData.UpgradeCostToLvl3.ToString();
                    break;
            }

            _nameLvl.text = "Améliorer " + _currentRoomSelected.RoomData.Name + " au niveau " + (_currentRoomSelected.CurrentLvl + 1).ToString() + " ?";
            _description.text = _currentRoomSelected.RoomData.Description;
            _statsButton.Select();
            _descriptionButton.Unselect();
            _name.text = _currentRoomSelected.RoomData.Name;
            SetUpgradeCost();

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

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                _newProductionRateTxt.text = ((deliveryRoom.DeliveryRoomData.ProductionPerDeliveryAtLvl2 * 60f) / deliveryRoom.DeliveryRoomData.DeliveryTimeAtLvl2).ToString();
                _capacityTxt.text = deliveryRoom.DeliveryRoomData.InternalStorageAtLvl2.ToString();
                break;
            case 2:
                _newProductionRateTxt.text = ((deliveryRoom.DeliveryRoomData.ProductionPerDeliveryAtLvl3 * 60f) / deliveryRoom.DeliveryRoomData.DeliveryTimeAtLvl3).ToString();
                _capacityTxt.text = deliveryRoom.DeliveryRoomData.InternalStorageAtLvl3.ToString();
                break;
        }
        _capacityImage.sprite = deliveryRoom.DeliveryRoomData.RawMaterialPicto;

        _newRawMaterialProductionRate.SetActive(true);
        _capacity.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a machining room.
    /// </summary>
    private void DisplayMachiningRoomData()
    {
        MachiningRoom machiningRoom = (MachiningRoom)_currentRoomBehaviour;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                _capacityTxt.text = machiningRoom.MachiningRoomData.InternalStorageAtLvl2.ToString();
                break;
            case 2:
                _capacityTxt.text = machiningRoom.MachiningRoomData.InternalStorageAtLvl3.ToString();
                break;
        }
        _capacityImage.sprite = machiningRoom.MachiningRoomData.StoragePicto;

        _capacity.SetActive(true);
        _upgradedProductionTime.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's an assembly room.
    /// </summary>
    private void DisplayAssemblyRoomData()
    {
        AssemblyRoom assemblyRoom = (AssemblyRoom)_currentRoomBehaviour;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                _capacityTxt.text = assemblyRoom.AssemblyRoomData.InternalStorageAtLvl2.ToString();
                break;
            case 2:
                _capacityTxt.text = assemblyRoom.AssemblyRoomData.InternalStorageAtLvl3.ToString();
                break;
        }
        _capacityImage.sprite = assemblyRoom.AssemblyRoomData.StoragePicto;

        _capacity.SetActive(true);
        _upgradedProductionTime.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a raw material storage room.
    /// </summary>
    private void DisplayRawMaterialStorageData()
    {
        RawMaterialStorageRoom rawMaterialStorageRoom = (RawMaterialStorageRoom)_currentRoomBehaviour;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                _capacityTxt.text = "+ " + rawMaterialStorageRoom.RawMaterialStorageRoomData.CapacityBonusAtLvl2.ToString();
                break;
            case 2:
                _capacityTxt.text = "+ " + rawMaterialStorageRoom.RawMaterialStorageRoomData.CapacityBonusAtLvl3.ToString();
                break;
        }
        _capacityImage.sprite = rawMaterialStorageRoom.RawMaterialStorageRoomData.RawMaterialPicto;

        _capacity.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a storage room.
    /// </summary>
    private void DisplayItemStorageData()
    {
        StorageRoom storageRoom = (StorageRoom)_currentRoomBehaviour;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                _capacityTxt.text = "+ " + storageRoom.StorageRoomData.CapacityBonusAtLvl2.ToString();
                break;
            case 2:
                _capacityTxt.text = "+ " + storageRoom.StorageRoomData.CapacityBonusAtLvl3.ToString();
                break;
        }
        _capacityImage.sprite = storageRoom.StorageRoomData.ItemsPicto;

        _capacity.SetActive(true);
    }

    /// <summary>
    /// Called to display infos when it's a research room.
    /// </summary>
    private void DisplayResearchRoomData()
    {
        ResearchManager researchManager = ResearchManager.Instance;

        _scrollRect.enabled = true;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                {
                    List<ComponentData> components = researchManager.AllResearchableComponents;
                    for (int i = 0; i < components.Count; i++)
                    {
                        if (researchManager.IsThisComponentResearchableAtThisLvl(components[i], 2))
                        {
                            GameObject newResearchContainer = Instantiate(_newResearch, _statsContainer.transform);
                            newResearchContainer.GetComponent<NewResearchContainer>().InitData(components[i].ComponentPicto, components[i].Name);
                            _newResearchContainers.Add(newResearchContainer);
                        }
                    }
                    List<ObjectData> objects = researchManager.AllResearchableObjects;
                    for (int i = 0; i < objects.Count; i++)
                    {
                        if (researchManager.IsThisObjectResearchableAtThisLvl(objects[i], 2))
                        {
                            GameObject newResearchContainer = Instantiate(_newResearch, _statsContainer.transform);
                            newResearchContainer.GetComponent<NewResearchContainer>().InitData(objects[i].ObjectPicto, objects[i].Name);
                            _newResearchContainers.Add(newResearchContainer);
                        }
                    }
                }
                break;
            case 2:
                {
                    List<ComponentData> components = researchManager.AllResearchableComponents;
                    for (int i = 0; i < components.Count; i++)
                    {
                        if (researchManager.IsThisComponentResearchableAtThisLvl(components[i], 3))
                        {
                            GameObject newResearchContainer = Instantiate(_newResearch, _statsContainer.transform);
                            newResearchContainer.GetComponent<NewResearchContainer>().InitData(components[i].ComponentPicto, components[i].Name);
                            _newResearchContainers.Add(newResearchContainer);
                        }
                    }
                    List<ObjectData> objects = researchManager.AllResearchableObjects;
                    for (int i = 0; i < objects.Count; i++)
                    {
                        if (researchManager.IsThisObjectResearchableAtThisLvl(objects[i], 3))
                        {
                            GameObject newResearchContainer = Instantiate(_newResearch, _statsContainer.transform);
                            newResearchContainer.GetComponent<NewResearchContainer>().InitData(objects[i].ObjectPicto, objects[i].Name);
                            _newResearchContainers.Add(newResearchContainer);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Called to set the availability of the upgrade cost.
    /// </summary>
    private void SetUpgradeCost()
    {
        _upgradeButton.onClick.RemoveAllListeners();
        RawMaterialStorage rawMaterialStorage = RawMaterialStorage.Instance;

        switch (_currentRoomSelected.CurrentLvl)
        {
            case 1:
                if (rawMaterialStorage.AmoutOfRawMaterial >= _currentRoomSelected.RoomData.UpgradeCostToLvl2)
                {
                    _upgradeCostTxt.color = Color.white;
                    _upgradeButton.onClick.AddListener(UpgradeCurrentSelectedRoom);
                }
                else
                {
                    _upgradeCostTxt.color = Color.red;
                }
                break;
            case 2:
                if (rawMaterialStorage.AmoutOfRawMaterial >= _currentRoomSelected.RoomData.UpgradeCostToLvl3)
                {
                    _upgradeCostTxt.color = Color.white;
                    _upgradeButton.onClick.AddListener(UpgradeCurrentSelectedRoom);
                }
                else
                {
                    _upgradeCostTxt.color = Color.red;
                }
                break;
        }
    }

    /// <summary>
    /// Called to upgrade the current selected room.
    /// </summary>
    public void UpgradeCurrentSelectedRoom()
    {
        if (_currentRoomSelected != null)
        {
            if (_currentRoomBehaviour is DeliveryRoom deliveryRoom)
            {
                if (deliveryRoom.CurrentAmountInInternalStorage > 0)
                {
                    UIManager.Instance.DeliveryUpgradeWarningPopUp.SetActive(true);
                    UIManager.Instance.DeliveryUpgradeWarningPopUp.GetComponent<DeliveryWarningButton>().ValidateButton.onClick.AddListener(() =>
                    {
                        _currentRoomSelected.UpgradeRoom();
                        UIManager.Instance.CloseSFX();
                        UIManager.Instance.DeliveryUpgradeWarningPopUp.SetActive(false);
                        ClosePopUp();
                    });
                }
                else
                {
                    _currentRoomSelected.UpgradeRoom();
                    ClosePopUp();
                }
            }
            else
            {
                _currentRoomSelected.UpgradeRoom();
                ClosePopUp();
            }
        }
    }

    /// <summary>
    /// Called to close the pop up and reset some values.
    /// </summary>
    public void ClosePopUp()
    {
        _popUp.SetActive(false);
        _descriptionContainer.SetActive(false);
        _statsContainer.SetActive(true);
        _capacity.SetActive(false);
        _newRawMaterialProductionRate.SetActive(false);
        _upgradedProductionTime.SetActive(false);

        _scrollRect.enabled = false;

        for (int i = 0; i < _newResearchContainers.Count; i++)
        {
            Destroy(_newResearchContainers[i]);
        }
        _newResearchContainers.Clear();

        UIManager.Instance.CloseUI();
        gameObject.SetActive(false);
    }
}
