using UnityEngine;
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
    public event Inputs Tap, Touch0ContactStarted, Touch0ContactCanceled, Touch1ContactStarted, Touch1ContactCanceled, Hold0, Hold1;

    /// <summary>
    /// Events to get some context about inputs.
    /// </summary>
    public delegate void InputsContext(InputAction.CallbackContext context);
    public event InputsContext TapContext, Touch0ContactStartedContext, Touch0ContactCanceledContext, Touch1ContactStartedContext, Touch1ContactCanceledContext, Hold0Context, Hold1Context;

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
        switch (context.action.name)
        {
            case "Tap":
                if (context.performed)
                {
                    Tap?.Invoke();
                    TapContext?.Invoke(context);
                }
                break;

            case "TouchContact0":
                if (context.started)
                {
                    Touch0ContactStarted?.Invoke();
                    Touch0ContactStartedContext?.Invoke(context);
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
                    Touch1ContactStarted?.Invoke();
                    Touch1ContactStartedContext?.Invoke(context);
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
        }
    }
}
