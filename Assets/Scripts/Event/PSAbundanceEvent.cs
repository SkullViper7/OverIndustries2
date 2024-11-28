using UnityEngine;

public class PSAbundanceEvent : MonoBehaviour
{
    [SerializeField] private EventData _eventData;

    void Start()
    {
        CheckCondition();
    }

    public void CheckCondition()
    {
        
        //if() --> x room lvl 2
        //{
        //    EventManager.Instance.StartEvent(_eventData);
        //}
    }

}
