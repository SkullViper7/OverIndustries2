using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    // Singleton
    private static InteractionManager _instance = null;
    public static InteractionManager Instance => _instance;

    /// <summary>
    /// Current room selected by the player.
    /// </summary>
    public Room CurrentRoomSelected { get; private set; }

    /// <summary>
    /// Current employee selected by the player.
    /// </summary>
    public Employee CurrentEmployeeSelected { get; private set; }

    /// <summary>
    /// A reference to the input manager.
    /// </summary>
    private InputsManager _inputsManager;

    public event Action<Room> RoomSelected, RoomDoubleTap;

    public event Action<Employee> EmployeeSelected, EmployeeDragAndDrop, EmployeeStartDragAndDrop;

    public event Action RoomUnselected, EmployeeUnselected;

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

    private void Start()
    {
        _inputsManager = InputsManager.Instance;
        InitListeners();
    }

    private void InitListeners()
    {
        _inputsManager.Tap += FindTarget;
        _inputsManager.DoubleTap += FindDoubleTapTarget;
        _inputsManager.DragAndDropPerformed += FindDragAndDropTarget;
        _inputsManager.DragAndDropStarted += FindStartDragAndDropTarget;
    }

    /// <summary>
    /// Casts a ray from the camera to the touch position and triggers interaction with any IInteractable object hit.
    /// </summary>
    private void FindTarget()
    {
        // Get the touch position on the screen and set its z-coordinate to the camera's distance
        Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Calculate the direction from the camera to the touch position in world space
        Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                        Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                        Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

        Debug.DrawRay(Camera.main.transform.position, direction * 100f, Color.green, 1000f);

        RaycastHit hit;
        // Cast a ray from the camera in the calculated direction and check for hits
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 1000))
        {
            // If the ray hits an object with an employee component, trigger its event with datas of the employee
            // or if it's an object with a room component, trigger its event with datas of the room
            if (hit.collider.TryGetComponent<Employee>(out Employee employee))
            {
                if (CurrentEmployeeSelected != employee)
                {
                    if (CurrentRoomSelected != null)
                    {
                        CurrentRoomSelected = null;
                        RoomUnselected?.Invoke();
                    }

                    CurrentEmployeeSelected = employee;
                    employee.IsSelected = true;
                    employee.StopRoutine();

                    EmployeeSelected?.Invoke(employee);
                }
            }
            else if (hit.collider.transform.parent.TryGetComponent<Room>(out Room room))
            {
                if (CurrentRoomSelected != room)
                {
                    if (CurrentEmployeeSelected != null)
                    {
                        employee.IsSelected = false;
                        employee.RandomWayPoint();
                        CurrentEmployeeSelected = null;
                        EmployeeUnselected?.Invoke();
                    }

                    CurrentRoomSelected = room;
                    RoomSelected?.Invoke(room);
                }
            }
            else
            {
                if (CurrentRoomSelected != null)
                {
                    CurrentRoomSelected = null;
                    RoomUnselected?.Invoke();
                }

                if (CurrentEmployeeSelected != null)
                {
                    CurrentEmployeeSelected = null;
                    EmployeeUnselected?.Invoke();
                }
            }
        }
        else
        {
            if (CurrentRoomSelected != null)
            {
                CurrentRoomSelected = null;
                RoomUnselected?.Invoke();
            }

            if (CurrentEmployeeSelected != null)
            {
                CurrentEmployeeSelected = null;
                EmployeeUnselected?.Invoke();
            }
        }
    }

    private void FindStartDragAndDropTarget()
    {
        // Get the touch position on the screen and set its z-coordinate to the camera's distance
        Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Calculate the direction from the camera to the touch position in world space
        Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                        Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                        Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

        RaycastHit hit;
        // Cast a ray from the camera in the calculated direction and check for hits
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 1000))
        {
            // If the ray hits an object with an employee component, trigger its event with datas of the employee
            // or if it's an object with a room component, trigger its event with datas of the room
            if (hit.collider.TryGetComponent<Employee>(out Employee employee))
            {
                if (CurrentRoomSelected != null)
                {
                    CurrentRoomSelected = null;
                    RoomUnselected?.Invoke();
                }

                if (CurrentEmployeeSelected == null)
                {
                    CurrentEmployeeSelected = employee;
                    employee.IsSelected = true;
                    employee.StopRoutine();
                    EmployeeSelected?.Invoke(employee);
                    EmployeeStartDragAndDrop?.Invoke(employee);
                }
            }
            else
            {
                if (CurrentEmployeeSelected != null)
                {
                    employee.IsSelected = false;
                    employee.RandomWayPoint();
                    CurrentEmployeeSelected = null;
                    EmployeeUnselected?.Invoke();
                }
            }
        }
        else
        {
            if (CurrentRoomSelected != null)
            {
                CurrentRoomSelected = null;
                RoomUnselected?.Invoke();
            }

            if (CurrentEmployeeSelected != null)
            {
                CurrentEmployeeSelected.IsSelected = false;
                CurrentEmployeeSelected.RandomWayPoint();
                CurrentEmployeeSelected = null;
                EmployeeUnselected?.Invoke();
            }
        }
    }

    public void FindDragAndDropTarget()
    {
        // Get the touch position on the screen and set its z-coordinate to the camera's distance
        Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Calculate the direction from the camera to the touch position in world space
        Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                        Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                        Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

        RaycastHit hit;
        // Cast a ray from the camera in the calculated direction and check for hits
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 1000))
        {
            // If the ray hits an object with an employee component, trigger its event with datas of the employee
            // or if it's an object with a room component, trigger its event with datas of the room
            if (hit.collider.TryGetComponent<Employee>(out Employee employee))
            {
                if (CurrentEmployeeSelected == employee)
                {
                    if (CurrentRoomSelected != null)
                    {
                        CurrentRoomSelected = null;
                        RoomUnselected?.Invoke();
                    }

                    EmployeeDragAndDrop?.Invoke(employee);
                }
                else
                {
                    if (CurrentRoomSelected != null)
                    {
                        CurrentRoomSelected = null;
                        RoomUnselected?.Invoke();
                    }

                    if (CurrentEmployeeSelected != null)
                    {
                        CurrentEmployeeSelected = null;
                        EmployeeUnselected?.Invoke();
                    }
                }
            }
            else
            {
                if (CurrentRoomSelected != null)
                {
                    CurrentRoomSelected = null;
                    RoomUnselected?.Invoke();
                }

                if (CurrentEmployeeSelected != null)
                {
                    CurrentEmployeeSelected = null;
                    EmployeeUnselected?.Invoke();
                }
            }
        }
        else
        {
            if (CurrentRoomSelected != null)
            {
                CurrentRoomSelected = null;
                RoomUnselected?.Invoke();
            }

            if (CurrentEmployeeSelected != null)
            {
                CurrentEmployeeSelected = null;
                EmployeeUnselected?.Invoke();
            }
        }
    }

    /// <summary>
    /// Called to find a room targeted when a double tap is performed.
    /// </summary>
    private void FindDoubleTapTarget()
    {
        // Get the touch position on the screen and set its z-coordinate to the camera's distance
        Vector3 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // Calculate the direction from the camera to the touch position in world space
        Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x - Camera.main.transform.position.x,
                                        Camera.main.ScreenToWorldPoint(touchPosition).y - Camera.main.transform.position.y,
                                        Camera.main.ScreenToWorldPoint(touchPosition).z - Camera.main.transform.position.z).normalized;

        RaycastHit hit;
        // Cast a ray from the camera in the calculated direction and check for hits
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 1000))
        {
            // If the ray hits an object with a room component, trigger its event with datas of the room
            if (hit.collider.transform.parent.TryGetComponent<Room>(out Room room))
            {
                if (room == CurrentRoomSelected)
                {
                    if (CurrentEmployeeSelected != null)
                    {
                        CurrentEmployeeSelected = null;
                        EmployeeUnselected?.Invoke();
                    }

                    RoomDoubleTap?.Invoke(room);
                }
                else
                {
                    if (CurrentEmployeeSelected != null)
                    {
                        CurrentEmployeeSelected = null;
                        EmployeeUnselected?.Invoke();
                    }

                    CurrentRoomSelected = room;
                    RoomSelected?.Invoke(room);
                }
            }
            else
            {
                if (CurrentRoomSelected != null)
                {
                    CurrentRoomSelected = null;
                    RoomUnselected?.Invoke();
                }
            }
        }
        else
        {
            if (CurrentRoomSelected != null)
            {
                CurrentRoomSelected = null;
                RoomUnselected?.Invoke();
            }

            if (CurrentEmployeeSelected != null)
            {
                CurrentEmployeeSelected = null;
                EmployeeUnselected?.Invoke();
            }
        }
    }
}