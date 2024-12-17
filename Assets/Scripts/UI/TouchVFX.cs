using UnityEngine;

public class TouchVFX : MonoBehaviour
{
    InputsManager _inputsManager;

    GameObject _vfx;

    void Start()
    {
        _inputsManager = InputsManager.Instance;

        _inputsManager.Tap += ActivateVFX;
    }

    void ActivateVFX()
    {
        _vfx.SetActive(true);
    }
}
