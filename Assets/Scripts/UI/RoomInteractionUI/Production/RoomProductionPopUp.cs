using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomProductionPopUp : MonoBehaviour
{
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

    private void OnEnable()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

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
    /// Called to reset production buttons and close pop up.
    /// </summary>
    public void ClosePopUp()
    {
        for (int i =0; i< _productionButtons.Count; i++)
        {
            _productionButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(_productionButtons[i]);
        }

        _productionButtons.Clear();

        UIManager.Instance.HUD.SetActive(true);
        gameObject.SetActive(false);
    }
}
