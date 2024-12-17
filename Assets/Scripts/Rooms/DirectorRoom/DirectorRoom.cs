using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    // Singleton
    private static DirectorRoom _instance = null;
    public static DirectorRoom Instance => _instance;
    [field: SerializeField] public DirectorRoomData DirectorRoomData { get; private set; }
    [SerializeField] private Room _roomMain;

    [SerializeField, Tooltip("Time to wait before create a new employee")] private int _waitNewEmployee;
    [SerializeField, Tooltip("employee prefab")] private GameObject _employee;

    private JobProfileGenerator _jobProfileGenerator;
    private int _poolEmployeeID;
    private GameObject _newEmployee;
    public List<GameObject> RecrutementList { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        DirectorRoomData = (DirectorRoomData)roomBehaviourData;
        _roomMain = roomMain;
    }
    public void Start()
    {
        _jobProfileGenerator = JobProfileGenerator.Instance;
        _poolEmployeeID = ObjectPoolManager.Instance.NewObjectPool(_employee);
    }

    public void CreateEmployee()
    {
        for (int i = 0; RecrutementList.Count < 5; i++)
        {
            _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(_poolEmployeeID);
            _newEmployee.SetActive(true);
            RecrutementList.Add(_newEmployee);

            _jobProfileGenerator.GenerateProfile(_newEmployee.GetComponent<Employee>());
        }
    }

    /// <summary>
    /// Give the employee to the button 
    /// </summary>
    public void SetHireEmployee(int i)
    {
        HireEmployee(RecrutementList[i].GetComponent<Employee>());
    }

    /// <summary>
    /// Set this new employee
    /// </summary>
    /// <param name="_employeeToHire"></param>
    public void HireEmployee(Employee _employeeToHire)
    {
        _employeeToHire.IsHired = true;
        _employeeToHire.transform.GetChild(0).gameObject.SetActive(true);
        EmployeeList.Instance.AddEmployee(_employeeToHire.gameObject);

        _employeeToHire.AssignRoom = this.gameObject;
        _roomMain.EmployeeAssign.Add(_employeeToHire);

        _employeeToHire.SetEmployee();
    }

    public void RefuseEmployee(Employee _employee)
    {
        ObjectPoolManager.Instance.ReturnObjectToThePool(_poolEmployeeID, _employee.gameObject);
    }

    public void CloseHireEmployee()
    {
        for(int i = 0; i < RecrutementList.Count; i++)
        {
            if (RecrutementList[i].GetComponent<Employee>().IsHired == false)
            {

                Employee employee = RecrutementList[i].GetComponent<Employee>();
                employee.EmployeeJob.Clear();
                employee.gameObject.SetActive(false);
                employee.EmployeeName = null;

                ObjectPoolManager.Instance.ReturnObjectToThePool(_poolEmployeeID, employee.gameObject);
            }
        }
        RecrutementList.Clear();

    }
}
