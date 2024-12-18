using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomProductionPopUp : MonoBehaviour
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
    /// Prefab of a button to product a component.
    /// </summary>
    [Space, Header("Production"), SerializeField]
    private GameObject _productionButtonPrefab;

    /// <summary>
    /// Row where buttons for production are.
    /// </summary>
    [SerializeField]
    private Transform _itemRow;

    /// <summary>
    /// List to stock all buttons generated.
    /// </summary>
    private List<GameObject> _productionButtons = new();

    private void Awake()
    {
        _popUp.SetActive(false);
    }

    private void OnEnable()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            _nameLvl.text = currentRoomSelected.RoomData.Name + " (Niveau " + currentRoomSelected.CurrentLvl.ToString() + ")";

            if (currentRoomSelected.RoomBehaviour is MachiningRoom)
            {
                List<ComponentData> manufacturableComponents = ResearchManager.Instance.ManufacturableComponents;

                for (int i = 0; i < manufacturableComponents.Count; i++)
                {
                    GameObject newProductionButton = Instantiate(_productionButtonPrefab, _itemRow);
                    _productionButtons.Add(newProductionButton);
                    newProductionButton.GetComponent<ProductionButton>().InitButtonForComponent(manufacturableComponents[i]);
                }
            }
            else
            {
                List<ObjectData> manufacturableObjects = ResearchManager.Instance.ManufacturableObjects;

                for (int i = 0; i < manufacturableObjects.Count; i++)
                {
                    GameObject newProductionButton = Instantiate(_productionButtonPrefab, _itemRow);
                    _productionButtons.Add(newProductionButton);
                    newProductionButton.GetComponent<ProductionButton>().InitButtonForObject(manufacturableObjects[i]);
                }
            }
        }

        _popUp.SetActive(true);
    }

    /// <summary>
    /// Called to reset production buttons and close pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _popUp.SetActive(false);

        for (int i =0; i< _productionButtons.Count; i++)
        {
            _productionButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(_productionButtons[i]);
        }

        _productionButtons.Clear();

        UIManager.Instance.CloseUI();
        gameObject.SetActive(false);
    }
}
