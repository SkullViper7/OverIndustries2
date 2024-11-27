using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GridManager _spotChecker;

    [SerializeField] GameObject _panelPrefab;

    List<Panel> _panels;

    // void Start()
    // {
    //     _panels = new List<Panel>();

    //     for (int i = 0; i < _spotChecker.OccupiedSpots.GetLength(0); i++)
    //     {
    //         for (int j = 0; j < _spotChecker.OccupiedSpots.GetLength(1); j++)
    //         {
    //             if (!_spotChecker.OccupiedSpots[i, j])
    //             {
    //                 GameObject panel = Instantiate(_panelPrefab);
    //                 _panels.Add(panel.GetComponent<Panel>());
    //             }
    //         }
    //     }
    // }
}
