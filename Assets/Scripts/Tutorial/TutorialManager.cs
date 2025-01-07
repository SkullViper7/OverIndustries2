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

    private bool _canContinue = true;
    private bool _isResearchFinish = false;

    private GameObject _roomResearch;

    void Start()
    {
        ShowTutorialStep();

        for (int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(false);
        }
    }

    /// <summary>
    /// Show the tutorial panel
    /// </summary>
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
                case 6:
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

    /// <summary>
    /// Reveal the text character by character
    /// </summary>
    /// <param name="fullText"></param>
    /// <returns></returns>
    private IEnumerator RevealText(string fullText)
    {
        while (_currentCharIndex < fullText.Length)
        {
            _tutoralText.text += fullText[_currentCharIndex]; // Add the next character
            _currentCharIndex++;
            yield return new WaitForSeconds(0.025f); // Adjust the delay as necessary
        }
        //_currentCharIndex = 0; // Reset character index for the new step
    }

    /// <summary>
    /// Reveal the full text of the current step
    /// </summary>
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
            CanContinue();
            Debug.Log(_canContinue);

            if (_canContinue)
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

                _canContinue = false;
            }
        }
    }

    /// <summary>
    /// Skip the tutorial
    /// </summary>
    public void SkipTutorial()
    {
        _currentStep = _tutorialSteps.Length;
        HideTutorialStep();
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

    /// <summary>
    /// Hide the tutorial panel
    /// </summary>
    public void HideTutorialStep()
    {
        for (int i = 0; i < _buttonActions.Count; i++)
        {
            _buttonActions[i].SetActive(true);
        }

        _tutorialPanel.SetActive(false);
    }

    void CanContinue()
    {
        for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
        {
            switch (_currentStep)
            {
                case 2:
                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Research)
                    {
                        _roomResearch = GridManager.Instance.InstantiatedRooms[i].gameObject;
                        _canContinue = true;
                    }
                    break;
                case 3:
                    if (DirectorRoom.Instance._roomMain.EmployeeAssign.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 4:
                    if (_roomResearch.GetComponent<Room>().EmployeeAssign.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 5:
                    _roomResearch.GetComponent<ResearchRoom>().ResearchCompleted += ResearchFinish;
                    if (_isResearchFinish)
                    {
                        _canContinue = true;

                    }
                    break;
                case 6:
                    if (QuestManager.Instance.QuestList.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 9:
                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Delivery)
                    {
                        _canContinue = true;
                    }
                    break;
                case 11:
                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Machining)
                    {
                        _canContinue = true;
                    }
                    break;
                case 16:
                    // lié a questManager cré un event pour le finish de la quete et ainsi avoir acces à la dernière phrase du tuto
                    break;
                default:
                    _canContinue = true;
                    break;
            }
        }
    }

    void ResearchFinish()
    {
        _isResearchFinish = true;
    }
}
