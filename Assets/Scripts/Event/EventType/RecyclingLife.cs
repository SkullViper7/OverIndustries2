using UnityEngine;

public class RecyclingLife : MonoBehaviour
{
    /// <summary>
    /// Cet event double la production de la salle de recyclage 
    /// </summary>

    [SerializeField] private EventData _eventData;

    void Start()
    {
        EventManager.Instance.CheckCondition(_eventData);
        EventManager.Instance.EventConditionCompleted += EventComportement;
    }

    public void EventComportement()
    {
        Debug.Log("recyling event");
    }
}