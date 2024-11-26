using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    // Singleton
    private static InteractionManager _instance = null;
    public static InteractionManager Instance => _instance;

    /// <summary>
    /// Current room selected by the player.
    /// </summary>
    public RoomTemp CurrentRoomSelected { get; private set; }

    /// <summary>
    /// A reference to the input manager.
    /// </summary>
    private InputsManager _inputsManager;

    /// <summary>
    /// Events to indicate that there is an interaction with a room.
    /// </summary>
    /// <param name="roomMain"> Main component of the room. </param>
    public delegate void RoomInteractionDelegate(RoomTemp roomMain);
    public event RoomInteractionDelegate RoomInteraction;

    /// <summary>
    /// Events to indicate that there is an interaction with an employee.
    /// </summary>
    public delegate void EmployeeInteractionDelegate();
    public event EmployeeInteractionDelegate EmployeeInteraction;

    /// <summary>
    /// Events to indicate that there is no interaction.
    /// </summary>
    public delegate void NoInteractionDelegate();
    public event NoInteractionDelegate NoInteraction;

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
        if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
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
                if (hit.collider.transform.parent.TryGetComponent<RoomTemp>(out RoomTemp room))
                {
                    CurrentRoomSelected = room;
                    RoomInteraction?.Invoke(room);
                }
                else
                {
                    NoInteraction?.Invoke();
                }
                //If the ray hits an object with a employee component, trigger its event with datas of the employee
                //else if (hit.collider.gameObject.TryGetComponent<RoomTemp>(out RoomTemp room))
                //{
                //  EmployeeInteraction?.Invoke;
                //}
            }
            else
            {
                NoInteraction?.Invoke();
            }
        }
    }

    /// <summary>
    /// Called to checks if the start of the input is not over a UI element.
    /// </summary>
    /// <param name="screenPosition"> Position of the start of the input. </param>
    /// <returns></returns>
    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = screenPosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults.Count > 0;
    }
}
