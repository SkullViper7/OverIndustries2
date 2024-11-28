using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private static PanelManager _instance;

    public static PanelManager Instance;

    [SerializeField] GameObject _panel;
    Dictionary<string, Dictionary<string, Panel>> _grid = new();

    const string _rowFormat = "row{0}";
    const string _columnFormat = "column{0}";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitPanel()
    {
        // Create all slots in the dictionnary
        for (int i = 0; i < GridManager.Instance.GridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, Panel>());

            for (int j = 0; j < GridManager.Instance.GridSize.x; j++)
            {
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
            }
        }
    }

    public void AddPanel(Vector2 position)
    {

    }

    public void RemovePanel(Vector2 position)
    {
        
    }
}
