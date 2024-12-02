using UnityEngine;

public class RawMaterialAbundanceEvent : MonoBehaviour
{
    /// <summary>
    /// Cet event multiplie par 1.5 les mati�re premi�re gagner par le joueurs. (dur�e limit�)
    /// </summary>

    [SerializeField] private EventData _eventData;

    void Start()
    {
        EventManager.Instance.CheckCondition(_eventData);

        EventManager.Instance.EventConditionCompleted += EventComportement;

    }

    public void EventComportement()
    {
        RawMaterialStorage.Instance.RawMaterialProduct += MultiplieRawMaterialProduct;
        Debug.Log("raw material abundance event");
    }

    public void MultiplieRawMaterialProduct(int _amountProduct)
    {
        RawMaterialStorage.Instance.AddRawMaterials(_amountProduct / 2);
    }
}
