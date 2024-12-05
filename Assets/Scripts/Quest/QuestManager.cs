using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance = null;
    public static QuestManager Instance => _instance;

    [field: SerializeField] public List<QuestData> QuestList { get; private set; }
    public List<Quest> CurrentQuestList { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of current quest")] public int MaxCurrentQuest { get; private set; }

    [Space, Header("Quest Stats")]
    private Quest _newQuest;
    [field: SerializeField, Tooltip("Minimum number of object need to all quest")] public int MinNumberQuestObject { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of object need to all quest")] public int MaxNumberQuestObject { get; private set; }


    public event System.Action<Quest> NewQuestGenerate;
    public event System.Action UpdateAdvancementQuest, ResetQuestText;
    public event System.Action<int> QuestComplited;

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public void Start()
    {
        ItemStorage.Instance.StorageChanged += QuestAdvancement;
    }

    /// <summary>
    /// Choisi une nouvelle quête, chaque quête gare son nom, sa description et son objet associer, seul le nombre d'objets nécessaire change
    /// </summary>
    public void GenerateNewQuest()
    {
        if (QuestList.Count > 0 && MaxCurrentQuest > CurrentQuestList.Count)
        {
            //_newQuest = ObjectPoolManager.Instance.RequestObject(4).GetComponent<Quest>();


            int randomQuest = Random.Range(0, QuestList.Count);
            _newQuest.GiveQuest(QuestList[randomQuest]);


            for (int i = 0; i < _newQuest.QuestData.NumberOfObject.Count; i++)
            {
                int randomNumberOfThisObject = Random.Range(MinNumberQuestObject, MaxNumberQuestObject);
                _newQuest.QuestData.NumberOfObject[i] = randomNumberOfThisObject;
            }

            NewQuestGenerate.Invoke(_newQuest);
        }
    }

    public void NewCurrentQuest(Quest _quest)
    {
        CurrentQuestList.Add(_quest);
        QuestList.Remove(_quest.QuestData);
    }

    /// <summary>
    /// Donne l'avancement de la quête pour chaque objectifs + check si il est accompli
    /// </summary>
    /// <param name="_quest"></param>
    /// <param name="_objectifNumber"></param>
    /// <param name="_actualStockOfThisObject"></param>
    public void QuestAdvancement(ObjectType _object, int _actualStockOfThisObject)
    {
        UpdateAdvancementQuest.Invoke();

        for (int i = 0; i < CurrentQuestList.Count; i++)
        {
            for (int j = 0; j < CurrentQuestList[i].QuestData.Object.Count; j++)
            {
                if (CurrentQuestList[i].QuestData.Object.Count > 1 && j == 0 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Object[j]) >= CurrentQuestList[i].QuestData.NumberOfObject[j] && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Object[j + 1]) >= CurrentQuestList[i].QuestData.NumberOfObject[j + 1])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);

                    Debug.Log($"Point de satisfations gagné : {CurrentQuestList[i].QuestData.PSWin}");
                }
                if (CurrentQuestList[i].QuestData.Object.Count == 1 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Object[j]) >= CurrentQuestList[i].QuestData.NumberOfObject[j])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);

                    Debug.Log($"Point de satisfations gagné : {CurrentQuestList[i].QuestData.PSWin}");
                }
                else
                {
                    Debug.Log("Objectifs incompleted");
                }
            }
        }
    }

    /// <summary>
    /// Donne la commande au client, retire les objet du stockage 
    /// </summary>
    /// <param name="_quest"></param>
    public void GiveQuestObject(Quest _quest)
    {
        for (int i = 0; i < CurrentQuestList.Count; i++)
        {
            if (_quest.QuestData.Name == CurrentQuestList[i].QuestData.Name)
            {
                CurrentQuestList.Remove(CurrentQuestList[i]);
                ResetQuestText.Invoke();
            }
        }

        QuestList.Add(_quest.QuestData);

        for (int j = 0; j < _quest.QuestData.NumberOfObject.Count; j++)
        {
            ItemStorage.Instance.SubstractObject(_quest.QuestData.Object[j], _quest.QuestData.NumberOfObject[j]);
        }

        UpdateAdvancementQuest.Invoke();
    }
}