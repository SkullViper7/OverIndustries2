using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("Tutorial Buttons")]
    [SerializeField]
    private List<GameObject> _buttonActions;

    [Header("Tutorial Steps Image")]
    [SerializeField]
    private List<Image> _tutorialImages;

    [SerializeField]
    private TutorialManager _tutorialManager;

    [SerializeField]
    private GameObject _background;

    [SerializeField]
    private GameObject _tutoralContinuePopUp;

    [SerializeField]
    private UIManager _uiManager;

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
        _tutorialManager.OnTutorialButtonShow += ShowButtonAction;
        _tutorialManager.OnTutorialButtonHide += ShowButtonAction;
        _tutorialManager.OnTutorialHideBackground += HideBackground;
        _tutorialManager.OnTutorialShowBackground += ShowBackground;
        _tutorialManager.OnTutorialContinuePopUp += ShowTutorialContinuePopUp;
    }

    public void ShowTutorialStep(int imageActive)
    {
        if (imageActive <= _tutorialImages.Count)
        {
            _tutorialImages[imageActive].gameObject.SetActive(true);
        }
    }

    public void ShowButtonAction(int buttonActive)
    {
        if (buttonActive <= _buttonActions.Count)
        {
            _buttonActions[buttonActive].SetActive(true);
        }
    }

    public void HideTutorialStep()
    {
        for (int i = 0; i < _tutorialImages.Count; i++)
        {
            _tutorialImages[i].gameObject.SetActive(false);
        }
    }

    public void ShowButtonAction()
    {
        for (int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(true);
        }
    }

    public void HideBackground()
    {
        _background.SetActive(false);
    }

    public void ShowBackground()
    {
        _background.SetActive(true);
    }

    public void ShowTutorialContinuePopUp()
    {
        _uiManager.OpenSFX();
        _uiManager.OpenUI();
        ChronoManager.Instance.StopChronometer();
        _tutoralContinuePopUp.SetActive(true);
    }

    public void ContinueGame()
    {
        ChronoManager.Instance.StartChronometer(20);
    }
}
