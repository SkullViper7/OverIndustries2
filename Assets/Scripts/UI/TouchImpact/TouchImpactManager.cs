using UnityEngine;
using UnityEngine.InputSystem;

public class TouchImpactManager : MonoBehaviour
{
    /// <summary>
    /// Prefab of the touch impact.
    /// </summary>
    [SerializeField]
    private GameObject _touchImpact;

    /// <summary>
    /// Transform where touch impacts are spawned.
    /// </summary>
    [SerializeField]
    private Transform _touchImpactContainer;

    /// <summary>
    /// Id of the pool where impacts are stocked.
    /// </summary>
    private int _touchImpactPoolID;

    void Start()
    {
        // Create the pool for buttons
        _touchImpactPoolID = ObjectPoolManager.Instance.NewObjectPool(_touchImpact);

        InputsManager.Instance.TouchScreen += ActivateTouchImpact;
    }

    /// <summary>
    /// Called to spawn a new touch impact.
    /// </summary>
    private void ActivateTouchImpact()
    {
        GameObject touchImpact = ObjectPoolManager.Instance.GetObjectInPool(_touchImpactPoolID);
        touchImpact.transform.SetParent(_touchImpactContainer, false);
        touchImpact.transform.position = Touchscreen.current.primaryTouch.position.ReadValue();
        touchImpact.GetComponent<TouchImpact>().ImpactFinished += DesactivateImpact;
        touchImpact.SetActive(true);
    }

    /// <summary>
    /// Called to return a touch impact when it is used.
    /// </summary>
    /// <param name="touchImpact"> The touch impact to return. </param>
    private void DesactivateImpact(GameObject touchImpact)
    {
        touchImpact.SetActive(false);
        ObjectPoolManager.Instance.ReturnObjectToThePool(_touchImpactPoolID, touchImpact);
    }
}
