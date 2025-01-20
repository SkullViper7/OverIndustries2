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
    public bool HasMaintenanceTechnician;

    //Value number of jobs
    //public int PourcentageHasTwoJob {  get; private set; }
    //public int PourcentageHasThreeJob {  get; private set; }

    //Wait create new employee
    private int secondSinceCreateEmployee;

    [field: SerializeField]
    public List<GameObject> RecrutementList { get; private set; } = new List<GameObject>();

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

        RoomMain.NewLvl += UpdateDirectorRoom;
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

    /// <summary>
    /// Instanciate a new employee and call job profile generator
    /// </summary>
    public void CreateEmployee()
    {
        if (EmployeeManager.Instance.Employees.Count != EmployeeManager.Instance.Capacity)
        {
            if (RoomMain.EmployeeAssign.Count < RoomMain.RoomData.Capacity)
            {
                _jobProfileGenerator.ResetForNewOtherProfile();
                _jobProfileGenerator.ResetForNewProductProfile();

                for (int i = 0; RecrutementList.Count < 5; i++)
                {
                    ////generate maintenace techincian
                    //if (EmployeeManager.Instance.Employees.Count > 5 && !HasMaintenanceTechnician)
                    //{
                    //    Debug.Log("maintenance");
                    //    _newEmployee = Instantiate(DirectorRoomData.MaintenanceTechnician);
                    //    RecrutementList.Add(_newEmployee);

                    //    _jobProfileGenerator.GenerateMaintenanceProfile(_newEmployee.GetComponent<Employee>());
                    //}
                    //if (EmployeeManager.Instance.Employees.Count > 12 && HasMaintenanceTechnician)
                    //{
                    //    HasMaintenanceTechnician = false;
                    //}

                    //generate employee for production room
                    if (RecrutementList.Count < 3)
                    {
                        _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(PoolEmployeeID);
                        RecrutementList.Add(_newEmployee);

                        _jobProfileGenerator.GenerateOtherProfile(_newEmployee.GetComponent<Employee>());
                    }

                    //generate all other employee
                    else
                    {
                        _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(PoolEmployeeID);
                        RecrutementList.Add(_newEmployee);

                        _jobProfileGenerator.GenerateProductProfile(_newEmployee.GetComponent<Employee>());
                    }
                }
            }
        }
    }

    public void MaxEmployeeAssign()
    {
        UIManager.Instance.OpenUI();
        UIManager.Instance.AssignEmployeeWarningUI.gameObject.SetActive(true);
        UIManager.Instance.AssignEmployeeWarningUI.ShowWarningMessage(RoomMain);
    }

    public void EmployeeCapacity()
    {
        UIManager.Instance.OpenUI();
        UIManager.Instance.AssignEmployeeWarningUI.gameObject.SetActive(true);
        UIManager.Instance.AssignEmployeeWarningUI.WarningMessage();
    }

    private void UpdateDirectorRoom(int newLvl)
    {
        switch (newLvl)
        {
            case 1:
                //PourcentageHasTwoJob = DirectorRoomData.PourcentageHasTwoJobAtLvl1;
                //PourcentageHasThreeJob = DirectorRoomData.PourcentageHasThreeJobAtLvl1;
                EmployeeManager.Instance.AddCapacity(DirectorRoomData.CapacityBonusAtLvl1);
                break;
            //case 2:
            //    PourcentageHasTwoJob = DirectorRoomData.PourcentageHasTwoJobAtLvl2;
            //    PourcentageHasThreeJob = DirectorRoomData.PourcentageHasThreeJobAtLvl2;
            //    break;
            case 3:
                //PourcentageHasTwoJob = DirectorRoomData.PourcentageHasTwoJobAtLvl3;
                //PourcentageHasThreeJob = DirectorRoomData.PourcentageHasThreeJobAtLvl3;
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}
