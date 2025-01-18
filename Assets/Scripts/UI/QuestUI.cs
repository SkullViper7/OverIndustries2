using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject _newQuestPopUp;

    [Space, Header("Quest Info")]
    [field: SerializeField] private TextMeshProUGUI _questNameText;
    [field: SerializeField] private TextMeshProUGUI _questDescriptionText;
    [field: SerializeField] private TextMeshProUGUI _questObjectifText;
    [field: SerializeField] private TextMeshProUGUI _questPSText;

    [Space, Header("Info current quest")]
    [field: SerializeField] private List<TextMeshProUGUI> _nameCurrentQuestTextList;
    [field: SerializeField] private List<TextMeshProUGUI> _objectifCurrentQuestTextList;
    [field: SerializeField] private List<TextMeshProUGUI> _PSCurrentQuestTextList;

    [Space, Header("Complited quest")]
    [field: SerializeField] private List<GameObject> _buttonQuestComplitedList = new List<GameObject>();

    [Space, Header("Complited quest")]
    [SerializeField] private GameObject _warningQuestPopUp;
    private QuestData _actualQuest;

    void Start()
    {
        QuestManager.Instance.NewQuestGenerate += ShowQuestInfo;
        QuestManager.Instance.UpdateAdvancementQuest += UpdateCurrentAdvancementQuestComponent;
        QuestManager.Instance.UpdateAdvancementQuest += UpdateCurrentAdvancementQuestObject;
        QuestManager.Instance.QuestComplited += QuestComplited;
        QuestManager.Instance.ResetQuestText += ResetQuestText;
        QuestManager.Instance.MaxQuest += QuestWarning;
    }

    /// <summary>
    /// Show the quest info, description and objectifs.
    /// </summary>
    /// <param name="_quest"></param>
    public void ShowQuestInfo(QuestData _quest)
    {
        _newQuestPopUp.SetActive(true);
        _actualQuest = _quest;

        _questNameText.text = _actualQuest.Name;
        _questDescriptionText.text = _actualQuest.Description;
        _questPSText.text = _actualQuest.PSWin.ToString();

        if (_quest.NumberOfComponent.Count != 0)
        {
            if (_quest.NumberOfComponent.Count == 1)
            {
                _questObjectifText.text = $"{_quest.NumberOfComponent[0]} {_quest.Component[0].Name}";
            }
            if (_quest.NumberOfComponent.Count == 2)
            {
                _questObjectifText.text = $"{_quest.NumberOfComponent[0]} {_quest.Component[0].Name} " +
                    $"\n{_quest.NumberOfComponent[1]} {_quest.Component[1].Name}";
            }
            if (_quest.NumberOfComponent.Count == 3)
            {
                _questObjectifText.text = $"{_quest.NumberOfComponent[0]} {_quest.Component[0].Name} " +
                    $"\n{_quest.NumberOfComponent[1]} {_quest.Component[1].Name} " +
                    $"\n{_quest.NumberOfComponent[2]} {_quest.Component[2].Name}";
            }
            if (_quest.NumberOfComponent.Count == 4)
            {
                _questObjectifText.text = $"{_quest.NumberOfComponent[0]} {_quest.Component[0].Name} " +
                    $"\n{_quest.NumberOfComponent[1]} {_quest.Component[1].Name} " +
                    $"\n{_quest.NumberOfComponent[2]} {_quest.Component[2].Name} " +
                    $"\n{_quest.NumberOfComponent[3]} {_quest.Component[3].Name}";
            }
            if (_quest.NumberOfComponent.Count == 5)
            {
                _questObjectifText.text = $"{_quest.NumberOfComponent[0]} {_quest.Component[0].Name} " +
                    $"\n{_quest.NumberOfComponent[1]} {_quest.Component[1].Name} " +
                    $"\n{_quest.NumberOfComponent[2]} {_quest.Component[2].Name} " +
                    $"\n{_quest.NumberOfComponent[3]} {_quest.Component[3].Name} " +
                    $"\n{_quest.NumberOfComponent[4]} {_quest.Component[4].Name}";
            }
        }

        if (_quest.NumberOfObject.Count != 0)
        {
            if (_quest.NumberOfObject.Count == 1)
            {
                _questObjectifText.text = $"{_questObjectifText.text} {_quest.NumberOfObject[0]} {_quest.Objects[0].Name}";
            }
            if (_quest.NumberOfObject.Count == 2)
            {
                _questObjectifText.text = $"{_questObjectifText.text} " +
                    $"{_quest.NumberOfObject[0]} {_quest.Objects[0].Name} " +
                    $"\n{_quest.NumberOfObject[1]} {_quest.Objects[1].Name}";
            }
            if (_quest.NumberOfObject.Count == 3)
            {
                _questObjectifText.text = $"{_questObjectifText.text} " +
                    $"{_quest.NumberOfObject[0]} {_quest.Objects[0].Name} " +
                    $"\n{_quest.NumberOfObject[1]} {_quest.Objects[1].Name} " +
                    $"\n{_quest.NumberOfObject[2]} {_quest.Objects[2].Name}";
            }
        }
    }

    //Warning pop up 
    public void QuestWarning()
    {
        _warningQuestPopUp.SetActive(true);
    }

    public void QuestAccept()
    {
        QuestManager.Instance.NewCurrentQuest(_actualQuest);
        _questObjectifText.text = "";
        UpdateCurrentAdvancementQuestObject();
        UpdateCurrentAdvancementQuestComponent();
    }
    public void QuestRefuse()
    {
        _questObjectifText.text = "";
    }

    /// <summary>
    /// Update the actual quest and there avancement with the actual storage of this component type
    /// </summary>
    public void UpdateCurrentAdvancementQuestComponent()
    {
        for (int i = 0; i < QuestManager.Instance.CurrentQuestList.Count; i++)
        {
            RectTransform rect = _objectifCurrentQuestTextList[i].GetComponent(typeof(RectTransform)) as RectTransform;

            QuestData _currentQuest = QuestManager.Instance.CurrentQuestList[i];
            _nameCurrentQuestTextList[i].text = _currentQuest.Name;
            _PSCurrentQuestTextList[i].gameObject.transform.parent.gameObject.SetActive(true);
            _PSCurrentQuestTextList[i].text = _currentQuest.PSWin.ToString();

            if (_currentQuest.NumberOfComponent.Count == 1)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[0])} " +
                    $"/ {_currentQuest.NumberOfComponent[0]} {_currentQuest.Component[0].Name}";
            }
            if (_currentQuest.Component.Count == 2)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[0])} " +
                    $"/ {_currentQuest.NumberOfComponent[0]} {_currentQuest.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[1])} " +
                    $"/ {_currentQuest.NumberOfComponent[1]} {_currentQuest.Component[1].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 80);

            }
            if (_currentQuest.NumberOfComponent.Count == 3)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[0])} " +
                    $"/ {_currentQuest.NumberOfComponent[0]} {_currentQuest.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[1])} " +
                    $"/ {_currentQuest.NumberOfComponent[1]} {_currentQuest.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[2])} " +
                    $"/ {_currentQuest.NumberOfComponent[2]} {_currentQuest.Component[2].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 120);

            }
            if (_currentQuest.NumberOfComponent.Count == 4)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[0])} " +
                    $"/ {_currentQuest.NumberOfComponent[0]} {_currentQuest.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[1])} " +
                    $"/ {_currentQuest.NumberOfComponent[1]} {_currentQuest.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[2])} " +
                    $"/ {_currentQuest.NumberOfComponent[2]} {_currentQuest.Component[2].Name}" +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[3])} " +
                    $"/ {_currentQuest.NumberOfComponent[3]} {_currentQuest.Component[3].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 160);

            }
            if (_currentQuest.NumberOfComponent.Count == 5)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[0])} " +
                    $"/ {_currentQuest.NumberOfComponent[0]} {_currentQuest.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[1])} " +
                    $"/ {_currentQuest.NumberOfComponent[1]} {_currentQuest.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[2])} " +
                    $"/ {_currentQuest.NumberOfComponent[2]} {_currentQuest.Component[2].Name}" +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[3])} " +
                    $"/ {_currentQuest.NumberOfComponent[3]} {_currentQuest.Component[3].Name}" +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.Component[4])} " +
                    $"/ {_currentQuest.NumberOfComponent[4]} {_currentQuest.Component[4].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 200);

            }
        }
    }

    /// <summary>
    /// Update the actual quest and there avancement with the actual storage of this object type
    /// </summary>
    public void UpdateCurrentAdvancementQuestObject()
    {
        for (int i = 0; i < QuestManager.Instance.CurrentQuestList.Count; i++)
        {
            RectTransform rect = _objectifCurrentQuestTextList[i].GetComponent(typeof(RectTransform)) as RectTransform;

            QuestData _currentQuest = QuestManager.Instance.CurrentQuestList[i];
            _nameCurrentQuestTextList[i].text = _currentQuest.Name;
            _PSCurrentQuestTextList[i].gameObject.transform.parent.gameObject.SetActive(true);
            _PSCurrentQuestTextList[i].text = _currentQuest.PSWin.ToString();

            if (_currentQuest.NumberOfObject.Count == 1)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[0])} " +
                    $"/ {_currentQuest.NumberOfObject[0]} {_currentQuest.Objects[0].Name}";
                
                if (_objectifCurrentQuestTextList[i].text != "")
                {
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x + 40);
                }
            }
            if (_currentQuest.NumberOfObject.Count == 2)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[0])} " +
                    $"/ {_currentQuest.NumberOfObject[0]} {_currentQuest.Objects[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[1])} " +
                    $"/ {_currentQuest.NumberOfObject[1]} {_currentQuest.Objects[1].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x + 80);

            }
            if (_currentQuest.NumberOfObject.Count == 3)
            {
                _objectifCurrentQuestTextList[i].text =
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[0])} " +
                    $"/ {_currentQuest.NumberOfObject[0]} {_currentQuest.Objects[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[1])} " +
                    $"/ {_currentQuest.NumberOfObject[1]} {_currentQuest.Objects[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.Objects[2])} " +
                    $"/ {_currentQuest.NumberOfObject[2]} {_currentQuest.Objects[2].Name}";
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x + 120);

            }
        }
    }

    /// <summary>
    /// Reset all text for a new quest
    /// </summary>
    public void ResetQuestText()
    {
        for (int i = 0; i < _objectifCurrentQuestTextList.Count; i++)
        {
            _buttonQuestComplitedList[i].SetActive(false);
            _PSCurrentQuestTextList[i].transform.parent.gameObject.SetActive(false);
            _nameCurrentQuestTextList[i].text = "";
            _objectifCurrentQuestTextList[i].text = "";
        }
    }

    /// <summary>
    /// Change the text color if the quest is complited and active button to give object
    /// </summary>
    /// <param name="_currentQuestID"></param>
    public void QuestComplited(int _currentQuestID)
    {
        _buttonQuestComplitedList[_currentQuestID].SetActive(true);
        _buttonQuestComplitedList[_currentQuestID].GetComponent<Quest>().QuestData = QuestManager.Instance.CurrentQuestList[_currentQuestID];
    }
}
