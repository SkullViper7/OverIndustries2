using UnityEngine;
using UnityEngine.InputSystem;

public class NavigationManager : MonoBehaviour
{
    [Header("Scrolling")]
    [SerializeField, Range(1f, 10f)]
    private float _scrollSpeed = 1f;

    [Header("Zoom")]
    [SerializeField]
    private float _zoomSpeed = 0.004f;

    [SerializeField]
    private float _maxZoom = -1.75f;

    [SerializeField]
    private float _minZoom = -15f;

    private float _initialDistance = 0f;

    [Header("Input Positions")]
    private Vector2 _hold0Position;

    private Vector2 _hold0Delta;

    private bool _hold0Performed;

    private Vector2 _hold1Position;

    private Vector2 _hold1Delta;

    private bool _hold1Performed;


    private Vector2 _previousMidpoint = Vector2.zero;

    private Vector2 _currentMidpoint = Vector2.zero;

    private InputsManager _inputManager;

    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main.GetComponent<Camera>();
    }

    void Start()
    {
        InitListeners();
    }

    /// <summary>
    /// Called at the start to init listeners of the inputs.
    /// </summary>
    private void InitListeners()
    {
        _inputManager = InputsManager.Instance;

        // Comportment when first touch is triggered
        _inputManager.Touch0ContactStarted += (() =>
        {
            _hold0Performed = true;
            StartOfATouch();
        });

        // Comportment when first touch is no longer triggered
        _inputManager.Touch0ContactCanceled += (() =>
        {
            _hold0Performed = false;

            _hold0Position = Vector2.zero;
            _hold0Delta = Vector2.zero;

            _initialDistance = 0f;
            _previousMidpoint = Vector2.zero;
            _currentMidpoint = Vector2.zero;
        });

        // Comportment when second touch is triggered
        _inputManager.Touch1ContactStarted += (() =>
        {
            _hold1Performed = true;
            StartOfATouch();
        });

        // Comportment when second touch is no longer triggered
        _inputManager.Touch1ContactCanceled += (() =>
        {
            _hold1Performed = false;

            _hold1Position = Vector2.zero;
            _hold1Delta = Vector2.zero;

            _initialDistance = 0f;
            _previousMidpoint = Vector2.zero;
            _currentMidpoint = Vector2.zero;
        });

        // Comportment when first touch is continuously triggered
        _inputManager.Hold0Context += ((context) =>
        {
            if (_hold0Position == Vector2.zero)
            {
                _hold0Position = context.ReadValue<Vector2>();
            }

            _hold0Delta = context.ReadValue<Vector2>() - _hold0Position;
            _hold0Position = context.ReadValue<Vector2>();

            if (!_hold1Performed)
            {
                Scroll(_hold0Delta);
            }
        });

        // Comportment when second touch is continuously triggered
        _inputManager.Hold1Context += ((context) =>
        {
            if (_hold1Position == Vector2.zero)
            {
                _hold1Position = context.ReadValue<Vector2>();
            }

            _hold1Delta = context.ReadValue<Vector2>() - _hold1Position;
            _hold1Position = context.ReadValue<Vector2>();

            if (!_hold0Performed)
            {
                Scroll(_hold1Delta);
            }
        });
    }

    /// <summary>
    /// Called at the start of a touch to calculate some initiale parameters for next actions.
    /// </summary>
    private void StartOfATouch()
    {
        Vector2 touch0Position = new();
        Vector2 touch1Position = new();

        // Get positions of the two touches if there is
        if (Touchscreen.current.touches.Count > 0)
        {
            touch0Position = Touchscreen.current.touches[0].position.ReadValue();
            _hold0Position = touch0Position;
        }
        if (Touchscreen.current.touches.Count > 1)
        {
            touch1Position = Touchscreen.current.touches[1].position.ReadValue();
            _hold1Position = touch1Position;
        }

        if (_hold0Performed && _hold1Performed)
        {
            // Calculate initial distance between the two points
            _initialDistance = (touch1Position - touch0Position).magnitude;

            // Calculate the midpoint between the two points
            _previousMidpoint = new Vector2((touch0Position.x + touch1Position.x) / 2, (touch0Position.y + touch1Position.y) / 2);
        }
    }

    /// <summary>
    /// Continuously called while the player holds a touch on the screen to scroll.
    /// </summary>
    private void Scroll(Vector2 delta)
    {
        if (!GameManager.Instance.InDragAndDrop)
        {
            // Utilise la distance entre la caméra et le plan de déplacement
            float distance = Mathf.Abs(_camera.transform.position.z);

            // Applique une fonction exponentielle pour ajuster le facteur d'échelle
            float scaleFactor = Mathf.Pow(distance, 0.5f) * 2f / Screen.height; // Racine carrée pour rendre le scroll plus sensible à faible distance

            // Applique le delta pour déplacer la caméra en suivant le mouvement du doigt
            Vector3 cameraDelta = new Vector3(delta.x * scaleFactor, delta.y * scaleFactor, 0) * _scrollSpeed;
            _camera.transform.position -= _camera.transform.TransformDirection(cameraDelta);
        }
    }

    /// <summary>
    /// Continuously called while the player holds 2 touches on the screen to zoom.
    /// </summary>
    private void Zoom()
    {
        float pinchDelta = (_hold0Position - _hold1Position).magnitude;

        if (_initialDistance == 0)
        {
            _initialDistance = pinchDelta;
            return;
        }

        float distanceDelta = pinchDelta - _initialDistance;

        // Utilise directement la valeur de la position de la caméra comme facteur
        float distanceScaleFactor = 1f / Mathf.Max(0.1f, Mathf.Abs(_camera.transform.localPosition.z));

        // Facteur de mise à l'échelle basé sur la taille de l'écran (diagonale en pixels)
        float screenScaleFactor = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 1000f;

        // Applique les deux facteurs dans le calcul du zoom
        Vector3 newPosition = _camera.transform.position;

        newPosition.z += distanceDelta * _zoomSpeed / (distanceScaleFactor * screenScaleFactor);

        newPosition.z = Mathf.Clamp(newPosition.z, _minZoom, _maxZoom);

        _camera.transform.localPosition = newPosition;

        _initialDistance = pinchDelta;
    }

    private void Update()
    {
        if (_hold0Performed && _hold1Performed)
        {
            Zoom();

            _currentMidpoint = new Vector2((_hold0Position.x + _hold1Position.x) / 2, (_hold0Position.y + _hold1Position.y) / 2);
            Vector2 delta = _currentMidpoint - _previousMidpoint;
            _previousMidpoint = _currentMidpoint;
            Scroll(delta);
        }
    }
}