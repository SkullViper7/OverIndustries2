using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    // Singleton
    private static DragAndDrop _instance = null;
    public static DragAndDrop Instance => _instance;

    //playerInput reference
    private PlayerInput _playerInput;

    private Employee EmployeeToMove;
    private Vector3 _startPosition;
    public bool EmployeeSelect { get; private set; } = false;

    public event System.Action<Room> RoomAssignIsFull;

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

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["TouchContact0"].started += OnTouchContact0;
        _playerInput.actions["Hold0"].performed += OnHold0;
        _playerInput.actions["TouchContact0"].canceled += OnTouchContact0Canceled;

        InteractionManager.Instance.EmployeeInteraction += GetEmployeeToMove;
    }

    void OnTouchContact0(InputAction.CallbackContext context)
    {

    }

    void OnHold0(InputAction.CallbackContext context)
    {
        if (EmployeeSelect)
        {
            RaycastHit hit;
            GameManager.Instance.InDragAndDrop = true;

            // Get the touch position on the screen and set its z-coordinate to the camera's distance
            Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

            // Calculate the direction from the camera to the touch position in world space
            Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                            Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                            Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

            // Cast a ray from the camera in the calculated direction and check for hits
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 200))
            {
                int PosX = (int)Mathf.Round(hit.point.x);
                int PosY = (int)Mathf.Round(hit.point.y);

                EmployeeToMove.gameObject.transform.position = new Vector3(PosX, PosY, 1);
            }
        }
    }

    /// <summary>
    /// Get the employee select
    /// </summary>
    /// <param name="_employee"></param>
    public void GetEmployeeToMove(Employee _employee)
    {
        _startPosition = _employee.gameObject.transform.position;
        EmployeeToMove = _employee;
    }

    /// <summary>
    /// Set parameter to start move employee select
    /// </summary>
    public void MoveEmployee()
    {
        EmployeeSelect = true;
        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = false;
        EmployeeToMove.SetIdleAnimation();
        GameManager.Instance.InDragAndDrop = true;
    }

    /// <summary>
    /// Quand le joueur relache l'écran
    /// </summary>
    /// <param name="context"></param>
    void OnTouchContact0Canceled(InputAction.CallbackContext context)
    {
        //check si la position est une salle + si elle a de la place pour l'employer, sinon retourne a sa place d'origine
        if (EmployeeSelect)
        {
            EmployeeSelect = false;
            RaycastHit hit;

            // Get the touch position on the screen and set its z-coordinate to the camera's distance
            Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

            // Calculate the direction from the camera to the touch position in world space
            Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                            Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                            Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

            // Cast a ray from the camera in the calculated direction and check for hits
            if (Physics.Raycast(EmployeeToMove.transform.position, direction, out hit, 200))
            {
                //Check if hit room
                if (hit.collider.transform.parent.TryGetComponent<Room>(out Room room))
                {
                    //Check if this room can have a employee
                    if (room.EmployeeAssign.Count != room.RoomData.Capacity)
                    {
                        //Check if this room is not the same room assign to the employee
                        if (room.gameObject != EmployeeToMove.AssignRoom)
                        {
                            //Check if this employee has already a room assign
                            if (EmployeeToMove.AssignRoom != null)
                            {
                                EmployeeToMove.AssignRoom.GetComponent<Room>().RemoveAssignEmployeeInThisRoom(EmployeeToMove);
                            }

                            SetParameter(room);
                        }
                        else
                        { ResetParameter(); }
                    }
                    else
                    { 
                        ResetParameter();
                        RoomAssignIsFull.Invoke(room);
                    }
                }
                else
                { ResetParameter(); }
            }
            else
            { ResetParameter(); }
        }
    }

    public void ResetParameter()
    {
        EmployeeToMove.transform.position = _startPosition;
        GameManager.Instance.InDragAndDrop = false;

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;
        EmployeeToMove.SetRoutineParameter();
    }

    public void SetParameter(Room room)
    {
        GameManager.Instance.InDragAndDrop = false;

        room.AssignEmployeeInThisRoom(EmployeeToMove);
        EmployeeToMove.AssignRoom = room.gameObject;

        EmployeeToMove.transform.position = room.gameObject.transform.GetComponentInChildren<InteractAnimation>().gameObject.transform.position;
        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;

        EmployeeToMove.SetRoutineParameter();
    }
}