using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    // Singleton
    private static DirectorRoom _instance = null;
    public static DirectorRoom Instance => _instance;

    //Reference
    [field: SerializeField] public Room RoomMain { get; private set; }
    [field: SerializeField] public DirectorRoomData DirectorRoomData { get; private set; }
    private JobProfileGenerator _jobProfileGenerator;

    //Reference for create employee
    public int PoolEmployeeID { get; private set; }
    private GameObject _newEmployee;

    //Wait create new employee
    private int secondSinceCreateEmployee;
    [field: SerializeField] public List<GameObject> RecrutementList { get; private set; } = new List<GameObject>();

    public event System.Action<Room> MaxAssignEmployee;

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
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
        RoomMain = roomMain;
    }

    public void Start()
    {
        _jobProfileGenerator = JobProfileGenerator.Instance;
        PoolEmployeeID = ObjectPoolManager.Instance.NewObjectPool(DirectorRoomData.Employee);
        ChronoManager.Instance.NewSecondTick += WaitCreateEmployee;
        CreateEmployee();
    }

    /// <summary>
    /// call at new second past
    /// </summary>
    public void WaitCreateEmployee()
    {
        secondSinceCreateEmployee++;

        if (secondSinceCreateEmployee >= DirectorRoomData.WaitTime)
        {
            CreateEmployee();
            secondSinceCreateEmployee = 0;
        }
    }

    public void CreateEmployee()
    {
        if (RoomMain.EmployeeAssign.Count < RoomMain.RoomData.Capacity)
        {
            for (int i = 0; RecrutementList.Count < 5; i++)
            {
                _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(PoolEmployeeID);
                RecrutementList.Add(_newEmployee);

                _jobProfileGenerator.GenerateProfile(_newEmployee.GetComponent<Employee>());
            }
        }
    }

    public void MaxEmployeeAssign()
    {
        MaxAssignEmployee.Invoke(RoomMain);
    }
}
