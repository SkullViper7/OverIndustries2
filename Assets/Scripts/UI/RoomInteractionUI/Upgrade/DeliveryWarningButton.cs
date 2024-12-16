using UnityEngine;
using UnityEngine.UI;

public class DeliveryWarningButton : MonoBehaviour
{
    /// <summary>
    /// Button to validate.
    /// </summary>
    [field : SerializeField]
    public Button ValidateButton { get; private set; }

    private void OnDisable()
    {
        ValidateButton.onClick.RemoveAllListeners();
    }
}
