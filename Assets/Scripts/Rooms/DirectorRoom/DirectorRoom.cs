using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorRoom : MonoBehaviour, IRoomBehaviour
{
    public DirectorRoomData DirectorRoomData { get; private set; }
    private Room _roomMain;

    private GameObject _newEmployee;
    public List<GameObject> _recrutementList;

    [SerializeField] private int _waitNewEmployee;

    [SerializeField] private JobProfileGenerator _jobProfileGenerator;

    public void Start()
    {
        StartCoroutine(WaitNewEmployee());
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
            //_newEmployee = ObjectPoolManager.Instance.RequestObject(3);
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
