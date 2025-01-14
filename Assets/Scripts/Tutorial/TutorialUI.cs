using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("Tutorial Panel")]
    [SerializeField]
    private List<GameObject> _buttonActions;

    [Header("Tutorial Steps Image")]
    [SerializeField]
    private List<Image> _tutorialImages;

    [SerializeField]
    private TutorialManager _tutorialManager;

    private void Start()
    {
        for (int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(false);
        }

        for (int i = 0; i < _tutorialImages.Count; i++)
        {
            _tutorialImages[i].gameObject.SetActive(false);
        }

        _tutorialManager.OnTutorialImageStep += ShowTutorialStep;
        _tutorialManager.OnTutorialImageHide += HideTutorialStep;
    }

    public void ShowTutorialStep(int imageActive)
    {
        if (imageActive <= _tutorialImages.Count)
        {
            _tutorialImages[imageActive].gameObject.SetActive(true);
        }
    }

    public void HideTutorialStep()
    {
        for (int i = 0; i < _tutorialImages.Count; i++)
        {
            _tutorialImages[i].gameObject.SetActive(false);
        }
    }
}
