using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private NavigationManager _navigationManager;

    [Header("Tutorial Panel")]
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

    //Rooms
    private GameObject _roomResearch;
    private GameObject _roomDirector;
    private GameObject _roomAssembly;
    private GameObject _roomMachining;

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

            for (int i = 0; i < _buttonActions.Count; i++)
            {
                _buttonActions[i].SetActive(true);
            }

            //switch (_currentStep)
            //{
            //case 2:
            //    ShowConstructButton();
            //    break;
            ////case 3:
            ////    ShowStorage();
            ////    break;
            //case 4:
            //    //DesactiveConstructButton();
            //    break;
            //case 8:
            //    ShowConstructButton();
            //    break;
            //case 9:
            //    DesactiveConstructButton();
            //    break;
            //case 10:
            //    ShowCommandeButton();
            //    break;
            //case 12:
            //    ShowConstructButton();
            //    break;
            //}
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
                //case 2:
                //    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Storage)
                //    {
                //        _canContinue = true;
                //    }
                //    break;

                case 13:
                    //_roomDirector.GetComponent<DirectorRoom>().EmployeeAssign[0].GetComponent<Employee>().EmployeeJob = Job.MachiningTechnician;
                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Research)
                    {
                        _roomResearch = GridManager.Instance.InstantiatedRooms[i].gameObject;
                        _canContinue = true;
                    }
                    break;
                case 15:
                    if (DirectorRoom.Instance.RoomMain.EmployeeAssign.Count >= 1)
                    {
                        _roomDirector = DirectorRoom.Instance.RoomMain.gameObject;
                        _canContinue = true;
                    }
                    break;
                case 17:
                    if (_roomResearch.GetComponent<Room>().EmployeeAssign.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                //case 19:
                //    if (_roomResearch.GetComponent<ResearchRoom>().ObjectResearchStarted += ConditionIsMet)
                //    {
                //        _canContinue = true;
                //    }
                //    break;
                case 21:
                    _roomResearch.GetComponent<ResearchRoom>().ResearchCompleted += ConditionIsMet;
                    if (_isResearchFinish)
                    {
                        _canContinue = true;
                    }
                    break;
                case 23:
                    if (QuestManager.Instance.CurrentQuestList.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 24:
                    bool isStorage = false;
                    bool isAssembly = false;
                    bool isMachining = false;

                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Storage)
                    {
                        GameObject roomStorage = GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Storage ? GridManager.Instance.InstantiatedRooms[i].gameObject : null;
                        if (roomStorage.GetComponent<Room>().EmployeeAssign.Count >= 1)
                        {
                            isStorage = true;
                        }

                        for (int j = 0; j < GridManager.Instance.InstantiatedRooms.Count; j++)
                        {
                            if (GridManager.Instance.InstantiatedRooms[j].RoomData.RoomType == RoomType.Assembly)
                            {
                                _roomAssembly = GridManager.Instance.InstantiatedRooms[j].gameObject;
                                if (_roomAssembly.GetComponent<Room>().EmployeeAssign.Count >= 1)
                                {
                                    isAssembly = true;
                                }
                            }
                        }

                        for (int k = 0; k < GridManager.Instance.InstantiatedRooms.Count; k++)
                        {
                            if (GridManager.Instance.InstantiatedRooms[k].RoomData.RoomType == RoomType.Machining)
                            {
                                _roomMachining = GridManager.Instance.InstantiatedRooms[k].gameObject;
                                if (_roomMachining.GetComponent<Room>().EmployeeAssign.Count >= 1)
                                {
                                    isMachining = true;
                                }
                            }
                        }

                        if (isStorage && isAssembly && isMachining)
                        {
                            _canContinue = true;
                        }

                    }
                    break;
                //case 26:
                //    if (prod lancé)
                //    {
                //        _canContinue = true;
                //    }
                //    break;
                // faire une production
                default:
                    _canContinue = true;
                    break;
            }
        }
    }

    void ConditionIsMet()
    {
        _isResearchFinish = true;
    }
}
