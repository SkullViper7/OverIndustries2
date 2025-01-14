using UnityEngine;
using TMPro;
using System.Collections;

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

    private bool _canContinue = true;
    private bool _isConditionIsMet = false;

    //Rooms
    private GameObject _roomResearch;
    private GameObject _roomDirector;
    private GameObject _roomAssembly;
    private GameObject _roomMachining;

    public event System.Action<int> OnTutorialImageStep;
    public event System.Action OnTutorialImageHide;
    public event System.Action<int> OnTutorialButtonShow;
    public event System.Action OnTutorialButtonHide;
    public event System.Action OnTutorialHideBackground;
    public event System.Action OnTutorialShowBackground;

    void Start()
    {
        ShowTutorialStep();
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
                case 3:
                    OnTutorialImageStep?.Invoke(0);
                    OnTutorialImageStep?.Invoke(1);
                    break;
                case 4:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(2);
                    OnTutorialButtonShow?.Invoke(0);
                    break;
                case 5:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(3);
                    OnTutorialButtonShow?.Invoke(1);
                    break;
                case 6:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(4);
                    OnTutorialButtonShow?.Invoke(2);
                    break;
                case 7:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(5);
                    OnTutorialButtonShow?.Invoke(3);
                    break;
                case 8:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(6);
                    break;
                case 10:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(7);
                    OnTutorialImageStep?.Invoke(8);
                    break;
                case 11:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(6);
                    OnTutorialImageStep?.Invoke(7);
                    OnTutorialImageStep?.Invoke(9);
                    break;
                case 12:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(2);
                    OnTutorialImageStep?.Invoke(10);
                    OnTutorialImageStep?.Invoke(11);
                    break;
                case 13:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(12);
                    OnTutorialImageStep?.Invoke(13);
                    OnTutorialImageStep?.Invoke(14);
                    break;
                case 14:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(12);
                    OnTutorialImageStep?.Invoke(15);
                    OnTutorialImageStep?.Invoke(16);
                    OnTutorialImageStep?.Invoke(17);
                    OnTutorialImageStep?.Invoke(18);
                    break;
                case 15:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(17);
                    OnTutorialImageStep?.Invoke(19);
                    OnTutorialImageStep?.Invoke(20);
                    break;
                case 16:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(21);
                    OnTutorialImageStep?.Invoke(22);
                    break;
                case 17:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(3);
                    OnTutorialImageStep?.Invoke(23);
                    OnTutorialImageStep?.Invoke(24);
                    break;
                case 19:
                    OnTutorialShowBackground?.Invoke();

                    OnTutorialImageHide?.Invoke();
                    OnTutorialImageStep?.Invoke(25);
                    OnTutorialImageStep?.Invoke(26);
                    OnTutorialImageStep?.Invoke(27);
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

            //_canContinue = true;
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
        OnTutorialButtonHide?.Invoke();
        HideTutorialStep();
    }

    /// <summary>
    /// Hide the tutorial panel
    /// </summary>
    public void HideTutorialStep()
    {
        OnTutorialButtonHide?.Invoke();
        OnTutorialHideBackground?.Invoke();
        _tutorialPanel.SetActive(false);
    }

    void CanContinue()
    {
        for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
        {
            switch (_currentStep)
            {
                case 12:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Research)
                    {
                        _roomResearch = GridManager.Instance.InstantiatedRooms[i].gameObject;
                        _canContinue = true;
                    }
                    break;
                case 13:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    if (DirectorRoom.Instance.RoomMain.EmployeeAssign.Count >= 1)
                    {
                        _roomDirector = DirectorRoom.Instance.RoomMain.gameObject;
                        _canContinue = true;
                    }
                    break;
                case 14:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    if (_roomResearch.GetComponent<Room>().EmployeeAssign.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 15:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    _roomResearch.GetComponent<ResearchRoom>().StartReasearch += ConditionIsMet;
                    if (_isConditionIsMet)
                    {
                        _isConditionIsMet = false;
                        _canContinue = true;
                    }
                    break;
                case 16:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    _roomResearch.GetComponent<ResearchRoom>().ResearchCompleted += ConditionIsMet;
                    if (_isConditionIsMet)
                    {
                        _isConditionIsMet = false;
                        _canContinue = true;
                    }
                    break;
                case 17:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

                    if (QuestManager.Instance.CurrentQuestList.Count >= 1)
                    {
                        _canContinue = true;
                    }
                    break;
                case 18:
                    OnTutorialImageHide?.Invoke();
                    OnTutorialHideBackground?.Invoke();

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
        _isConditionIsMet = true;
    }
}
