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

    private bool _currentEvent;
    private EventData _actualEvent;

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

        for(int i = 0;  i < _eventParent.transform.childCount; i++)
        {
            _eventList.Add(_eventParent.transform.GetChild(i).gameObject);
        }
    }
    public void RandomEvent(int _minute)
    {
        if (!_currentEvent && (_minute - _lastEvent > _timeBetweenEvent) )
        {
            int _eventNumber = Random.Range(0, _eventList.Count);
            _eventList[_eventNumber].gameObject.SetActive(true);
        }
    }

    public void StartEvent(EventData _eventData)
    {
        StartCoroutine(EventDuration(_eventData.Duration));
    }

    IEnumerator EventDuration(int _time)
    {
        yield return new WaitForSeconds(_time);
    }
}
