using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialPanel;
    [SerializeField]
    private TextMeshProUGUI _tutoralText;
    private int _currentStep = 0;

    [Header("Tutorial Steps")]
    [SerializeField]
    private string[] _tutorialSteps;
    private int _currentCharIndex = 0;

    [SerializeField]
    private List<GameObject> _buttonActions;

    void Start()
    {
        ShowTutorialStep();

        for(int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(false);
        }
    }

    public void ShowTutorialStep()
    {
        if (_currentStep <= _tutorialSteps.Length)
        {
            _currentCharIndex = 0; // Reset character index for the new step
            _tutoralText.text = ""; // Clear the text initially
            StartCoroutine(RevealText(_tutorialSteps[_currentStep])); // Start revealing the text

            switch (_currentStep)
            {
                case 2:
                    ShowConstructButton();
                    break;
                case 3:
                    DesactiveConstructButton();
                    break;
                case 7:
                    ShowCommandeButton();
                    break;
                case 8:
                    ShowStorage();
                    break;
                case 9:
                    ShowConstructButton();
                    break;
                case 14:
                    DesactiveConstructButton();
                    break;
            }
        }
        else
        {
            HideTutorialStep();
        }
    }

    private IEnumerator RevealText(string fullText)
    {
        while (_currentCharIndex < fullText.Length)
        {
            _tutoralText.text += fullText[_currentCharIndex]; // Add the next character
            _currentCharIndex++;
            yield return new WaitForSeconds(0.025f); // Adjust the delay as necessary
        }
        _currentCharIndex = 0; // Reset character index for the new step
    }

    public void NextStep()
    {
        if (_currentCharIndex < _tutorialSteps[_currentStep].Length)
        {
            // If the text isn't fully revealed, reveal all of it
            _tutoralText.text = _tutorialSteps[_currentStep];
            _currentCharIndex = _tutorialSteps[_currentStep].Length; // Mark as fully revealed
        }
        else
        {
            _currentStep++;

            if (_currentStep >= _tutorialSteps.Length)
            {
                HideTutorialStep();
            }
            else
            {
                ShowTutorialStep();
            }
        }
    }

    void ShowConstructButton()
    {
        _buttonActions[0].SetActive(true);
    }

    void DesactiveConstructButton()
    {
        _buttonActions[0].SetActive(false);
    }

    void ShowStorage()
    {
        _buttonActions[0].SetActive(false);
        _buttonActions[1].SetActive(false);
        _buttonActions[2].SetActive(true);
    }

    void ShowCommandeButton()
    {
        _buttonActions[1].SetActive(true);
    }

    public void HideTutorialStep()
    {
        for (int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(true);
        }

        _tutorialPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        _tutorialPanel.SetActive(true);
    }
}
