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
        if (_currentStep < _tutorialSteps.Length)
        {
            _currentCharIndex = 0; // Reset character index for the new step
            _tutoralText.text = ""; // Clear the text initially
            StartCoroutine(RevealText(_tutorialSteps[_currentStep])); // Start revealing the text
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
            switch (_currentStep)
            {
                case 0:
                    Step1();
                    break;
                case 2:
                    Step2();
                    break;
                default:
                    ShowTutorialStep();
                    break;
            }
            _currentStep++;

            if (_currentStep >= _tutorialSteps.Length)
            {
                HideTutorialStep();
            }
        }
    }

    void Step1()
    {
        _nextButton.SetActive(true);
        HideTutorialStep();
    }

    void Step2()
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
