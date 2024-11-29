using UnityEngine;

public class BigQuestEvent : MonoBehaviour
{
    /// <summary>
    /// Cet event invoque une grosse commande, si le joueur la complète avant la fin du temps imparti, 
    /// il a un boost de PS si il dépasse le temps donné, l'event devient une quête normal
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
