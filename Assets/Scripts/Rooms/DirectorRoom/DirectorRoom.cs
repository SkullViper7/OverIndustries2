using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    // Singleton
    private static DirectorRoom _instance = null;
    public static DirectorRoom Instance => _instance;

    [field: SerializeField] public Room RoomMain { get; private set; }
    [field: SerializeField] public DirectorRoomData DirectorRoomData { get; private set; }

    private JobProfileGenerator _jobProfileGenerator;
    private int _poolEmployeeID;
    private GameObject _newEmployee;
    public List<GameObject> RecrutementList { get; private set; } = new List<GameObject>();

    public event System.Action<Room> MaxAssignEmployee;

    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        DirectorRoomData = (DirectorRoomData)roomBehaviourData;
        RoomMain = roomMain;
    }
    public void Start()
    {
        _jobProfileGenerator = JobProfileGenerator.Instance;
        _poolEmployeeID = ObjectPoolManager.Instance.NewObjectPool(RoomMain.RoomData.Employee);
        CreateEmployee();
    }

    public void CreateEmployee()
    {
        if (RoomMain.EmployeeAssign.Count < RoomMain.RoomData.Capacity)
        {
            for (int i = 0; RecrutementList.Count < 5; i++)
            {
                _newEmployee = ObjectPoolManager.Instance.GetObjectInPool(_poolEmployeeID);
                _newEmployee.SetActive(true);
                RecrutementList.Add(_newEmployee);

                _jobProfileGenerator.GenerateProfile(_newEmployee.GetComponent<Employee>());
            }
        }
        else
        {
            MaxAssignEmployee.Invoke(RoomMain);
        }
    }
}
