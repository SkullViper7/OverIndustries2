using System.Collections;
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
    private Button _nameSortButton;

    [SerializeField]
    private Button _jobSortButton;

    [SerializeField]
    private GameObject _employeeButton;

    /// <summary>
    /// List of buttons instantiated.
    /// </summary>
    private List<EmployeeButton> _employeeButtons;
}
