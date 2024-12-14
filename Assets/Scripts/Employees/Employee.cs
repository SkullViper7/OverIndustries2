using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> _wayPointList = new List<GameObject>();

    /// <summary>
    /// reference
    /// </summary>
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private GameObject _actualWayPoint;

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        SetRoutineParameter();
    }

    /// <summary>
    /// Set all parameter for start employee routine
    /// </summary>
    public void SetRoutineParameter()
    {
        gameObject.transform.Rotate(Vector3.zero);
        SetIdleAnimation();

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
        if (!GameManager.Instance.InDragAndDrop && !_navMeshAgent.hasPath)
        {
            int i = Random.Range(0, _wayPointList.Count);

            if (_wayPointList[i] != _actualWayPoint)
            {
                _actualWayPoint = _wayPointList[i];
                SetWalkAnimation();

                _navMeshAgent.destination = _actualWayPoint.transform.position;

                //Bloque la rotation pendant le déplacement
                _navMeshAgent.updateRotation = false;
                SetRotation();
                StartCoroutine(WaitNewDestination());
            }
            else
            {
                _navMeshAgent.ResetPath();
                RandomWayPoint();
            }
        }
        else
        {
            SetIdleAnimation();
        }
    }

    /// <summary>
    /// Change employee rotation when his walk
    /// </summary>
    public void SetRotation()
    {
        if (_actualWayPoint.transform.position.x >= gameObject.transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (gameObject.transform.rotation.y == 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    /// <summary>
    /// Choose interact animation with the actual interact point
    /// </summary>
    /// <param name="_interactPoint"></param>
    public void SetIntercatAnimation(InteractAnimation _interactPoint)
    {
        if (!GameManager.Instance.InDragAndDrop)
        {
            gameObject.transform.rotation = _interactPoint.transform.rotation;

            _animator.SetTrigger("Interact");
            int i = Random.Range(0, _interactPoint._interactTriggerAnimation.Count);
            _animator.SetTrigger(_interactPoint._interactTriggerAnimation[i]);
        }
        else
        {
            SetIdleAnimation();
        }
    }

    public void SetWalkAnimation()
    {
        if (!GameManager.Instance.InDragAndDrop)
        {
            _animator.SetTrigger("Walk");
        }
        else
        {
            SetIdleAnimation();
        }
    }

    public void SetIdleAnimation()
    {
        _animator.SetTrigger("Idle");
    }

    /// <summary>
    /// Check if the employee is at interact point
    /// </summary>
    /// <param name="collider"></param>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == _actualWayPoint.name)
        {
            SetIntercatAnimation(_actualWayPoint.GetComponent<InteractAnimation>());
        }
    }

    public IEnumerator WaitNewDestination()
    {
        int i = Random.Range(_minTimeBetweenWayPoint, _maxTimeBetweenWayPoint);

        yield return new WaitForSeconds(i);
        RandomWayPoint();
    }
}