using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Singleton
    private static EventManager _instance = null;
    public static EventManager Instance => _instance;

    /// <summary>
    /// Reference
    /// </summary>
    private ScoreManager _scoreManager;
    private ChronoManager _chronoManager;

    /// <summary>
    /// Get all event
    /// </summary>
    [SerializeField] private GameObject _eventParent;

    /// <summary>
    /// Time Difference between 2 event
    /// </summary>
    [SerializeField] private int _timeBetweenEvent;
    [SerializeField] private int _lastEvent;

    /// <summary>
    /// List of all event
    /// </summary>
    [field: SerializeField] private List<GameObject> _eventList = new List<GameObject>();

    public EventData ActualEvent { get; private set; }
    public bool CurrentEvent { get; private set; }
    private int _eventNumber;

    public event System.Action EventConditionCompleted;
    public event System.Action CurrentEventIsFinish;

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

    public void Start()
    {
        ChronoManager.Instance.NewMinute += RandomEvent;

        for (int i = 0; i < _eventParent.transform.childCount; i++)
        {
            _eventList.Add(_eventParent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Choose random event tout les x minutes suivant le chrono
    /// </summary>
    /// <param name="_minute"></param>
    public void RandomEvent(int _minute)
    {
        if (!CurrentEvent && (_minute - _lastEvent > _timeBetweenEvent))
        {
            _eventNumber = Random.Range(0, _eventList.Count);
            _eventList[_eventNumber].gameObject.SetActive(true);
        }
    }

    public void StartEvent(EventData _eventData)
    {
        CurrentEvent = true;
        ActualEvent = _eventData;
        EventConditionCompleted.Invoke();

        StartCoroutine(EventDuration(_eventData.Duration));
    }

    public void CheckCondition(EventData _eventData)
    {
        if (_eventData.Condition)
        {
            int _room = 0;
            bool _goodRoomType = false;

            //Check number of room with level >= min room level for start this event
            for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
            {
                if (GridManager.Instance.InstantiatedRooms[i].CurrentLvl > _eventData.MinRoomLevel)
                {
                    _room++;
                }
            }

            //Check if a special room type is necessary and check if this room type are instanciates
            if (_eventData.SpecialRoomType)
            {
                for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
                {
                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == _eventData.RoomTypeNeed)
                    {
                        _goodRoomType = true;
                    }
                }
            }

            //Check if all conditions are completed and start event
            if (_room >= _eventData.NumberOfRoom && _goodRoomType && _eventData.MinNumberPS <= ScoreManager.Instance.TotalPS)
            {
                StartEvent(_eventData);
            }
            else if (_room >= _eventData.NumberOfRoom && !_eventData.SpecialRoomType && _eventData.MinNumberPS <= ScoreManager.Instance.TotalPS)
            {
                StartEvent(_eventData);
            }
            else //All conditions are uncompleted
            {
                _eventList[_eventNumber].SetActive(false);
            }
        }
        else
        {
            StartEvent(_eventData);
        }
    }

    public void CloseCurrentEvent()
    {
        CurrentEventIsFinish.Invoke();

        _eventList[_eventNumber].SetActive(false);
        CurrentEvent = false;
    }

    IEnumerator EventDuration(int _time)
    {
        yield return new WaitForSeconds(_time);
        CloseCurrentEvent();
    }
}
