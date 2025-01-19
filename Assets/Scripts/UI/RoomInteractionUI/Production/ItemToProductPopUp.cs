using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToProductPopUp : MonoBehaviour
{
    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// The text where it's asking to the player if he wants to product the item.
    /// </summary>
    [Header("Generic"), SerializeField]
    private TMP_Text _question;

    /// <summary>
    /// The picto of the item.
    /// </summary>
    [SerializeField]
    private Image _itemPicto;

    /// <summary>
    /// The name of the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _itemName;

    /// <summary>
    /// Container of the description.
    /// </summary>
    [SerializeField]
    private GameObject _description;

    /// <summary>
    /// The description of the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _itemDescription;

    /// <summary>
    /// The header of time to product the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionTimeHeader;

    /// <summary>
    /// The time to product the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionTime;

    /// <summary>
    /// The button to validate the production.
    /// </summary>
    [SerializeField]
    private Button _validationButton;

    /// <summary>
    /// Container of stats.
    /// </summary>
    [Space, Header("Stats"), SerializeField]
    private GameObject _stats;

    /// <summary>
    /// Header of the production time.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionRatetxt;

    /// <summary>
    /// The picto of the production.
    /// </summary>
    [SerializeField]
    private Image _productionRatePicto;

    /// <summary>
    /// The cell where component cost is displayed.
    /// </summary>
    [Space, Header("Component"), SerializeField]
    private GameObject _componentCostCell;

    /// <summary>
    /// The cost of the component.
    /// </summary>
    [SerializeField]
    private TMP_Text _componentCost;

    /// <summary>
    /// The cell where object recipe is displayed.
    /// </summary>
    [Space, Header("Object"), SerializeField]
    private GameObject _objectRecipeCell;

    /// <summary>
    /// The recipe of the object.
    /// </summary>
    [SerializeField]
    private GameObject[] _objectRecipe;

    /// <summary>
    /// The container of the employee needed.
    /// </summary>
    [SerializeField]
    private GameObject _employee;

    /// <summary>
    /// The picto of the employee needed.
    /// </summary>
    [SerializeField]
    private Image _employeeImage;

    [SerializeField]
    private Sprite _machiningEmployee;
    [SerializeField]
    private Sprite _assemblyEmployee;

    /// <summary>
    /// The text of the employee needed.
    /// </summary>
    [SerializeField]
    private TMP_Text _employeeTxt;

    /// <summary>
    /// Current component data displayed on screen.
    /// </summary>
    private ComponentData _currentComponentData;

    /// <summary>
    /// Current object data displayed on screen.
    /// </summary>
    private ObjectData _currentObjectData;

    [SerializeField]
    private SwitchButtons _statsButton;

    [SerializeField]
    private SwitchButtons _descriptionButton;

    private void Awake()
    {
        _popUp.SetActive(false);
        _description.SetActive(false);
        _stats.SetActive(true);
        _componentCostCell.SetActive(false);
        _objectRecipeCell.SetActive(false);
        for (int i = 0; i < _objectRecipe.Length; i++)
        {
            _objectRecipe[i].SetActive(false);
        }
        _employee.SetActive(false);
    }

    /// <summary>
    /// Called to display all data of the component on screen.
    /// </summary>
    /// <param name="componentData"> Data of the component. </param>
    public void InitPopUpForComponent(ComponentData componentData)
    {
        _currentComponentData = componentData;

        _question.text = "Produire " + _currentComponentData.Name + " ?";
        _itemPicto.sprite = _currentComponentData.ComponentPicto;
        _itemName.text = _currentComponentData.Name;
        _statsButton.Select();
        _descriptionButton.Unselect();
        _itemDescription.text = _currentComponentData.Description;
        _productionTimeHeader.text = "Durée d'un cycle de production :";

        switch (InteractionManager.Instance.CurrentRoomSelected.CurrentLvl)
        {
            case 1:
                _productionRatetxt.text = (60f / _currentComponentData.ProductionTimeAtLvl1).ToString();
                if (_currentComponentData.ProductionTimeAtLvl1 >= 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl1 / 60).ToString() + "min " + (_currentComponentData.ProductionTimeAtLvl1 % 60).ToString() + "sec";
                }
                else if (_currentComponentData.ProductionTimeAtLvl1 == 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl1 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentComponentData.ProductionTimeAtLvl1.ToString() + "sec";
                }
                break;
            case 2:
                _productionRatetxt.text = (60f / _currentComponentData.ProductionTimeAtLvl2).ToString();
                if (_currentComponentData.ProductionTimeAtLvl2 >= 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl2 / 60).ToString() + "min " + (_currentComponentData.ProductionTimeAtLvl2 % 60).ToString() + "sec";
                }
                else if (_currentComponentData.ProductionTimeAtLvl2 == 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl2 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentComponentData.ProductionTimeAtLvl2.ToString() + "sec";
                }
                break;
            case 3:
                _productionRatetxt.text = (60f / _currentComponentData.ProductionTimeAtLvl3).ToString();
                if (_currentComponentData.ProductionTimeAtLvl3 >= 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl3 / 60).ToString() + "min " + (_currentComponentData.ProductionTimeAtLvl3 % 60).ToString() + "sec";
                }
                else if (_currentComponentData.ProductionTimeAtLvl3 == 60)
                {
                    _productionTime.text = (_currentComponentData.ProductionTimeAtLvl3 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentComponentData.ProductionTimeAtLvl3.ToString() + "sec";
                }
                break;
        }
        _productionRatePicto.sprite = _currentComponentData.ComponentPicto;

        _componentCost.text = _currentComponentData.Cost.ToString();
        _componentCostCell.SetActive(true);

        _employeeTxt.text = _currentComponentData.JobNeeded.JobName;
        _employeeImage.sprite = _machiningEmployee;
        _employee.SetActive(true);

        _validationButton.onClick.AddListener(LaunchComponentProduction);

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called to display all data of the object on screen.
    /// </summary>
    /// <param name="componentData"> Data of the object. </param>
    public void InitPopUpForObject(ObjectData objectData)
    {
        _currentObjectData = objectData;

        _question.text = "Produire " + _currentObjectData.Name + " ?";
        _itemPicto.sprite = _currentObjectData.ObjectPicto;
        _itemName.text = _currentObjectData.Name;
        _itemDescription.text = _currentObjectData.Description;
        _statsButton.Select();
        _descriptionButton.Unselect();
        _productionTimeHeader.text = "Durée d'un cycle d'assemblage :";

        switch (InteractionManager.Instance.CurrentRoomSelected.CurrentLvl)
        {
            case 1:
                _productionRatetxt.text = (60f / _currentObjectData.ProductionTimeAtLvl1).ToString();
                if (_currentObjectData.ProductionTimeAtLvl1 >= 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl1 / 60).ToString() + "min " + (_currentObjectData.ProductionTimeAtLvl1 % 60).ToString() + "sec";
                }
                else if (_currentObjectData.ProductionTimeAtLvl1 == 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl1 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentObjectData.ProductionTimeAtLvl1.ToString() + "sec";
                }
                break;
            case 2:
                _productionRatetxt.text = (60f / _currentObjectData.ProductionTimeAtLvl2).ToString();
                if (_currentObjectData.ProductionTimeAtLvl2 >= 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl2 / 60).ToString() + "min " + (_currentObjectData.ProductionTimeAtLvl2 % 60).ToString() + "sec";
                }
                else if (_currentObjectData.ProductionTimeAtLvl2 == 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl2 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentObjectData.ProductionTimeAtLvl2.ToString() + "sec";
                }
                break;
            case 3:
                _productionRatetxt.text = (60f / _currentObjectData.ProductionTimeAtLvl3).ToString();
                if (_currentObjectData.ProductionTimeAtLvl3 >= 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl3 / 60).ToString() + "min " + (_currentObjectData.ProductionTimeAtLvl3 % 60).ToString() + "sec";
                }
                else if (_currentObjectData.ProductionTimeAtLvl3 == 60)
                {
                    _productionTime.text = (_currentObjectData.ProductionTimeAtLvl3 / 60).ToString() + "min ";
                }
                else
                {
                    _productionTime.text = _currentObjectData.ProductionTimeAtLvl3.ToString() + "sec";
                }
                break;
        }
        _productionRatePicto.sprite = _currentObjectData.ObjectPicto;

        List<Ingredient> recipe = _currentObjectData.Ingredients;
        for (int i = 0; i < recipe.Count; i++)
        {
            _objectRecipe[i].SetActive(true);
            _objectRecipe[i].GetComponentInChildren<Image>().sprite = recipe[i].ComponentData.ComponentPicto;
            _objectRecipe[i].GetComponentInChildren<TMP_Text>().text = recipe[i].Quantity.ToString();
        }
        _objectRecipeCell.SetActive(true);

        _employeeTxt.text = _currentObjectData.JobNeeded.JobName;
        _employeeImage.sprite = _assemblyEmployee;
        _employee.SetActive(true);

        _validationButton.onClick.AddListener(LaunchObjectProduction);

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called when validation button is clicked to launch a component production.
    /// </summary>
    private void LaunchComponentProduction()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;
            machiningRoom.TryStartProductionCycle(_currentComponentData);
            UIManager.Instance.UpgradeButton.SetActive(false);
            UIManager.Instance.ProductionButton.SetActive(false);
            UIManager.Instance.StopProductionButton.SetActive(true);
            UIManager.Instance.RoomProductionPopUp.GetComponent<RoomProductionPopUp>().ClosePopUp();
            ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }

    /// <summary>
    /// Called when validation button is clicked to launch an object production.
    /// </summary>
    private void LaunchObjectProduction()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;
            assemblyRoom.TryStartProductionCycle(_currentObjectData);
            UIManager.Instance.UpgradeButton.SetActive(false);
            UIManager.Instance.ProductionButton.SetActive(false);
            UIManager.Instance.StopProductionButton.SetActive(true);
            UIManager.Instance.RoomProductionPopUp.GetComponent<RoomProductionPopUp>().ClosePopUp();
            ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }

    /// <summary>
    /// Called to close pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _validationButton.onClick.RemoveAllListeners();

        _popUp.SetActive(false);

        _description.SetActive(false);
        _stats.SetActive(true);
        _componentCostCell.SetActive(false);
        _objectRecipeCell.SetActive(false);
        for (int i = 0; i < _objectRecipe.Length; i++)
        {
            _objectRecipe[i].SetActive(false);
        }
        _employee.SetActive(false);

        gameObject.SetActive(false);
    }
}
