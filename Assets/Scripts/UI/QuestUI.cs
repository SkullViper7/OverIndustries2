using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    //QuestInfo
    [field: SerializeField] public TextMeshProUGUI QuestNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuestDescriptionText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuestObjectifText { get; private set; }

    //InfoCurrentQuest
    [field: SerializeField] public GameObject CurrentQuestTextParent { get; private set; }
    public List<TextMeshProUGUI> _nameCurrentQuestTextList = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> _objectifCurrentQuestTextList = new List<TextMeshProUGUI>();

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
        QuestManager.Instance.UpdateAdvancementQuest += UpdateCurrentAdvancementQuest;
        QuestManager.Instance.QuestComplited += QuestComplited;
        QuestManager.Instance.ResetQuestText += ResetQuestText;
    }

    public void ShowQuestInfo(Quest _quest)
    {
        _actualQuest = _quest;

        QuestNameText.text = _actualQuest.QuestData.Name;
        QuestDescriptionText.text = _actualQuest.QuestData.Description;

        if (_quest.QuestData.NumberOfObject.Count == 1)
        {
            QuestObjectifText.text = $"{_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Object[0]}";
        }

        if (_quest.QuestData.NumberOfObject.Count == 2)
        {
            QuestObjectifText.text = $"{_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Object[0]} \n{_quest.QuestData.NumberOfObject[1]} {_quest.QuestData.Object[1]}";
        }
        if (_quest.QuestData.NumberOfObject.Count == 3)
        {
            QuestObjectifText.text = $"{_quest.QuestData.NumberOfObject[0]} {_quest.QuestData.Object[0]} \n{_quest.QuestData.NumberOfObject[1]} {_quest.QuestData.Object[1]} \n{_quest.QuestData.NumberOfObject[2]} {_quest.QuestData.Object[2]}";
        }
    }
    public void QuestAccept()
    {
        Debug.Log("Quest accepted !");
        QuestManager.Instance.NewCurrentQuest(_actualQuest);

        UpdateCurrentAdvancementQuest();
    }
    public void QuestRefuse()
    {
        Debug.Log("Quest refused !");
        ObjectPool.Instance.ReturnThisObject(_actualQuest.gameObject);
    }

    public void UpdateCurrentAdvancementQuest()
    {
        for (int i = 0; i < QuestManager.Instance.CurrentQuestList.Count; i++)
        {
            Quest _currentQuest = QuestManager.Instance.CurrentQuestList[i];
            _nameCurrentQuestTextList[i].text = _currentQuest.QuestData.Name;

            if (_currentQuest.QuestData.NumberOfObject.Count == 1)
            {
                _objectifCurrentQuestTextList[i].text = $"{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Object[0]}";
            }

            if (_currentQuest.QuestData.NumberOfObject.Count == 2)
            {
                _objectifCurrentQuestTextList[i].text = $"{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Object[0]} \n{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[1]} {_currentQuest.QuestData.Object[1]}";
            }

            if (_currentQuest.QuestData.NumberOfObject.Count == 3)
            {
                _objectifCurrentQuestTextList[i].text = $"{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[0]} {_currentQuest.QuestData.Object[0]} \n{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[1]} {_currentQuest.QuestData.Object[1]} \n{QuestManager.Instance.StockOfThisObjectTemp}/{_currentQuest.QuestData.NumberOfObject[2]} {_currentQuest.QuestData.Object[2]}";
            }
        }
    }
    public void ResetQuestText(int _questID)
    {
        _nameCurrentQuestTextList[_questID].color = Color.black;
        _objectifCurrentQuestTextList[_questID].color = Color.black;
        _buttonQuestComplitedList[_questID].SetActive(false);

        for (int i = 0; i < _objectifCurrentQuestTextList.Count; i++)
        {
            _nameCurrentQuestTextList[i].text = " ";
            _objectifCurrentQuestTextList[i].text = " ";
        }
    }

    public void QuestComplited(int _currentQuestID)
    {
        _nameCurrentQuestTextList[_currentQuestID].color = QuestComplitedColorText;
        _objectifCurrentQuestTextList[_currentQuestID].color = QuestComplitedColorText;
        _buttonQuestComplitedList[_currentQuestID].SetActive(true);
        _buttonQuestComplitedList[_currentQuestID].GetComponent<Quest>().QuestData = QuestManager.Instance.CurrentQuestList[_currentQuestID].QuestData;
    }
}
