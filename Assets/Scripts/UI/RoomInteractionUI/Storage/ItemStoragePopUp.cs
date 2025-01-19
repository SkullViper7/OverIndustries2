using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemStoragePopUp : MonoBehaviour
{
    /// <summary>
    /// List of buttons.
    /// </summary>
    [SerializeField]
    private List<StorageButton> _storageButtons;

    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    [SerializeField]
    private TMP_Text _capacityTxt;

    public StorageButton CurrentSelectedStorageButton { get; private set; }

    private void Awake()
    {
        _popUp.SetActive(false);
    }

    private void OnEnable()
    {
        ItemStorage itemStorage = ItemStorage.Instance;

        _capacityTxt.text = itemStorage.CurrentStorage.ToString() + "/" + itemStorage.StorageCapacity.ToString();

        int index = 0;

        foreach (KeyValuePair<ComponentData, int> component in itemStorage.ComponentStorage)
        {
            _storageButtons[index].InitButtonForComponent(component.Key, component.Value);
            _storageButtons[index].ButtonClicked += SelectButton;
            index++;
        }

        foreach (KeyValuePair<ObjectData, int> objectData in itemStorage.ObjectStorage)
        {
            _storageButtons[index].InitButtonForObject(objectData.Key, objectData.Value);
            index++;
        }

        _popUp.SetActive(true);
    }

    private void OnDisable()
    {
        CurrentSelectedStorageButton = null;
        _popUp.SetActive(false);

        for (int i = 0;  i < _storageButtons.Count; i++)
        {
            _storageButtons[i].ButtonClicked -= SelectButton;
            _storageButtons[i].ResetButton();
        }
    }

    public void SelectButton(StorageButton selectedButton)
    {
        if (CurrentSelectedStorageButton != null)
        {
            CurrentSelectedStorageButton.UnselectButton();
        }

        CurrentSelectedStorageButton = selectedButton;
    }
}
