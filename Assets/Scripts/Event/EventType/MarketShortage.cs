using UnityEngine;

public class MarketShortage : MonoBehaviour
{
    /// <summary>
    /// Cet event r�duit le nombre de mati�re premi�re que le joueur r�cup�re.
    /// </summary>

    [SerializeField] private EventData _eventData;

    void Start()
    {
        EventManager.Instance.CheckCondition(_eventData);
        EventManager.Instance.EventConditionCompleted += EventComportement;
    }

    public void EventComportement()
    {
        RawMaterialStorage.Instance.RawMaterialProduct += SubstractRawMaterialProduct;
        Debug.Log("market shortage event");
    }

    public void SubstractRawMaterialProduct(int _amountProduct)
    {
        int _substract = (int)Mathf.Round(_amountProduct / _eventData.RawMaterialDivider);
        RawMaterialStorage.Instance.SubstractRawMaterials(_substract);
    }
}