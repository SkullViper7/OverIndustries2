using UnityEngine;

public class EmployeeInteractionManager : MonoBehaviour
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

        _interactionManager.EmployeeSelected += SelectEmployee;
        _interactionManager.EmployeeUnselected += DesactivateAllButtons;
    }

    /// <summary>
    /// Called to show some buttons when an interaction on an employee is triggered.
    /// </summary>
    /// <param name="employee"> Main component of the employee. </param>
    public void SelectEmployee(Employee employee)
    {
        _currentEmployeeSelected = employee;
        _uiManager.InfoEmployeeButton.SetActive(true);

        employee.StopRoutine();

        // Play animation
        _uiManager.InteractionButtonGroup.ShowButtons();
    }

    /// <summary>
    /// Call _job profile to show name and employee job
    /// </summary>
    public void ShowInfoEmployee()
    {
        _jobProfileUI.ShowProfile(_currentEmployeeSelected);
    }
    
    /// <summary>
    /// Call _job profile to show give employee selected to show fiche metier
    /// </summary>
    public void ShowInfoJob()
    {
        _jobProfileUI.ShowHiredEmployeeFicheMetier(_currentEmployeeSelected);
    }

    /// <summary>
    /// Called to desactivate all buttons.
    /// </summary>
    private void DesactivateAllButtons()
    {
        if (_currentEmployeeSelected != null)
        {
            _currentEmployeeSelected.SetRoutineParameter();
        }

        _uiManager.InteractionButtonGroup.HideButtons();
    }
}