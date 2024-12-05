using UnityEngine;

public class EmployeeInteraction : MonoBehaviour
{
    /// <summary>
    /// A reference to the interaction manager.
    /// </summary>
    private InteractionManager _interactionManager;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    private UIManager _uiManager;

    [SerializeField] private JobProfileUI _jobProfileUI;

    private Employee _currentEmployeeSelected;

    private void Start()
    {
        _interactionManager = InteractionManager.Instance;
        _uiManager = UIManager.Instance;

        _interactionManager.EmployeeInteraction += ShowButtons;
        _interactionManager.NoInteraction += DesactivateAllButtons;
    }

    /// <summary>
    /// Called to show some buttons when an interaction on a room is triggered.
    /// </summary>
    /// <param name="employee"> Main component of the room. </param>
    public void ShowButtons(Employee employee)
    {
        _currentEmployeeSelected = employee;
        _uiManager.InfoEmployeeButton.SetActive(true);
        _uiManager.MoveEmployeeButton.SetActive(true);
    }

    /// <summary>
    /// Call _job profile to show name and employee job
    /// </summary>
    public void ShowInfoEmployee()
    {
        _jobProfileUI.ShowProfile(_currentEmployeeSelected);
    }

    /// <summary>
    /// Called to desactivate all buttons.
    /// </summary>
    private void DesactivateAllButtons()
    {
        _uiManager.InfoEmployeeButton.SetActive(false);
        _uiManager.MoveEmployeeButton.SetActive(false);
    }
}