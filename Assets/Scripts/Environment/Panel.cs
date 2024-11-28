using UnityEngine;

public class Panel : MonoBehaviour
{
    public Vector2 PanelPosition { get; private set; }

    public void SetPanelPosition(Vector2 position)
    {
        PanelPosition = position;
    }
}
