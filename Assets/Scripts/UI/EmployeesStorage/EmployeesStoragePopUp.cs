using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeesStoragePopUp : MonoBehaviour
{
    /// <summary>
    /// The pop up.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    [SerializeField]
    private TMP_Text _capacityTxt;

    [SerializeField]
    private Button _sortButton;

    [SerializeField]
    private RectTransform _sortArrow;

    [SerializeField]
    private Button _nameSortButton;

    [SerializeField]
    private TMP_Text _nameSortButtonTxt;

    [SerializeField]
    private Button _jobSortButton;

    [SerializeField]
    private TMP_Text _jobSortButtonTxt;

    [SerializeField]
    private GameObject _employeeButton;

    [SerializeField]
    private Transform _employeeButonContainer;

    [SerializeField]
    private Sprite _selectedButton;

    [SerializeField]
    private Sprite _unselectedButton;

    /// <summary>
    /// List of buttons instantiated.
    /// </summary>
    private List<EmployeeButton> _employeeButtons = new();

    private bool _isSortedByAscendingOrder = true;

    private bool _isSortingByName = true;

    private void Awake()
    {
        _popUp.SetActive(false);

        _sortButton.onClick.AddListener(SwitchSortingOrder);
        _nameSortButton.onClick.AddListener(SortByNameButtonSelected);
        _jobSortButton.onClick.AddListener(SortByJobButtonSelected);
    }

    private void OnEnable()
    {
        EmployeeManager employeeManager = EmployeeManager.Instance;

        _capacityTxt.text = employeeManager.Employees.Count.ToString() + "/" + employeeManager.Capacity.ToString();

        _isSortedByAscendingOrder = true;
        _sortArrow.rotation = Quaternion.Euler(0, 0, 0);
        _isSortingByName = true;
        EmployeeManager.Instance.SortByName(_isSortedByAscendingOrder);
        _nameSortButton.GetComponent<Image>().sprite = _selectedButton;
        if (ColorUtility.TryParseHtmlString("#606060", out Color newGreyColor))
        {
            _nameSortButtonTxt.color = newGreyColor;
        }
        _jobSortButton.GetComponent<Image>().sprite = _unselectedButton;
        if (ColorUtility.TryParseHtmlString("#4AD8AF", out Color newGreenColor))
        {
            _jobSortButtonTxt.color = newGreenColor;
        }

        LoadPopUp();

        _popUp.SetActive(true);
    }

    private void LoadPopUp()
    {
        EmployeeManager employeeManager = EmployeeManager.Instance;

        for (int i = 0; i < employeeManager.Employees.Count; i++)
        {
            GameObject newEmployeeButton = Instantiate(_employeeButton, _employeeButonContainer);
            _employeeButtons.Add(newEmployeeButton.GetComponent<EmployeeButton>());

            newEmployeeButton.GetComponent<EmployeeButton>().InitButton(employeeManager.Employees[i], i);

            newEmployeeButton.GetComponent<EmployeeButton>().Button.onClick.AddListener(() => FocusEmployee(newEmployeeButton.GetComponent<EmployeeButton>().CurrentEmployee));
        }
    }

    private void UnloadPopUp()
    {
        for (int i = 0; i < _employeeButtons.Count; i++)
        {
            Destroy(_employeeButtons[i].gameObject);
        }

        _employeeButtons.Clear();
    }

    private void OnDisable()
    {
        _popUp.SetActive(false);

        UnloadPopUp();
    }

    private void SwitchSortingOrder()
    {
        _isSortedByAscendingOrder = !_isSortedByAscendingOrder;
        _sortArrow.rotation = _isSortedByAscendingOrder ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180);
        SortEmployees();
    }

    private void SortByNameButtonSelected()
    {
        _isSortingByName = true;
        _nameSortButton.GetComponent<Image>().sprite = _selectedButton;
        if (ColorUtility.TryParseHtmlString("#606060", out Color newGreyColor))
        {
            _nameSortButtonTxt.color = newGreyColor;
        }
        _jobSortButton.GetComponent<Image>().sprite = _unselectedButton;
        if (ColorUtility.TryParseHtmlString("#4AD8AF", out Color newGreenColor))
        {
            _jobSortButtonTxt.color = newGreenColor;
        }
        SortEmployees();
    }

    private void SortByJobButtonSelected()
    {
        _isSortingByName = false;
        _nameSortButton.GetComponent<Image>().sprite = _unselectedButton;
        if (ColorUtility.TryParseHtmlString("#4AD8AF", out Color newGreenColor))
        {
            _nameSortButtonTxt.color = newGreenColor;
        }
        _jobSortButton.GetComponent<Image>().sprite = _selectedButton;
        if (ColorUtility.TryParseHtmlString("#606060", out Color newGreyColor))
        {
            _jobSortButtonTxt.color = newGreyColor;
        }
        SortEmployees();
    }

    /// <summary>
    /// Called to sort employees depending on button configuration.
    /// </summary>
    private void SortEmployees()
    {
        UnloadPopUp();

        if (_isSortingByName)
        {
            EmployeeManager.Instance.SortByName(_isSortedByAscendingOrder);
        }
        else
        {
            EmployeeManager.Instance.SortByJob(_isSortedByAscendingOrder);
        }

        LoadPopUp();
    }

    /// <summary>
    /// Called to focus an employee.
    /// </summary>
    /// <param name="employee"></param>
    private void FocusEmployee(Employee employee)
    {
        InteractionManager.Instance.SelectEmployee(employee);
        NavigationManager.Instance.AutoZoom(employee.AssignRoom.GetComponent<Room>());

        UIManager.Instance.CloseUI();
        gameObject.SetActive(false);
    }
}
