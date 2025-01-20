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
    /// Time Difference between 2 event
    /// </summary>
    [SerializeField] private int _timeEnterEvent;
    private int _lastEvent;

    /// <summary>
    /// List of all event
    /// </summary>
    [SerializeField] private List<GameObject> _eventList;

    public EventData ActualEvent { get; private set; }
    public bool CurrentEvent { get; private set; }
    private int _eventNumber;

    public event System.Action<EventData> NewEventStart;
    public event System.Action EventConditionCompleted;
    public event System.Action<EventData> CurrentEventIsFinish;

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
    }

    /// <summary>
    /// Choose random event tout les minutes
    /// </summary>
    /// <param name="_minute"></param>
    public void RandomEvent(int _minute)
    {
        //if (!CurrentEvent && (_minute - _lastEvent > _timeEnterEvent))
        //{
        //    _eventNumber = Random.Range(0, _eventList.Count);
        //    _eventList[_eventNumber].gameObject.SetActive(true);
        //}

        if (!CurrentEvent && _minute <= 15)
        {
            _eventNumber = Random.Range(0, _eventList.Count);
            _eventList[_eventNumber].gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Check if event condition are completed
    /// </summary>
    /// <param name="_eventData"></param>
    public void CheckCondition(EventData _eventData)
    {
        if (_eventData.Condition)
        {
            int _room = 0;
            bool _goodRoomType = false;
            bool _canCreatQuest = false;

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

            //Check if this event need to create a quest
            if (_eventData.CanCreateQuest)
            {
                if (QuestManager.Instance.CurrentQuestList.Count < QuestManager.Instance.MaxCurrentQuest)
                {
                    _canCreatQuest = true;
                }
            }

            //Check if all conditions are completed and start event
            if (_room >= _eventData.NumberOfRoom && _goodRoomType && _eventData.MinNumberPS <= ScoreManager.Instance.TotalPS)
            {
                StartEvent(_eventData);
            }
            //Condition : number of room + no room type need + minimum number PS + event need to create quest + can create quest
            else if (_room >= _eventData.NumberOfRoom && !_eventData.SpecialRoomType && _eventData.MinNumberPS <= ScoreManager.Instance.TotalPS && _eventData.CanCreateQuest && _canCreatQuest)
            {
                StartEvent(_eventData);
            }
            //Condition : number of room + no room type need + minimum number PS + event don't need to create quest
            else if (_room >= _eventData.NumberOfRoom && !_eventData.SpecialRoomType && _eventData.MinNumberPS <= ScoreManager.Instance.TotalPS && !_eventData.CanCreateQuest)
            {
                StartEvent(_eventData);
            }
            else //All conditions are not completed
            {
                _eventList[_eventNumber].SetActive(false);
            }
        }
        else
        {
            StartEvent(_eventData);
        }
    }

    /// <summary>
    /// All condition are completed, start the event choose
    /// </summary>
    /// <param name="_eventData"></param>
    public void StartEvent(EventData _eventData)
    {
        Debug.Log("Event Start");

        CurrentEvent = true;
        ActualEvent = _eventData;

        NewEventStart.Invoke(_eventData);
        EventConditionCompleted.Invoke();

        StartCoroutine(EventDuration(_eventData.Duration));
    }

    /// <summary>
    /// Current event is finish, reset parameter
    /// </summary>
    public void CloseCurrentEvent()
    {
        CurrentEventIsFinish.Invoke(ActualEvent);

        _eventList[_eventNumber].SetActive(false);
        CurrentEvent = false;

        Debug.Log("Current event finish");
    }

    /// <summary>
    /// Duration of current event
    /// </summary>
    /// <param name="_time"></param>
    IEnumerator EventDuration(int _time)
    {
        yield return new WaitForSeconds(_time);
        CloseCurrentEvent();
    }
}
