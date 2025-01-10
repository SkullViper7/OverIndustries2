using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RawMaterialUI : MonoBehaviour
{
    [SerializeField]
    private Image _fillAmount;

    [SerializeField]
    private TMP_Text _amountTxt;

    private void Start()
    {
        RawMaterialStorage.Instance.NewAmountInInternalStorage += ChangeDisplay;
        ChangeDisplay(RawMaterialStorage.Instance.AmoutOfRawMaterial, RawMaterialStorage.Instance.StorageCapacity);
    }

    /// <summary>
    /// Called when the raw material storage changes to update display on screen.
    /// </summary>
    /// <param name="amount"> Amount to display. </param>
    /// <param name="capacity"> Capacity to display. </param>
    private void ChangeDisplay(int amount, int capacity)
    {
        _amountTxt.text = amount.ToString("N0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", " ");

        _fillAmount.fillAmount = (float)amount / (float)capacity;

        // Red
        if (amount == capacity)
        {
            if (ColorUtility.TryParseHtmlString("#F76A74", out Color newColor))
            {
                _fillAmount.color = newColor;
            }
        }
        // Orange
        else if (amount * 100f / capacity >= 90f)
        {
            if (ColorUtility.TryParseHtmlString("#FF9160", out Color newColor))
            {
                _fillAmount.color = newColor;
            }
        }
        // Green
        else
        {
            if (ColorUtility.TryParseHtmlString("#40C1AA", out Color newColor))
            {
                _fillAmount.color = newColor;
            }
        }
    }
}
