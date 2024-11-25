using UnityEngine;

public class StatsUI : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _statsUI;

    public void Interaction()
    {
        if (_statsUI.activeInHierarchy) _statsUI.SetActive(false);
        else _statsUI.SetActive(true);
    }
}
