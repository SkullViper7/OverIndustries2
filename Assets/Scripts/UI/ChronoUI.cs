using UnityEngine;
using TMPro;

public class ChronoUI : MonoBehaviour
{
    /// <summary>
    /// Minutes, seconds and centiseconds on screen.
    /// </summary>
    [SerializeField]
    private TMP_Text _minutes, _seconds, _centiseconds;

    private void Start()
    {
        ChronoManager.Instance.NewMinute += UpdateMinutes;
        ChronoManager.Instance.NewSecond += UpdateSeconds;
        ChronoManager.Instance.NewCentisecond += UpdateCentiseconds;
    }

    /// <summary>
    /// Called to update minutes.
    /// </summary>
    /// <param name="newMinutes"> New minutes to update. </param>
    private void UpdateMinutes(int newMinutes)
    {
        _minutes.SetText(ConvertToString(newMinutes));
    }

    /// <summary>
    /// Called to update seconds.
    /// </summary>
    /// <param name="newSeconds"> New seconds to update. </param>
    private void UpdateSeconds(int newSeconds)
    {
        _seconds.SetText(ConvertToString(newSeconds));
    }

    /// <summary>
    /// Called to update centiseconds.
    /// </summary>
    /// <param name="newCentiseconds"> New centiseconds to update. </param>
    private void UpdateCentiseconds(int newCentiseconds)
    {
        _centiseconds.SetText(ConvertToString(newCentiseconds));
    }

    /// <summary>
    /// Called to convert a time value into a time to show on screen.
    /// </summary>
    /// <param name="_time"> Time to convert. </param>
    /// <returns></returns>
    private string ConvertToString(int _time)
    {
        if (_time >= 10)
        {
            return _time.ToString();
        }
        else
        {
            return $"{0}{_time}";
        }
    }
}
