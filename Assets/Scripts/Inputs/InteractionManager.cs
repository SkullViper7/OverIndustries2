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
    /// A reference to the input manager.
    /// </summary>
    private InputsManager _inputsManager;

    /// <summary>
    /// Events to indicate that there is an interaction with a room.
    /// </summary>
    /// <param name="roomMain"> Main component of the room. </param>
    public delegate void RoomInteractionDelegate(Room roomMain);
    public event RoomInteractionDelegate RoomInteraction;

    /// <summary>
    /// Events to indicate that there is an interaction with an employee.
    /// </summary>
    public delegate void EmployeeInteractionDelegate();
    public event EmployeeInteractionDelegate EmployeeInteraction;

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
    }

    /// <summary>
    /// Casts a ray from the camera to the touch position and triggers interaction with any IInteractable object hit.
    /// </summary>
    private void FindTarget()
    {
        RaycastHit hit;

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
            // If the ray hits an object with a room component, trigger its event with datas of the room
            if (hit.collider.transform.parent.TryGetComponent<Room>(out Room room))
            {
                CurrentRoomSelected = room;
                RoomInteraction?.Invoke(room);
            }
            //If the ray hits an object with a employee component, trigger its event with datas of the employee
            //else if (hit.collider.gameObject.TryGetComponent<RoomTemp>(out RoomTemp room))
            //{
            //  EmployeeInteraction?.Invoke;
            //}
        }
        else
        {

        }
    }
}
