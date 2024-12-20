using System.Collections;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        StartCoroutine(Counter());
    }

    IEnumerator Counter()
    {
        while (true)
        {
            _text.text = "FPS: " + Mathf.RoundToInt(1f / Time.deltaTime);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
