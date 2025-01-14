using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoBehaviour
{
    /// <summary>
    /// Picto of the component or object.
    /// </summary>
    [SerializeField]
    private Image _picto;

    [SerializeField]
    private GameObject _alreadyResearchedFilter;

    [SerializeField]
    private Sprite _researchable;

    [SerializeField]
    private Sprite _unresearchable;

    [SerializeField]
    private Sprite _desactivate;

    /// <summary>
    /// Image component of the button.
    /// </summary>
    private Image _background;

    /// <summary>
    /// Button Component.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Datas of the component to research with this button.
    /// </summary>
    private ComponentData _componentData;

    /// <summary>
    /// Datas of the object to research with this button.
    /// </summary>
    private ObjectData _objectData;

    /// <summary>
    /// A reference to the research manager.
    /// </summary>
    private ResearchManager _researchManager;

    /// <summary>
    /// A reference to the interaction manager.
    /// </summary>
    private InteractionManager _interactionManager;

    /// <summary>
    /// Called to initialize the button to research a component.
    /// </summary>
    /// <param name="componentData"> Datas of the component. </param>
    public void InitButtonForComponent(ComponentData componentData)
    {
        _button = GetComponent<Button>();
        _background = GetComponent<Image>();

        _researchManager = ResearchManager.Instance;
        _interactionManager = InteractionManager.Instance;

        _componentData = componentData;
        _picto.sprite = _componentData.ComponentPicto;

        // Check if the component is researchable with the lvl of the room
        if (_researchManager.IsThisComponentResearchableAtThisLvl(_componentData, _interactionManager.CurrentRoomSelected.CurrentLvl))
        {
            // Check if the component is already searched
            if (!_researchManager.IsThisComponentAlreadySearched(_componentData))
            {
                // Check if the component has already been searched
                if (!_researchManager.HasThisComponentAlreadyBeenSearched(_componentData))
                {
                    _button.interactable = true;
                    _background.sprite = _researchable;
                    _button.onClick.AddListener(OpenComponentToResearchPopUp);
                }
                else
                {
                    _button.interactable = false;
                    _background.sprite = _desactivate;
                }
            }
            else
            {
                _button.interactable = true;
                _background.sprite = _researchable;
                _alreadyResearchedFilter.SetActive(true);
            }
        }
        else
        {
            // Check if the component is already searched
            if (!_researchManager.IsThisComponentAlreadySearched(_componentData))
            {
                _button.interactable = true;
                _background.sprite = _unresearchable;
            }
            else
            {
                _button.interactable = true;
                _background.sprite = _unresearchable;
                _alreadyResearchedFilter.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Called to initialize the button to research an object.
    /// </summary>
    /// <param name="objectData"> Datas of the object. </param>
    public void InitButtonForObject(ObjectData objectData)
    {
        _button = GetComponent<Button>();
        _background = GetComponent<Image>();

        _researchManager = ResearchManager.Instance;
        _interactionManager = InteractionManager.Instance;

        _objectData = objectData;
        _picto.sprite = _objectData.ObjectPicto;

        // Check if the object is researchable with the lvl of the room
        if (_researchManager.IsThisObjectResearchableAtThisLvl(_objectData, _interactionManager.CurrentRoomSelected.CurrentLvl))
        {
            // Check if the component is already searched
            if (!_researchManager.IsThisObjectAlreadySearched(_objectData))
            {
                // Check if the component has already been searched
                if (!_researchManager.HasThisObjectAlreadyBeenSearched(_objectData))
                {
                    _button.interactable = true;
                    _background.sprite = _researchable;
                    _button.onClick.AddListener(OpenObjectToResearchPopUp);
                }
                else
                {
                    _button.interactable = false;
                    _background.sprite = _desactivate;
                }
            }
            else
            {
                _button.interactable = true;
                _background.sprite = _researchable;
                _alreadyResearchedFilter.SetActive(true);
            }
        }
        else
        {
            // Check if the component is already searched
            if (!_researchManager.IsThisObjectAlreadySearched(_objectData))
            {
                _button.interactable = true;
                _background.sprite = _unresearchable;
            }
            else
            {
                _button.interactable = true;
                _background.sprite = _unresearchable;
                _alreadyResearchedFilter.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Called when button is clicked to launch a component research.
    /// </summary>
    private void OpenComponentToResearchPopUp()
    {
        UIManager.Instance.OpenSFX();
        UIManager.Instance.ItemToResearchPopUp.SetActive(true);
        UIManager.Instance.ItemToResearchPopUp.GetComponent<ItemToResearchPopUp>().InitPopUpForComponent(_componentData);
    }

    /// <summary>
    /// Called when button is clicked to launch an object research.
    /// </summary>
    private void OpenObjectToResearchPopUp()
    {
        UIManager.Instance.OpenSFX();
        UIManager.Instance.ItemToResearchPopUp.SetActive(true);
        UIManager.Instance.ItemToResearchPopUp.GetComponent<ItemToResearchPopUp>().InitPopUpForObject(_objectData);
    }
}
