using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueUpdater : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    private TMP_Text _value;

    private void Awake()
    {
        _value = GetComponent<TMP_Text>();
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void OnEnable()
    {
        _value.text = "x" + _slider.value.ToString();
    }

    private void UpdateValue(float newValue)
    {
        _value.text = "x" + newValue.ToString();
    }
}
