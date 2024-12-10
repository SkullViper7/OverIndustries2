using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Employee : MonoBehaviour
{
    [Header("Employee Stats")]
    /// <summary>
    /// Job of the employee.
    /// </summary>
    public List<JobData> EmployeeJob;

    /// <summary>
    /// Name of the employee.
    /// </summary>
    public string EmployeeName;

    /// <summary>
    /// Room who employee is assign
    /// </summary>
    public GameObject AssignRoom;

    [Space, Header("Routines")]
    [SerializeField] private int _minTimeBetweenWayPoint;
    [SerializeField] private int _maxTimeBetweenWayPoint;

    /// <summary>
    /// List of wayPoint who employee can interact and have a animation
    /// </summary>
    public List<GameObject> _wayPointList = new List<GameObject>();

    /// <summary>
    /// NavMesh reference
    /// </summary>
    private NavMeshAgent _navMeshAgent;

    private GameObject _actualWayPoint;

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SetRoutineParameter();
    }

    public void SetRoutineParameter()
    {
        _wayPointList.Clear();
        _navMeshAgent.ResetPath();
        StopCoroutine(WaitNewDestination());

        for (int i = 0; i < AssignRoom.transform.GetChild(0).gameObject.transform.childCount; i++)
        {
            if (AssignRoom.transform.GetChild(0).GetChild(i).tag == "InteractPoint")
            {
                _wayPointList.Add(AssignRoom.transform.GetChild(0).GetChild(i).gameObject);
            }
        }
        RandomWayPoint();
    }

    /// <summary>
    /// Choose random wayPoint and set destination and rotation
    /// </summary>
    public void RandomWayPoint()
    {
        if (!GameManager.Instance.InDragAndDrop)
        {
            int i = Random.Range(0, _wayPointList.Count);

            if (_wayPointList[i] != _actualWayPoint && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                _actualWayPoint = _wayPointList[i];
                _navMeshAgent.destination = _actualWayPoint.transform.position;

                //Bloque la rotation pendant le déplacement et la garde fixe
                _navMeshAgent.updateRotation = false;
                gameObject.transform.Rotate(Vector3.zero);
                StartCoroutine(WaitNewDestination());
            }
            else
            {
                _navMeshAgent.ResetPath();

                RandomWayPoint();
            }
        }
    }
   
    public IEnumerator WaitNewDestination()
    {
        int i = Random.Range(_minTimeBetweenWayPoint, _maxTimeBetweenWayPoint);
        yield return new WaitForSeconds(i);
        RandomWayPoint();
    }
}
