using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageButton : MonoBehaviour
{
    /// <summary>
    /// Picto of the component or object.
    /// </summary>
    [SerializeField]
    private Image _picto;

    /// <summary>
    /// Image component of the button.
    /// </summary>
    private Image _image;

    /// <summary>
    /// Button Component.
    /// </summary>
    private Button _button;

    /// <summary>
    /// Datas of the component attached to this button.
    /// </summary>
    private ComponentData _componentData;

    /// <summary>
    /// Datas of the object attached to this button.
    /// </summary>
    private ObjectData _objectData;
}
