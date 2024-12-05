using UnityEngine;
using TMPro;

public class RawMaterialUI : MonoBehaviour
{
    /// <summary>
    /// Raw material amount on screen.
    /// </summary>
    [SerializeField]
    private TMP_Text _rawMaterialAmount, _capacity;

    private void Start()
    {
        RawMaterialStorage.Instance.AmountHasChanged += ChangeAmount;
        RawMaterialStorage.Instance.CapacityHasChanged += ChangeCapacity;
    }

    /// <summary>
    /// Called when the amount of raw material changes to update display on screen.
    /// </summary>
    /// <param name="newAmount"> New amount to display. </param>
    private void ChangeAmount(int newAmount)
    {
        _rawMaterialAmount.text = newAmount.ToString();
    }

    /// <summary>
    /// Called when the capacity of raw material changes to update display on screen.
    /// </summary>
    /// <param name="newCapacity"> New capacity to display. </param>
    private void ChangeCapacity(int newCapacity)
    {
        _capacity.text = newCapacity.ToString();
    }
}
