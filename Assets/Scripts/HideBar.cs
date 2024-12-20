using UnityEditor;
using UnityEngine;

public class HideBar : MonoBehaviour
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UnityEngine.iOS.Device.hideHomeButton = true;
        }
    }
}
