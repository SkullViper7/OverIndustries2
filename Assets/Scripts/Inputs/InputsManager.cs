using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    // Singleton
    private static InputsManager _instance = null;
    public static InputsManager Instance => _instance;

    /// <summary>
    /// Player input component.
    /// </summary>
    private PlayerInput _playerInput;

    /// <summary>
    /// Events to get some inputs.
    /// </summary>
    public delegate void Inputs();
    public event Inputs TouchScreen, Tap, DragAndDropStarted, DragAndDropPerformed, DragAndDropCanceled, DoubleTap, Touch0ContactStarted, Touch0ContactCanceled, Touch1ContactStarted, Touch1ContactCanceled, Hold0, Hold1;

    /// <summary>
    /// Events to get some context about inputs.
    /// </summary>
    public delegate void InputsContext(InputAction.CallbackContext context);
    public event InputsContext TapContext, DoubleTapContext, Touch0ContactStartedContext, Touch0ContactCanceledContext, Touch1ContactStartedContext, Touch1ContactCanceledContext, Hold0Context, Hold1Context;

    void Awake()
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

        _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        _playerInput.onActionTriggered += OnAction;
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        if (context.action.name == "TouchScreen")
        {
            if (context.started)
            {
                TouchScreen?.Invoke();
            }
        }

        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.UIIsOpen)
            {
                switch (context.action.name)
                {
                    case "Tap":
                        if (context.performed)
                        {
                            if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
                            {
                                Tap?.Invoke();
                                TapContext?.Invoke(context);
                            }
                        }
                        break;

                    case "DoubleTap":
                        if (context.performed)
                        {
                            if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
                            {
                                DoubleTap?.Invoke();
                                DoubleTapContext?.Invoke(context);
                            }
                        }
                        break;

                    case "TouchContact0":
                        if (context.started)
                        {
                            if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
                            {
                                Touch0ContactStarted?.Invoke();
                                Touch0ContactStartedContext?.Invoke(context);
                            }
                        }
                        if (context.canceled)
                        {
                            Touch0ContactCanceled?.Invoke();
                            Touch0ContactCanceledContext?.Invoke(context);
                        }
                        break;

                    case "TouchContact1":
                        if (context.started)
                        {
                            if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
                            {
                                Touch1ContactStarted?.Invoke();
                                Touch1ContactStartedContext?.Invoke(context);
                            }
                        }
                        if (context.canceled)
                        {
                            Touch1ContactCanceled?.Invoke();
                            Touch1ContactCanceledContext?.Invoke(context);
                        }
                        break;

                    case "Hold0":
                        if (context.performed)
                        {
                            Hold0?.Invoke();
                            Hold0Context?.Invoke(context);
                        }
                        break;

                    case "Hold1":
                        if (context.performed)
                        {
                            Hold1?.Invoke();
                            Hold1Context?.Invoke(context);
                        }
                        break;

                    case "DragAndDrop0":
                        if (context.started)
                        {
                            if (!IsPointerOverUI(Touchscreen.current.primaryTouch.position.ReadValue()))
                            {
                                DragAndDropStarted?.Invoke();
                            }
                        }
                        else if (context.performed)
                        {
                            DragAndDropPerformed?.Invoke();
                        }
                        else if (context.canceled)
                        {
                            DragAndDropCanceled?.Invoke();
                        }
                        break;
                }
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
