using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStoragePopUp : MonoBehaviour
{
    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// List of buttons.
    /// </summary>
    [SerializeField, Space, Header("Storage")]
    private List<StorageButton> _storageButtons;

    [SerializeField]
    private TMP_Text _capacityTxt;

    public StorageButton CurrentSelectedStorageButton { get; private set; }

    [SerializeField, Space, Header("Item")]
    private TMP_Text _nameTxt;

    [SerializeField]
    private Image _pictoImg;

    [SerializeField]
    private TMP_Text _descriptionTxt;

    [SerializeField]
    private GameObject _sliderValueObj;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Button _throwButton;

    private void Awake()
    {
        _nameTxt.enabled = false;
        _pictoImg.enabled = false;
        _descriptionTxt.enabled = false;
        _sliderValueObj.SetActive(false);
        _slider.gameObject.SetActive(false);
        _throwButton.gameObject.SetActive(false);
        _popUp.SetActive(false);

        _throwButton.onClick.AddListener(ThrowItems);
    }

    private void OnEnable()
    {
        LoadPopUp();

        _popUp.SetActive(true);
    }

    private void LoadPopUp()
    {
        ItemStorage itemStorage = ItemStorage.Instance;

        _capacityTxt.text = itemStorage.CurrentStorage.ToString() + "/" + itemStorage.StorageCapacity.ToString();

        if (itemStorage.CurrentStorage > 0)
        {
            int index = 0;

            foreach (KeyValuePair<ComponentData, int> component in itemStorage.ComponentStorage)
            {
                _storageButtons[index].InitButtonForComponent(component.Key);
                _storageButtons[index].ButtonClicked += SelectButton;
                index++;
            }

            foreach (KeyValuePair<ObjectData, int> objectData in itemStorage.ObjectStorage)
            {
                _storageButtons[index].InitButtonForObject(objectData.Key);
                _storageButtons[index].ButtonClicked += SelectButton;
                index++;
            }

            _storageButtons[0].SelectButton();
        }

        UpdateItem();
    }

    private void UnloadPopUp()
    {
        CurrentSelectedStorageButton = null;

        for (int i = 0; i < _storageButtons.Count; i++)
        {
            _storageButtons[i].ButtonClicked -= SelectButton;
            _storageButtons[i].ResetButton();
        }
    }

    private void OnDisable()
    {
        _popUp.SetActive(false);

        _nameTxt.enabled = false;
        _pictoImg.enabled = false;
        _descriptionTxt.enabled = false;
        _sliderValueObj.SetActive(false);
        _slider.gameObject.SetActive(false);
        _throwButton.gameObject.SetActive(false);

        UnloadPopUp();
    }

    private void SelectButton(StorageButton selectedButton)
    {
        if (CurrentSelectedStorageButton != null)
        {
            CurrentSelectedStorageButton.UnselectButton();
        }

        CurrentSelectedStorageButton = selectedButton;

        UpdateItem();
    }

    private void UpdateItem()
    {
        if (CurrentSelectedStorageButton != null)
        {
            if (CurrentSelectedStorageButton.ComponentData != null)
            {
                ComponentData componentData = CurrentSelectedStorageButton.ComponentData;

                _nameTxt.text = componentData.Name;
                _nameTxt.enabled = true;
                _pictoImg.sprite = componentData.ComponentPicto;
                _pictoImg.enabled = true;
                _descriptionTxt.text = componentData.Description;
                _descriptionTxt.enabled = true;
                _sliderValueObj.SetActive(true);
                _slider.maxValue = ItemStorage.Instance.ReturnNumberOfThisComponent(componentData);
                _slider.value = 1f;
                _slider.gameObject.SetActive(true);
                _throwButton.gameObject.SetActive(true);
            }
            else if (CurrentSelectedStorageButton.ObjectData != null)
            {
                ObjectData objectData = CurrentSelectedStorageButton.ObjectData;

                _nameTxt.text = objectData.Name;
                _nameTxt.enabled = true;
                _pictoImg.sprite = objectData.ObjectPicto;
                _pictoImg.enabled = true;
                _descriptionTxt.text = objectData.Description;
                _descriptionTxt.enabled = true;
                _sliderValueObj.SetActive(true);
                _slider.maxValue = ItemStorage.Instance.ReturnNumberOfThisObject(objectData);
                _slider.value = 1f;
                _slider.gameObject.SetActive(true);
                _throwButton.gameObject.SetActive(true);
            }
        }
        else
        {
            _nameTxt.enabled = false;
            _pictoImg.enabled = false;
            _descriptionTxt.enabled = false;
            _sliderValueObj.SetActive(false);
            _slider.gameObject.SetActive(false);
            _throwButton.gameObject.SetActive(false);
        }
    }

    private void ThrowItems()
    {
        if (CurrentSelectedStorageButton.ComponentData != null)
        {
            ItemStorage.Instance.SubstractComponents(CurrentSelectedStorageButton.ComponentData, (int)_slider.value);
        }
        else if (CurrentSelectedStorageButton.ObjectData != null)
        {
            ItemStorage.Instance.SubstractObjects(CurrentSelectedStorageButton.ObjectData, (int)_slider.value);
        }

        if (_slider.maxValue == _slider.value)
        {
            UnloadPopUp();
            LoadPopUp();
        }
        else
        {
            _capacityTxt.text = ItemStorage.Instance.CurrentStorage.ToString() + "/" + ItemStorage.Instance.StorageCapacity.ToString();
            CurrentSelectedStorageButton.ReloadItemValue();
            UpdateItem();
        }
    }
}
