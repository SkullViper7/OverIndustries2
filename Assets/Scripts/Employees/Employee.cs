using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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
    /// The employee is hired? -> embaucher
    /// </summary>
    public bool IsHired;

    /// <summary>
    /// Room who employee is assign
    /// </summary>
    public GameObject AssignRoom;

    [Space, Header("Routines")]
    [SerializeField] private int _minTimeBetweenWayPoint;
    [SerializeField] private int _maxTimeBetweenWayPoint;

    [Space, Header("Hat color")]
    [SerializeField] private SpriteRenderer _hat;
    [SerializeField] private List<Sprite> _hatList;
    [SerializeField] private List<string> _jobNameList;

    /// <summary>
    /// List of wayPoint who employee can interact and have a animation
    /// </summary>
    public List<GameObject> _wayPointList = new List<GameObject>();

    /// <summary>
    /// reference
    /// </summary>
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private GameObject _actualWayPoint;

    [SerializeField]
    private GameObject _outline;

    private void Start()
    {
        if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            SetEmployee();
            SetRoutineParameter();
            SetHatColor();
        }
    }
    public void SetEmployee()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        StartCoroutine(WaitNewDestination());
    }

    /// <summary>
    /// Set all parameter for start employee routine
    /// </summary>
    public void SetRoutineParameter()
    {
        if (IsHired)
        {
            gameObject.transform.Rotate(Vector3.zero);

            SetIdleAnimation();

            _wayPointList.Clear();
            if (_navMeshAgent.hasPath)
            {
                _navMeshAgent.ResetPath();
            }

            StopCoroutine(WaitNewDestination());

            if (AssignRoom != null)
            {
                _animator.SetBool("Assign", true);

                for (int a = 0; a < AssignRoom.transform.childCount; a++)
                {
                    if (AssignRoom.transform.GetChild(a).tag == "Room" && AssignRoom.transform.GetChild(a).gameObject.activeInHierarchy)
                    {
                        for (int i = 0; i < AssignRoom.transform.GetChild(a).gameObject.transform.childCount; i++)
                        {
                            if (AssignRoom.transform.GetChild(a).GetChild(i).tag == "InteractPoint")
                            {
                                _wayPointList.Add(AssignRoom.transform.GetChild(a).GetChild(i).gameObject);
                            }
                        }
                    }
                }
                RandomWayPoint();
            }
            else
            { _animator.SetBool("Assign", false); }
        }
    }

    /// <summary>
    /// Change hat color for assign room
    /// </summary>
    public void SetHatColor()
    {
        for (int i = 0; i < _jobNameList.Count; i++)
        {
            if (_jobNameList[i] == EmployeeJob[0].JobName)
            {
                _hat.sprite = _hatList[i];
            }
        }
    }

    /// <summary>
    /// Choose random wayPoint and set destination and rotation
    /// </summary>
    public void RandomWayPoint()
    {
        if (!GameManager.Instance.InDragAndDrop && !_navMeshAgent.hasPath && IsHired)
        {
            int i = Random.Range(0, _wayPointList.Count);

            if (_wayPointList[i] != _actualWayPoint || _wayPointList.Count == 0)
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
            _navMeshAgent.ResetPath();
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
            _navMeshAgent.ResetPath();

            _animator.SetTrigger("Interact");
            int i = Random.Range(0, _interactPoint._interactTriggerAnimation.Count);
            _animator.SetTrigger(_interactPoint._interactTriggerAnimation[i]);
        }
        else
        { SetIdleAnimation(); }
    }

    public void SetWalkAnimation()
    {
        if (!GameManager.Instance.InDragAndDrop)
        {
            _animator.SetTrigger("Walk");
        }
        else
        { SetIdleAnimation(); }
    }

    public void SetIdleAnimation()
    {
        _animator.SetTrigger("Idle");
    }

    public void StopRoutine()
    {
        StopCoroutine(WaitNewDestination());
        _navMeshAgent.ResetPath();

        SetIdleAnimation();
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
        if (_wayPointList.Count == 0)
        {
            yield return new WaitForSeconds(2);
            SetRoutineParameter();
        }

        int i = Random.Range(_minTimeBetweenWayPoint, _maxTimeBetweenWayPoint);
        yield return new WaitForSeconds(i);

        RandomWayPoint();
    }

    public void ShowOutline()
    {
        _outline.SetActive(true);
    }

    public void HideOutline()
    {
        _outline.SetActive(false);
    }
}