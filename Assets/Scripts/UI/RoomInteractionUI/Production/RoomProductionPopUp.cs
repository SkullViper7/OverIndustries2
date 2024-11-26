using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomProductionPopUp : MonoBehaviour
{
    /// <summary>
    /// The button to close the pop up.
    /// </summary>
    [SerializeField]
    private Button _closeButton;

    /// <summary>
    /// The background which close the pop up when clicked.
    /// </summary>
    [SerializeField]
    private Button _background;

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
    /// Raw where buttons for production are.
    /// </summary>
    [SerializeField]
    private Transform _raw;

    /// <summary>
    /// List to stock all buttons generated.
    /// </summary>
    private List<GameObject> _productionButtons = new();

    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePopUp);
        _background.onClick.AddListener(ClosePopUp);
    }

    /// <summary>
    /// Called to display data of the room.
    /// </summary>
    public void DisplayDatas()
    {
        RoomTemp currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            _nameLvl.text = currentRoomSelected.RoomData.Name + " (Niveau " + currentRoomSelected.CurrentLvl.ToString() + ")";

            if (currentRoomSelected.RoomBehaviour is MachiningRoom)
            {
                MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;

                for (int i = 0; i < machiningRoom.ManufacturableComponents.Count; i++)
                {
                    GameObject newProductionButton = Instantiate(_productionButtonPrefab, _raw);
                    _productionButtons.Add(newProductionButton);
                    newProductionButton.GetComponent<ProductionButton>().InitButtonForComponent(machiningRoom.ManufacturableComponents[i]);
                }
            }
            else
            {
                AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;

                for (int i = 0; i < assemblyRoom.AssemblableObjects.Count; i++)
                {
                    GameObject newProductionButton = Instantiate(_productionButtonPrefab, _raw);
                    _productionButtons.Add(newProductionButton);
                    newProductionButton.GetComponent<ProductionButton>().InitButtonForObject(assemblyRoom.AssemblableObjects[i]);
                }
            }
        }
    }

    /// <summary>
    /// Called to close the pop up.
    /// </summary>
    public void ClosePopUp()
    {
        _nameLvl.text = "";

        for (int i =0; i< _productionButtons.Count; i++)
        {
            Destroy(_productionButtons[i]);
        }

        _productionButtons.Clear();
        gameObject.SetActive(false);
    }
}
