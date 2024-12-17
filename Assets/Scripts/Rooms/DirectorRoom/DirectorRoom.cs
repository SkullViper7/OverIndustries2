using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    // Singleton
    private static DirectorRoom _instance = null;
    public static DirectorRoom Instance => _instance;
    [field: SerializeField] public DirectorRoomData DirectorRoomData { get; private set; }
    private Room _roomMain;

    [SerializeField, Tooltip("Time to wait before create a new employee")] private int _waitNewEmployee;
    [SerializeField, Tooltip("employee prefab")] private GameObject _employee;

    private JobProfileGenerator _jobProfileGenerator;
    private int _poolEmployeeID;
    private GameObject _newEmployee;
    public List<GameObject> RecrutementList { get; private set; } = new List<GameObject>();

    public event System.Action NewRecrutement;

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

        CreateEmployee();
    }

    public void CreateEmployee()
    {
        for (int i = 0; RecrutementList.Count < 5; i++)
        {
            _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(_poolEmployeeID);
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

    public void HireEmployee(Employee _employeeToHire)
    {
        _employeeToHire.IsHired = true;
    }

    public void RefuseEmployee(Employee _employee)
    {
        ObjectPoolManager.Instance.ReturnObjectToThePool(_poolEmployeeID, _employee.gameObject);
    }
}
