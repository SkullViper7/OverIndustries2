using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    //QuestInfo
    [field: SerializeField] public TextMeshProUGUI QuestNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuestDescriptionText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuestObjectifText { get; private set; }

    //InfoCurrentQuest
    [field: SerializeField] public GameObject CurrentQuestTextParent { get; private set; }
    private List<TextMeshProUGUI> _nameCurrentQuestTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _objectifCurrentQuestTextList = new List<TextMeshProUGUI>();

    //ComplitedQuest
    [field: SerializeField] public GameObject ButtonQuestComplited { get; private set; }
    private List<GameObject> _buttonQuestComplitedList = new List<GameObject>();
    [field: SerializeField] public Color32 QuestComplitedColorText { get; private set; }

    private Quest _actualQuest;

    void Start()
    {
        for (int i = 0; i < CurrentQuestTextParent.transform.childCount; i++)
        {
            _nameCurrentQuestTextList.Add(CurrentQuestTextParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>());
            _objectifCurrentQuestTextList.Add(CurrentQuestTextParent.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>());
        }
        for (int j = 0; j < ButtonQuestComplited.transform.childCount; j++)
        {
            _buttonQuestComplitedList.Add(ButtonQuestComplited.transform.GetChild(j).gameObject);
        }

        QuestManager.Instance.NewQuestGenerate += ShowQuestInfo;
        QuestManager.Instance.UpdateAdvancementQuest += UpdateCurrentAdvancementQuestComponent;
        QuestManager.Instance.UpdateAdvancementQuest += UpdateCurrentAdvancementQuestObject;
        QuestManager.Instance.QuestComplited += QuestComplited;
        QuestManager.Instance.ResetQuestText += ResetQuestText;
    }

    /// <summary>
    /// Show the quest info, description and objectifs.
    /// </summary>
    /// <param name="_quest"></param>
    public void ShowQuestInfo(Quest _quest)
    {
        _actualQuest = _quest;

        QuestNameText.text = _actualQuest.QuestData.Name;
        QuestDescriptionText.text = _actualQuest.QuestData.Description;

        if (_quest.QuestData.NumberOfComponent.Count != 0)
        {
            if (_quest.QuestData.NumberOfComponent.Count == 1)
            {
                QuestObjectifText.text = $"{_quest.QuestData.NumberOfComponent[0]} {_quest.QuestData.Component[0].Name}";
            }
            if (_quest.QuestData.NumberOfComponent.Count == 2)
            {
                QuestObjectifText.text = $"{_quest.QuestData.NumberOfComponent[0]} {_quest.QuestData.Component[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[1]} {_quest.QuestData.Component[1].Name}";
            }
            if (_quest.QuestData.NumberOfComponent.Count == 3)
            {
                QuestObjectifText.text = $"{_quest.QuestData.NumberOfComponent[0]} {_quest.QuestData.Component[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[1]} {_quest.QuestData.Component[1].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[2]} {_quest.QuestData.Component[2].Name}";
            }
            if (_quest.QuestData.NumberOfComponent.Count == 4)
            {
                QuestObjectifText.text = $"{_quest.QuestData.NumberOfComponent[0]} {_quest.QuestData.Component[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[1]} {_quest.QuestData.Component[1].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[2]} {_quest.QuestData.Component[2].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[3]} {_quest.QuestData.Component[3].Name}";
            }
            if (_quest.QuestData.NumberOfComponent.Count == 5)
            {
                QuestObjectifText.text = $"{_quest.QuestData.NumberOfComponent[0]} {_quest.QuestData.Component[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[1]} {_quest.QuestData.Component[1].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[2]} {_quest.QuestData.Component[2].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[3]} {_quest.QuestData.Component[3].Name} " +
                    $"\n{_quest.QuestData.NumberOfComponent[4]} {_quest.QuestData.Component[4].Name}";
            }
        }

        if (_quest.QuestData.NumberOfObject.Count != 0)
        {
            if (_quest.QuestData.NumberOfObject.Count == 1)
            {
                QuestObjectifText.text = $"{QuestObjectifText.text} {_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Objects[0].Name}";
            }
            if (_quest.QuestData.NumberOfObject.Count == 2)
            {
                QuestObjectifText.text = $"{QuestObjectifText.text} " +
                    $"{_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Objects[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfObject[1]} {_quest.QuestData.Objects[1].Name}";
            }
            if (_quest.QuestData.NumberOfObject.Count == 3)
            {
                QuestObjectifText.text = $"{QuestObjectifText.text} " +
                    $"{_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Objects[0].Name} " +
                    $"\n{_quest.QuestData.NumberOfObject[1]} {_quest.QuestData.Objects[1].Name} " +
                    $"\n{_quest.QuestData.NumberOfObject[2]} {_quest.QuestData.Objects[2].Name}";
            }
        }
    }

    public void QuestAccept()
    {
        QuestManager.Instance.NewCurrentQuest(_actualQuest);

        UpdateCurrentAdvancementQuestObject();
        UpdateCurrentAdvancementQuestComponent();
    }
    public void QuestRefuse()
    {
        Destroy(_actualQuest.gameObject);
    }

    /// <summary>
    /// Update the actual quest and there avancement with the actual storage of this component type
    /// </summary>
    public void UpdateCurrentAdvancementQuestComponent()
    {
        for (int i = 0; i < QuestManager.Instance.CurrentQuestList.Count; i++)
        {
            Quest _currentQuest = QuestManager.Instance.CurrentQuestList[i];
            _nameCurrentQuestTextList[i].text = _currentQuest.QuestData.Name;

            if (_currentQuest.QuestData.NumberOfComponent.Count == 1)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[0]}{_currentQuest.QuestData.Component[0].Name}";
            }
            if (_currentQuest.QuestData.NumberOfComponent.Count == 2)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[0]} {_currentQuest.QuestData.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[1]} {_currentQuest.QuestData.Component[1].Name}";
            }
            if (_currentQuest.QuestData.NumberOfComponent.Count == 3)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[0]} {_currentQuest.QuestData.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[1]} {_currentQuest.QuestData.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[2])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[2]} {_currentQuest.QuestData.Component[2].Name}";
            }
            if (_currentQuest.QuestData.NumberOfComponent.Count == 4)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[0]} {_currentQuest.QuestData.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[1]} {_currentQuest.QuestData.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[2])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[2]} {_currentQuest.QuestData.Component[2].Name}" +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[3])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[3]} {_currentQuest.QuestData.Component[3].Name}";
            }
            if (_currentQuest.QuestData.NumberOfComponent.Count == 5)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[0]} {_currentQuest.QuestData.Component[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[1]} {_currentQuest.QuestData.Component[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[2])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[2]} {_currentQuest.QuestData.Component[2].Name}" +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[3])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[3]} {_currentQuest.QuestData.Component[3].Name}"+
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisComponent(_currentQuest.QuestData.Component[4])} " +
                    $"/ {_currentQuest.QuestData.NumberOfComponent[4]} {_currentQuest.QuestData.Component[4].Name}";
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
            Quest _currentQuest = QuestManager.Instance.CurrentQuestList[i];
            _nameCurrentQuestTextList[i].text = _currentQuest.QuestData.Name;

            if (_currentQuest.QuestData.NumberOfObject.Count == 1)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Objects[0].Name}";
            }
            if (_currentQuest.QuestData.NumberOfObject.Count == 2)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Objects[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[1]} {_currentQuest.QuestData.Objects[1].Name}";
            }
            if (_currentQuest.QuestData.NumberOfObject.Count == 3)
            {
                _objectifCurrentQuestTextList[i].text = 
                    $"{_objectifCurrentQuestTextList[i].text} {ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[0])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Objects[0].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[1])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[1]} {_currentQuest.QuestData.Objects[1].Name} " +
                    $"\n{ItemStorage.Instance.ReturnNumberOfThisObject(_currentQuest.QuestData.Objects[2])} " +
                    $"/ {_currentQuest.QuestData.NumberOfObject[2]} {_currentQuest.QuestData.Objects[2].Name}";
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
            _nameCurrentQuestTextList[i].color = Color.black;
            _objectifCurrentQuestTextList[i].color = Color.black;

            _buttonQuestComplitedList[i].SetActive(false);
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
        _nameCurrentQuestTextList[_currentQuestID].color = QuestComplitedColorText;
        _objectifCurrentQuestTextList[_currentQuestID].color = QuestComplitedColorText;
        _buttonQuestComplitedList[_currentQuestID].SetActive(true);
        _buttonQuestComplitedList[_currentQuestID].GetComponent<Quest>().QuestData = QuestManager.Instance.CurrentQuestList[_currentQuestID].QuestData;
    }
}
