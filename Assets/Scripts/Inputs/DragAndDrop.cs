using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    // Singleton
    private static DragAndDrop _instance = null;
    public static DragAndDrop Instance => _instance;

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
        InteractionManager.Instance.EmployeeDragAndDrop += PerformedDragAndDrop;
        InteractionManager.Instance.EmployeeStartDragAndDrop += StartDragAndDrop;
    }

    private void StartDragAndDrop(Employee employee)
    {
        employee.StopRoutine();
    }

    public void PerformedDragAndDrop(Employee employee)
    {
        GameManager.Instance.StartDragAndDrop();
        employee.ShowOutline();

        EmployeeToMove = employee;
        _startPosition = employee.gameObject.transform.position;

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = false;
        EmployeeToMove.SetIdleAnimation();

        InputsManager.Instance.Hold0Context += OnHold0;
        InputsManager.Instance.DragAndDropCanceled += StopDragAndDrop;

    }

    void OnHold0(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        // Get the touch position on the screen and set its z-coordinate to the camera's distance
        Vector3 touchPosition = context.ReadValue<Vector2>();
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

            EmployeeToMove.gameObject.transform.position = new Vector3(PosX, PosY, -2);

            //déplace la caméra suivant la position en Y de l'employer
            if (Camera.main.WorldToScreenPoint(EmployeeToMove.gameObject.transform.position).y >= (Camera.main.scaledPixelHeight / 5 * 4) ||
                Camera.main.WorldToScreenPoint(EmployeeToMove.gameObject.transform.position).y <= (Camera.main.scaledPixelHeight / 5))
            {
                //Camera.main.transform.Translate(Vector3.down * 0.2f, Space.World);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                    new Vector3(Camera.main.transform.position.x, EmployeeToMove.gameObject.transform.position.y, Camera.main.transform.position.z), 0.08f);
            }

            //déplace la caméra suivant la position en X de l'employer
            if (Camera.main.WorldToScreenPoint(EmployeeToMove.gameObject.transform.position).x >= (Camera.main.scaledPixelWidth / 5 * 4) ||
                Camera.main.WorldToScreenPoint(EmployeeToMove.gameObject.transform.position).x <= (Camera.main.scaledPixelWidth / 5))
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                    new Vector3(EmployeeToMove.gameObject.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), 0.05f);
            }
        }

    }

    public void StopDragAndDrop()
    {
        InputsManager.Instance.Hold0Context -= OnHold0;
        InputsManager.Instance.DragAndDropCanceled -= StopDragAndDrop;

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

    public void ResetParameter()
    {
        EmployeeToMove.transform.position = _startPosition;
        GameManager.Instance.StopDragAndDrop();
        EmployeeToMove.HideOutline();

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;
        EmployeeToMove.SetRoutineParameter();
    }

    public void SetParameter(Room room)
    {
        GameManager.Instance.StopDragAndDrop();

        room.AssignEmployeeInThisRoom(EmployeeToMove);
        EmployeeToMove.AssignRoom = room.gameObject;
        EmployeeToMove.HideOutline();

        EmployeeToMove.transform.position = room.gameObject.transform.GetComponentInChildren<InteractAnimation>().gameObject.transform.position;
        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;

        EmployeeToMove.SetRoutineParameter();
    }
}