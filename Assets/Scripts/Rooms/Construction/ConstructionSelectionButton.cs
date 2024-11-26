using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionSelectionButton : MonoBehaviour
{
    /// <summary>
    /// Prefab of a room without datas.
    /// </summary>
    [SerializeField]
    private GameObject _emptyRoom;

    /// <summary>
    /// Data of the room.
    /// </summary>
    [SerializeField]
    private RoomData _roomData;

    /// <summary>
    /// Data of the room behaviour.
    /// </summary>
    [SerializeField, InterfaceType(typeof(IRoomBehaviourData))]
    private ScriptableObject _roomBehaviourData;

    private Button _selectionButton;

    private void Awake()
    {
        _selectionButton = GetComponent<Button>();
    }

    private void Start()
    {
        _selectionButton.onClick.AddListener(StartSearchingForAnAvailableSpot);
    }

    /// <summary>
    /// Called when button is clicked and start launching a research.
    /// </summary>
    private void StartSearchingForAnAvailableSpot()
    {

    }
}
