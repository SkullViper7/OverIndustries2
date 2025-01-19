using DG.Tweening;
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

    private float _maxZoom = -1.75f;

    [SerializeField]
    private float _minZoom = -15f;

    [SerializeField]
    private Vector2 _minPosLeftCorner, _maxPosRightCorner;

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

    private Sequence _zoomOrZoomOutSequence;

    private bool _isZoomedOnARoom;

    void Awake()
    {
        _camera = Camera.main.GetComponent<Camera>();
    }

    void Start()
    {
        _maxZoom = CalculateCameraMaxZoom();

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

            if (!_hold1Performed && _hold0Performed)
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

            if (!_hold0Performed && _hold1Performed)
            {
                Scroll(_hold1Delta);
            }
        });

        // Comportment when a double tap on a room is triggered
        InteractionManager.Instance.RoomDoubleTap += AutoZoomOrZoomOut;
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
            // Utilise la distance entre la cam�ra et le plan de d�placement
            float distance = Mathf.Abs(_camera.transform.position.z);

            // Applique une fonction exponentielle pour ajuster le facteur d'�chelle
            float scaleFactor = Mathf.Pow(distance, 0.5f) * 2f / Screen.height; // Racine carr�e pour rendre le scroll plus sensible � faible distance

            // Applique le delta pour d�placer la cam�ra en suivant le mouvement du doigt
            Vector3 cameraDelta = new Vector3(delta.x * scaleFactor, delta.y * scaleFactor, 0) * _scrollSpeed;
            Vector3 newCameraPosition = _camera.transform.position - _camera.transform.TransformDirection(cameraDelta);

            // Vérifie si la différence entre l'ancienne et la nouvelle position dépasse un seuil
            const float threshold = 0.05f; // Seuil minimum (ajuste cette valeur en fonction de tes besoins)
            if (Vector3.Distance(_camera.transform.position, newCameraPosition) > threshold)
            {
                _isZoomedOnARoom = false;
                CancelSequence();
            }

            _camera.transform.position -= _camera.transform.TransformDirection(cameraDelta);

            Vector3 clampedPosition = _camera.transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, _minPosLeftCorner.x, _maxPosRightCorner.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, _minPosLeftCorner.y, _maxPosRightCorner.y);
            _camera.transform.position = clampedPosition;
        }
    }

    /// <summary>
    /// Continuously called while the player holds 2 touches on the screen to zoom.
    /// </summary>
    private void Zoom()
    {
        CancelSequence();
        _isZoomedOnARoom = false;

        float pinchDelta = (_hold0Position - _hold1Position).magnitude;

        if (_initialDistance == 0)
        {
            _initialDistance = pinchDelta;
            return;
        }

        float distanceDelta = pinchDelta - _initialDistance;

        // Utilise directement la valeur de la position de la cam�ra comme facteur
        float distanceScaleFactor = 1f / Mathf.Max(0.1f, Mathf.Abs(_camera.transform.position.z));

        // Facteur de mise � l'�chelle bas� sur la taille de l'�cran (diagonale en pixels)
        float screenScaleFactor = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / 1000f;

        // Applique les deux facteurs dans le calcul du zoom
        Vector3 newPosition = _camera.transform.position;

        newPosition.z += distanceDelta * _zoomSpeed / (distanceScaleFactor * screenScaleFactor);

        newPosition.z = Mathf.Clamp(newPosition.z, _minZoom, _maxZoom);

        _camera.transform.position = newPosition;

        _initialDistance = pinchDelta;
    }

    /// <summary>
    /// Called to launch a zoom or a zoom out on a room.
    /// </summary>
    /// <param name="room"> The room selected. </param>
    private void AutoZoomOrZoomOut(Room room)
    {
        if (_zoomOrZoomOutSequence == null)
        {
            // Calculate cente of the room
            Vector2 centerPosition = new Vector2(room.transform.position.x + ((room.RoomData.Size * 3) / 2), room.transform.position.y + 2);

            // If camera is already zoom, zoom out of the room
            if (_camera.transform.position.z == _maxZoom || _isZoomedOnARoom)
            {
                Vector3 zoomOutPosition = new Vector3(centerPosition.x, centerPosition.y, _minZoom);

                _zoomOrZoomOutSequence = DOTween.Sequence();
                _zoomOrZoomOutSequence.Append(_camera.transform.DOMove(zoomOutPosition, 0.5f)).SetEase(Ease.InExpo).OnComplete(() =>
                {
                    _isZoomedOnARoom = false;
                    CancelSequence();
                });
            }
            // Else, zoom on the room
            else
            {
                if (room.RoomData.RoomType == RoomType.Elevator)
                {
                    float maxTime = 0.5f;
                    Vector3 maxZoomPosition = new Vector3(centerPosition.x, centerPosition.y, _minZoom);
                    Vector3 zoomPosition = new Vector3(centerPosition.x, centerPosition.y, _maxZoom);

                    // Calculate proportional time of the sequence
                    float time = ((zoomPosition - _camera.transform.position).magnitude * maxTime) / ((zoomPosition - maxZoomPosition).magnitude);
                    time = Mathf.Clamp(time, 0f, maxTime);

                    _zoomOrZoomOutSequence = DOTween.Sequence();
                    _zoomOrZoomOutSequence.Append(_camera.transform.DOMove(zoomPosition, time)).SetEase(Ease.InExpo).OnComplete(() =>
                    {
                        _isZoomedOnARoom = true;
                        CancelSequence();
                    });
                }
                else
                {
                    float maxTime = 0.5f;
                    Vector3 maxZoomPosition = new Vector3(centerPosition.x, centerPosition.y, _minZoom);
                    Vector3 zoomPosition = new Vector3(centerPosition.x, centerPosition.y, CalculateCameraZPositionForWidth(room.RoomData.Size));

                    // Calculate proportional time of the sequence
                    float time = ((zoomPosition - _camera.transform.position).magnitude * maxTime) / ((zoomPosition - maxZoomPosition).magnitude);
                    time = Mathf.Clamp(time, 0f, maxTime);

                    _zoomOrZoomOutSequence = DOTween.Sequence();
                    _zoomOrZoomOutSequence.Append(_camera.transform.DOMove(zoomPosition, time)).SetEase(Ease.InExpo).OnComplete(() =>
                    {
                        _isZoomedOnARoom = true;
                        CancelSequence();
                    });
                }
            }
        }
    }

    /// <summary>
    /// Called to calculate the max zoom of th camera based on the elevator height.
    /// </summary>
    /// <returns></returns>
    private float CalculateCameraMaxZoom()
    {
        float elevatorHeight = 4f;

        // Camera FOV in radians
        float fovRad = _camera.fieldOfView * Mathf.Deg2Rad;

        // Calculate the distance between the distance in z so that the height of the object matches the screen height
        float distanceZ = (elevatorHeight / 2) / Mathf.Tan(fovRad / 2);

        // Return the z position
        return -distanceZ;
    }


    private float CalculateCameraZPositionForWidth(int roomSize)
    {
        float objectWidth = roomSize * 3;

        // Camera FOV in radians
        float fovRad = _camera.fieldOfView * Mathf.Deg2Rad;

        // Screen aspect ratio
        float aspectRatio = _camera.aspect;

        // Calculate the distance between the distance in z so that the width of the object matches the screen width
        float distanceZ = (objectWidth / 2) / (Mathf.Tan(fovRad / 2) * aspectRatio);

        // Return the z position
        return -distanceZ;
    }

    /// <summary>
    /// Call to cancel a sequence if there is one.
    /// </summary>
    private void CancelSequence()
    {
        if (_zoomOrZoomOutSequence != null)
        {
            _zoomOrZoomOutSequence.Kill();
            _zoomOrZoomOutSequence = null;
        }
    }

    private void Update()
    {
        Vector3 clampedPosition = _camera.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _minPosLeftCorner.x, _maxPosRightCorner.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, _minPosLeftCorner.y, _maxPosRightCorner.y);
        _camera.transform.position = clampedPosition;

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