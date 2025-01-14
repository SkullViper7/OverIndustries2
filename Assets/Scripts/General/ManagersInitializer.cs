using UnityEngine;

public class ManagersInitializer : MonoBehaviour
{
    [SerializeField]
    private PanelManager _panelManager;

    private void Start()
    {
        GridManager.Instance.InitListeners();
        _panelManager.InitListeners();
        LightProbeManager.Instance.InitGrid();
    }
}
