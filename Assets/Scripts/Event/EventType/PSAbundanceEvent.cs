using UnityEngine;

public class PSAbundanceEvent : MonoBehaviour
{
    /// <summary>
    /// Cet event multiplie les PS gagner par le joueurs lorsqu'il termine une commande. (durée limité)
    /// </summary>

    [SerializeField] private EventData _eventData;

    void Start()
    {
        EventManager.Instance.CheckCondition(_eventData);
        EventManager.Instance.EventConditionCompleted += EventComportement;
    }

    public void EventComportement()
    {
        
    }
}
