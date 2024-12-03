using UnityEngine;
using TMPro;
using System.Collections;

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
    private GameObject _nextButton;

    void Start()
    {
        ShowTutorialStep();
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
                case 0:
                    Step1();
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

    void Step1()
    {
        _nextButton.SetActive(true);
    }

    public void HideTutorialStep()
    {
        _tutorialPanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        _tutorialPanel.SetActive(true);
    }
}
