using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    [field: SerializeField] public DirectorRoomData DirectorRoomData { get; private set; }
    private Room _roomMain;

    private GameObject _newEmployee;
    //private int _poolID;
    private List<GameObject> _recrutementList = new List<GameObject>();

    [SerializeField, Tooltip("Time to wait before create a new employee")] private int _waitNewEmployee;
    //[SerializeField] private GameObject _defaultEmployee;

    [SerializeField] private JobProfileGenerator _jobProfileGenerator;

    public void Start()
    {
        StartCoroutine(WaitNewEmployee());
        //_poolID = ObjectPoolManager.Instance.NewObjectPool(_defaultEmployee);
    }

    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain)
    {
        DirectorRoomData = (DirectorRoomData)roomBehaviourData;
        _roomMain = roomMain;
        StartCoroutine(WaitNewEmployee());
    }

    public void CreateEmployee()
    {
        if (_recrutementList.Count < 5)
        {
            //_newEmployee = ObjectPoolManager.Instance.GetObjectInPool(_poolID);
            _recrutementList.Add(_newEmployee);
        }

        StartCoroutine(WaitNewEmployee());
    }

    IEnumerator WaitNewEmployee()
    {
        yield return new WaitForSeconds(_waitNewEmployee);
        CreateEmployee();
    }
}
