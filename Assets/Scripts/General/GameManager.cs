using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    /// <summary>
    /// Gets if the game is in pause.
    /// </summary>
    public bool GameIsInPause { get; private set; }

    /// <summary>
    /// Gets if a UI is opened.
    /// </summary>
    [field : SerializeField]
    public bool UIIsOpen { get; private set; }

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public void OpenUI()
    {
        UIIsOpen = true;
    }

    public void CloseUI()
    {
        UIIsOpen = false;
    }
}
