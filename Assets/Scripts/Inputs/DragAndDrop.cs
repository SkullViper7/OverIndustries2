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

    private bool _isDraging;

    private Vector3 _screenPos;

    private bool _isMovingHorizontally;

    private bool _isMovingVertically;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _fixedZ = -0.5f;

    [SerializeField]
    private float _referenceDistance = -6f;

    private Vector3 _employeeOriginalScale;

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
        employee.EmployeeOverlapUI();

        EmployeeToMove = employee;
        _startPosition = employee.gameObject.transform.position;
        _employeeOriginalScale = employee.transform.localScale;

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = false;
        EmployeeToMove.SetIdleAnimation();

        InputsManager.Instance.Hold0Context += IsDraging;
        InputsManager.Instance.DragAndDropCanceled += StopDragAndDrop;

        if (Touchscreen.current.touches.Count > 0)
        {
            OnHold(Touchscreen.current.touches[0].position.ReadValue());
        }
    }

    private void IsDraging(InputAction.CallbackContext context)
    {
        _isDraging = true;
        _screenPos = context.ReadValue<Vector2>();
    }

    void OnHold(Vector3 holdPosition)
    {
        // Get the touch position on the screen (2D vector)
        Vector3 touchPosition = holdPosition;

        // Convert touch position to world space, but we adjust Z manually to keep it constant
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z) + _fixedZ;  // Adjust the Z to avoid perspective distortions

        // Convert screen position to world position with a fixed Z value
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        // Fix the Z-coordinate to the desired constant value
        worldTouchPosition.z = _fixedZ;

        // Move the object to the calculated position
        EmployeeToMove.transform.position = worldTouchPosition;

        // Calculer la distance entre la caméra et l'objet sur l'axe Z
        float distance = Mathf.Abs(Camera.main.transform.position.z - EmployeeToMove.transform.position.z);

        // Calculer le facteur d'échelle : plus la distance est grande, plus le scale augmente
        float scaleFactor = Mathf.Abs(distance / _referenceDistance);

        // Appliquer la nouvelle échelle à l'objet
        EmployeeToMove.transform.localScale = _employeeOriginalScale * scaleFactor;

        //déplace la caméra suivant la position en Y de l'employé
        if (touchPosition.y >= (Camera.main.scaledPixelHeight / 5 * 4) ||
            touchPosition.y <= (Camera.main.scaledPixelHeight / 5))
        {
            _isMovingVertically = true;
        }
        else
        {
            _isMovingVertically = false;
        }

        //déplace la caméra suivant la position en X de l'employé
        if (touchPosition.x >= (Camera.main.scaledPixelWidth / 10 * 9) ||
            touchPosition.x <= (Camera.main.scaledPixelWidth / 10))
        {
            _isMovingHorizontally = true;
        }
        else
        {
            _isMovingHorizontally = false;
        }
    }

    public void StopDragAndDrop()
    {
        _isMovingHorizontally = false;
        _isMovingVertically = false;
        _isDraging = false;
        _screenPos = Vector3.zero;
        InputsManager.Instance.Hold0Context -= IsDraging;
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
        GameManager.Instance.StopDragAndDrop();

        EmployeeToMove.transform.position = _startPosition;
        EmployeeToMove.transform.localScale = _employeeOriginalScale;
        EmployeeToMove.HideOutline();
        EmployeeToMove.EmployeeDoesntOverlapUI();

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;
        EmployeeToMove.SetRoutineParameter();
    }

    public void SetParameter(Room room)
    {
        GameManager.Instance.StopDragAndDrop();

        EmployeeToMove.transform.position = room.gameObject.transform.GetComponentInChildren<InteractAnimation>().gameObject.transform.position;
        EmployeeToMove.transform.localScale = _employeeOriginalScale;
        EmployeeToMove.HideOutline();
        EmployeeToMove.EmployeeDoesntOverlapUI();

        room.AssignEmployeeInThisRoom(EmployeeToMove);
        EmployeeToMove.AssignRoom = room.gameObject;

        EmployeeToMove.GetComponent<NavMeshAgent>().enabled = true;
        EmployeeToMove.SetRoutineParameter();
    }

    private void Update()
    {
        // Calculer un facteur de vitesse basé sur la position Z de la caméra
        float zoomFactor = Mathf.Log10(Mathf.Abs(Camera.main.transform.position.z) + 1);
        float adjustedSpeed = _speed * zoomFactor;

        // Déplacement continu horizontal
        if (_isMovingHorizontally)
        {
            // Déplacer la caméra continuellement dans la direction de l'axe X
            float moveDirection = (EmployeeToMove.gameObject.transform.position.x > Camera.main.transform.position.x) ? 1 : -1;

            // Appliquer le mouvement
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x + moveDirection * adjustedSpeed * Time.deltaTime,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );
        }

        // Déplacement continu vertical
        if (_isMovingVertically)
        {
            // Déplacer la caméra continuellement dans la direction de l'axe Y
            float moveDirection = (EmployeeToMove.gameObject.transform.position.y > Camera.main.transform.position.y) ? 1 : -1;

            // Appliquer le mouvement
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y + moveDirection * adjustedSpeed * Time.deltaTime,
                Camera.main.transform.position.z
            );
        }

        if (_isDraging)
        {
            OnHold(_screenPos);
        }
    }
}