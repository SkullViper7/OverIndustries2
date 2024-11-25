using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] GameObject _icon;

    PlayerInput _playerInput;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["TouchContact0"].started += OnTouchContact0;
        _playerInput.actions["Hold0"].performed += OnHold0;
    }

    void OnTouchContact0(InputAction.CallbackContext context)
    {
        _icon.SetActive(true);
    }

    void OnHold0(InputAction.CallbackContext context)
    {
        _icon.transform.position = context.ReadValue<Vector2>();
    }
}
