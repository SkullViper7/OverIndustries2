using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchPopUp : MonoBehaviour
{
    /// <summary>
    /// The text where name and lvl are displayed.
    /// </summary>
    [Space, Header("Infos"), SerializeField]
    private TMP_Text _nameLvl;

    /// <summary>
    /// Prefab of a button to launch a research.
    /// </summary>
    [Space, Header("Production"), SerializeField]
    private GameObject _researchButtonPrefab;

    /// <summary>
    /// Grid where buttons for components are stoked.
    /// </summary>
    [SerializeField]
    private Transform _componentsGrid;

    /// <summary>
    /// Grid where buttons for objects are stoked.
    /// </summary>
    [SerializeField]
    private Transform _objectsGrid;

    /// <summary>
    /// List to stock all buttons generated.
    /// </summary>
    private List<GameObject> _researchButtons = new();

    /// <summary>
    /// A reference to the research manager.
    /// </summary>
    private ResearchManager _researchManager;

    private void OnEnable()
    {
        _researchManager = ResearchManager.Instance;
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        if (currentRoomSelected != null)
        {
            _nameLvl.text = currentRoomSelected.RoomData.Name + " (Niveau " + currentRoomSelected.CurrentLvl.ToString() + ")";

            List<ComponentData> researchableComponents = _researchManager.AllResearchableComponents;

            for (int i = 0; i < researchableComponents.Count; i++)
            {
                GameObject newresearchButton = Instantiate(_researchButtonPrefab, _componentsGrid);
                _researchButtons.Add(newresearchButton);
                newresearchButton.GetComponent<ResearchButton>().InitButtonForComponent(researchableComponents[i]);
            }

            List<ObjectData> researchableObjects = _researchManager.AllResearchableObjects;

            for (int i = 0; i < researchableObjects.Count; i++)
            {
                GameObject newresearchButton = Instantiate(_researchButtonPrefab, _objectsGrid);
                _researchButtons.Add(newresearchButton);
                newresearchButton.GetComponent<ResearchButton>().InitButtonForObject(researchableObjects[i]);
            }
        }
    }

    /// <summary>
    /// Called to reset research buttons and close pop up.
    /// </summary>
    public void ClosePopUp()
    {
        for (int i = 0; i < _researchButtons.Count; i++)
        {
            _researchButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(_researchButtons[i]);
        }

        _researchButtons.Clear();

        UIManager.Instance.HUD.SetActive(true);
        gameObject.SetActive(false);
    }
}
