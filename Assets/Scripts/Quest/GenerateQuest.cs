using UnityEngine;

public class GenerateQuest : MonoBehaviour
{
    //public Quest _newQuest;

    //public int MinNumberQuestObject { get; private set; }
    //public int MaxNumberQuestObject { get; private set; }

    //public event System.Action<Quest> NewQuestGenerate;

    //public void Start()
    //{
    //    if (QuestManager.Instance.MaxCurrentQuest < QuestManager.Instance.CurrentQuestList.Count)
    //    {
    //        GenerateNewQuest();
    //    }
    //}

    //public void GenerateNewQuest()
    //{
    //    _newQuest = ObjectPool.Instance.RequestObject(4).GetComponent<Quest>();

    //    int randomQuest = Random.Range(0, QuestManager.Instance.AllQuestList.Count);
    //    _newQuest.GiveQuest(QuestManager.Instance.AllQuestList[randomQuest]);

    //    for (int i = 0; i < _newQuest.QuestData.NumberOfObject.Count; i++)
    //    {
    //        int randomNumberOfThisObject = Random.Range(MinNumberQuestObject, MaxNumberQuestObject);
    //        _newQuest.QuestData.NumberOfObject[i] = randomNumberOfThisObject;
    //    }
    //    NewQuestGenerate.Invoke(_newQuest);
    //}
}
