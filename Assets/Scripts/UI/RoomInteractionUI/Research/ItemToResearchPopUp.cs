using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToResearchPopUp : MonoBehaviour
{
    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// The text where it's asking to the player if he wants to research the item.
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
    /// The time to research the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _researchTime;

    /// <summary>
    /// The button to validate the research.
    /// </summary>
    [SerializeField]
    private Button _validationButton;

    /// <summary>
    /// Container of stats.
    /// </summary>
    [Space, Header("Stats"), SerializeField]
    private GameObject _stats;

    /// <summary>
    /// The picto of the item type.
    /// </summary>
    [SerializeField]
    private Image _itemTypeImg;

    /// <summary>
    /// The type of the item.
    /// </summary>
    [SerializeField]
    private TMP_Text _itemType;

    /// <summary>
    /// The picto of the room type.
    /// </summary>
    [SerializeField]
    private Image _roomTypeImg;

    /// <summary>
    /// The type of the room to product this item.
    /// </summary>
    [SerializeField]
    private TMP_Text _roomType;

    /// <summary>
    /// Header of the production time at lvl 1.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionTime1Header;

    /// <summary>
    /// Production time at lvl 1.
    /// </summary>
    [SerializeField]
    private TMP_Text _productionTime1;

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
    /// The cell where component research cost is displayed.
    /// </summary>
    [SerializeField]
    private GameObject _componentResearchCostCell;

    /// <summary>
    /// The research cost of the component.
    /// </summary>
    [SerializeField]
    private TMP_Text _componentResearchCost;

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
    /// The cell where object research cost is displayed.
    /// </summary>
    [SerializeField]
    private GameObject _objectResearchCostCell;

    /// <summary>
    /// The research cost of the object in components.
    /// </summary>
    [SerializeField]
    private GameObject[] _objectResearchCostInComponents;

    /// <summary>
    /// The research cost of the object in raw material.
    /// </summary>
    [SerializeField]
    private TMP_Text _objectResearchCostInRawMaterial;

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

    [SerializeField]
    private Sprite _componentPicto;


    [SerializeField]
    private Sprite _objectPicto;

    private void Awake()
    {
        _popUp.SetActive(false);
        _description.SetActive(false);
        _stats.SetActive(true);
        _componentCostCell.SetActive(false);
        _componentResearchCostCell.SetActive(false);
        _objectRecipeCell.SetActive(false);
        _objectResearchCostCell.SetActive(false);
        for (int i = 0; i < _objectRecipe.Length; i++)
        {
            _objectRecipe[i].SetActive(false);
        }
        for (int i = 0; i < _objectResearchCostInComponents.Length; i++)
        {
            _objectResearchCostInComponents[i].SetActive(false);
        }
    }

    /// <summary>
    /// Called to display all data of the component on screen.
    /// </summary>
    /// <param name="componentData"> Data of the component. </param>
    public void InitPopUpForComponent(ComponentData componentData)
    {
        _currentComponentData = componentData;

        // Display generic data
        _question.text = "Débloquer " + _currentComponentData.Name + " ?";
        _itemPicto.sprite = _currentComponentData.ComponentPicto;
        _itemName.text = _currentComponentData.Name;
        _statsButton.Select();
        _descriptionButton.Unselect();
        _itemDescription.text = _currentComponentData.Description;
        if (_currentComponentData.ResearchTime >= 60)
        {
            _researchTime.text = (_currentComponentData.ResearchTime / 60).ToString() + "min " + (_currentComponentData.ResearchTime % 60).ToString() + "sec";
        }
        else
        {
            _researchTime.text = _currentComponentData.ResearchTime.ToString() + "sec";
        }
        _itemTypeImg.sprite = _componentPicto;
        _itemType.text = "Pièce détachée";
        if (ColorUtility.TryParseHtmlString("#E67E22", out Color newColor))
        {
            _roomTypeImg.color = newColor;
        }
        _roomType.text = "Salle d'usinage";
        _productionTime1Header.text = "Durée de fabrication au niveau 1 :";
        if (_currentComponentData.ResearchTime >= 60)
        {
            _productionTime1.text = (_currentComponentData.ProductionTimeAtLvl1 / 60).ToString() + "min " + (_currentComponentData.ProductionTimeAtLvl1 % 60).ToString() + "sec";
        }
        else
        {
            _productionTime1.text = _currentComponentData.ProductionTimeAtLvl1.ToString() + "sec";
        }

        // Display specific data
        _componentCostCell.SetActive(true);
        _componentCost.text = _currentComponentData.Cost.ToString();
        _componentResearchCostCell.SetActive(true);
        _componentResearchCost.text = _currentComponentData.ResearchCost.ToString();

        UpdateResearchCostForComponent();

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called to display all data of the object on screen.
    /// </summary>
    /// <param name="objectData"></param>
    public void InitPopUpForObject(ObjectData objectData)
    {
        _currentObjectData = objectData;

        // Display generic data
        _question.text = "Débloquer " + _currentObjectData.Name + " ?";
        _itemPicto.sprite = _currentObjectData.ObjectPicto;
        _itemName.text = _currentObjectData.Name;
        _itemDescription.text = _currentObjectData.Description;
        _statsButton.Select();
        _descriptionButton.Unselect();
        if (_currentObjectData.ResearchTime >= 60)
        {
            _researchTime.text = (_currentObjectData.ResearchTime / 60).ToString() + "min " + (_currentObjectData.ResearchTime % 60).ToString() + "sec";
        }
        else
        {
            _researchTime.text = _currentObjectData.ResearchTime.ToString() + "sec";
        }
        _itemTypeImg.sprite = _objectPicto;
        _itemType.text = "Objet complet";
        if (ColorUtility.TryParseHtmlString("#2E86C1", out Color newColor))
        {
            _roomTypeImg.color = newColor;
        }
        _roomType.text = "Salle d'assemblage";
        _productionTime1Header.text = "Durée d'assemblage au niveau 1 :";
        if (_currentObjectData.ResearchTime >= 60)
        {
            _productionTime1.text = (_currentObjectData.ProductionTimeAtLvl1 / 60).ToString() + "min " + (_currentObjectData.ProductionTimeAtLvl1 % 60).ToString() + "sec";
        }
        else
        {
            _productionTime1.text = _currentObjectData.ProductionTimeAtLvl1.ToString() + "sec";
        }

        // Display specific data
        List<Ingredient> recipe = _currentObjectData.Ingredients;
        for (int i = 0; i < recipe.Count; i++)
        {
            _objectRecipe[i].SetActive(true);
            _objectRecipe[i].GetComponentInChildren<Image>().sprite = recipe[i].ComponentData.ComponentPicto;
            _objectRecipe[i].GetComponentInChildren<TMP_Text>().text = recipe[i].Quantity.ToString();
        }
        _objectRecipeCell.SetActive(true);

        List<Ingredient> recipeCost = _currentObjectData.ResearchCost.IngredientsCost;
        for (int i = 0; i < recipeCost.Count; i++)
        {
            _objectResearchCostInComponents[i].SetActive(true);
            _objectResearchCostInComponents[i].GetComponentInChildren<Image>().sprite = recipe[i].ComponentData.ComponentPicto;
            _objectResearchCostInComponents[i].GetComponentInChildren<TMP_Text>().text = recipeCost[i].Quantity.ToString();
        }
        _objectResearchCostInRawMaterial.text = _currentObjectData.ResearchCost.RawMaterialCost.ToString();
        _objectResearchCostCell.SetActive(true);

        UpdateResearchCostForObject();

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called to update the availability of the research cost for a component.
    /// </summary>
    private void UpdateResearchCostForComponent()
    {
        _validationButton.onClick.RemoveAllListeners();

        if (RawMaterialStorage.Instance.AmoutOfRawMaterial >= _currentComponentData.ResearchCost)
        {
            _componentResearchCost.color = Color.white;
            _validationButton.onClick.AddListener(LaunchComponentResearch);
        }
        else
        {
            _componentResearchCost.color = Color.red;
        }
    }

    /// <summary>
    /// Called to update the availability of the research cost for an object.
    /// </summary>
    private void UpdateResearchCostForObject()
    {
        _validationButton.onClick.RemoveAllListeners();

        bool isCostAvailable = true;

        // Check for raw material
        if (RawMaterialStorage.Instance.AmoutOfRawMaterial >= _currentObjectData.ResearchCost.RawMaterialCost)
        {
            _objectResearchCostInRawMaterial.color = Color.white;
        }
        else
        {
            _objectResearchCostInRawMaterial.color = Color.red;
            isCostAvailable = false;
        }

        // Check for components
        List<Ingredient> recipeCost = _currentObjectData.ResearchCost.IngredientsCost;
        for (int i = 0; i < recipeCost.Count; i++)
        {
            if (ItemStorage.Instance.ThereIsEnoughComponentsInStorage(recipeCost[i].ComponentData, recipeCost[i].Quantity))
            {
                _objectResearchCostInComponents[i].GetComponentInChildren<TMP_Text>().color = Color.white;
            }
            else
            {
                _objectResearchCostInComponents[i].GetComponentInChildren<TMP_Text>().color = Color.red;
                isCostAvailable = false;
            }
        }

        if (isCostAvailable)
        {
            _validationButton.onClick.AddListener(LaunchObjectResearch);
        }
    }

    /// <summary>
    /// Called when validation button is clicked to launch a component research.
    /// </summary>
    private void LaunchComponentResearch()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            ResearchRoom researchRoom = (ResearchRoom)currentRoomSelected.RoomBehaviour;
            researchRoom.TryStartComponentResearch(_currentComponentData);
            UIManager.Instance.UpgradeButton.SetActive(false);
            UIManager.Instance.ResearchButton.SetActive(false);
            UIManager.Instance.StopProductionButton.SetActive(true);
            UIManager.Instance.RoomResearchPopUp.GetComponent<ResearchPopUp>().ClosePopUp();
            ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }

    /// <summary>
    /// Called when validation button is clicked to launch an object research.
    /// </summary>
    private void LaunchObjectResearch()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            ResearchRoom researchRoom = (ResearchRoom)currentRoomSelected.RoomBehaviour;
            researchRoom.TryStartObjectResearch(_currentObjectData);
            UIManager.Instance.UpgradeButton.SetActive(false);
            UIManager.Instance.ResearchButton.SetActive(false);
            UIManager.Instance.StopProductionButton.SetActive(true);
            UIManager.Instance.RoomResearchPopUp.GetComponent<ResearchPopUp>().ClosePopUp();
            ClosePopUp();
            UIManager.Instance.CloseUI();
        }
    }

    /// <summary>
    /// Called to close pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _popUp.SetActive(false);

        _description.SetActive(false);
        _stats.SetActive(true);

        _validationButton.onClick.RemoveAllListeners();

        _componentCostCell.SetActive(false);
        _componentResearchCostCell.SetActive(false);

        _objectRecipeCell.SetActive(false);
        _objectResearchCostCell.SetActive(false);

        for (int i = 0; i < _objectRecipe.Length; i++)
        {
            _objectRecipe[i].SetActive(false);
        }

        for (int i = 0; i < _objectResearchCostInComponents.Length; i++)
        {
            _objectResearchCostInComponents[i].SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
