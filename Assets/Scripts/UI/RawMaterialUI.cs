using UnityEngine;
using TMPro;

public class RawMaterialUI : MonoBehaviour
{
    /// <summary>
    /// Raw material storage on screen.
    /// </summary>
    [SerializeField]
    private TMP_Text _storageDisplayed;

    private void Start()
    {
        RawMaterialStorage.Instance.NewAmountInInternalStorage += ChangeDisplay;
    }

    /// <summary>
    /// Called when the raw material storage changes to update display on screen.
    /// </summary>
    /// <param name="newAmount"> New amount to display. </param>
    /// <param name="newCapacity"> New capacity to display. </param>
    private void ChangeDisplay(int newAmount, int newCapacity)
    {
        _storageDisplayed.text = newAmount.ToString() + "/" + newCapacity.ToString();
    }
}
