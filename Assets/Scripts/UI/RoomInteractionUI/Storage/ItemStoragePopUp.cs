using System.Collections.Generic;
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

    private void Awake()
    {
        _popUp.SetActive(false);
    }

    private void OnEnable()
    {
        ItemStorage itemStorage = ItemStorage.Instance;
        int index = 0;

        foreach (KeyValuePair<ComponentData, int> component in itemStorage.ComponentStorage)
        {
            _storageButtons[index].InitButtonForComponent(component.Key, component.Value);
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
        _popUp.SetActive(false);

        ChronoManager.Instance.ResumeChrono();

        for (int i = 0;  i < _storageButtons.Count; i++)
        {
            _storageButtons[i].ResetButton();
        }
    }
}
