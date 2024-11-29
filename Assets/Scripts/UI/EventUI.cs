using System.Collections;
using TMPro;
using UnityEngine;

public class EventUI : MonoBehaviour
{
    [Header("Event Appear UI")]
    [SerializeField] private GameObject _popUpEventAppear;
    [SerializeField] private TextMeshProUGUI _eventNameText;
    [SerializeField] private TextMeshProUGUI _eventDescriptionText;

    [Space, SerializeField] private int _popUpDuration;

    //accès demande technicien de maintenace
    //amination warning des salles
    //effet spécifique durant un event ?

    void Start()
    {
        EventManager.Instance.NewEventStart += ShowEventAppear;
    }

    public void ShowEventAppear(EventData _eventData)
    {
        StartCoroutine(PopUpEventTime());

        _eventNameText.text = _eventData.Name;
        _eventDescriptionText.text = _eventData.Description;
        _popUpEventAppear.SetActive(true);
    }

    IEnumerator PopUpEventTime()
    {
        yield return new WaitForSeconds(_popUpDuration);
        _popUpEventAppear.SetActive(false);
    }
}
