using UnityEngine;
using UnityEngine.UI;

public class ScrollRectReset : MonoBehaviour
{
    private ScrollRect scrollRect;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    void OnDisable()
    {
        if (scrollRect != null && scrollRect.content != null)
        {
            scrollRect.content.anchoredPosition = Vector2.zero;
        }
    }
}