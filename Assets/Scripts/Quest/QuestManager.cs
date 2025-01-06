using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance = null;
    public static QuestManager Instance => _instance;

    [field: SerializeField] public List<QuestData> QuestList { get; private set; }
    public List<Quest> CurrentQuestList { get; private set; } = new List<Quest>();
    [field: SerializeField, Tooltip("Maximum number of current quest")] public int MaxCurrentQuest { get; private set; }

    [Space, Header("Quest Stats")]
    private Quest _newQuest;
    [field: SerializeField, Tooltip("Minimum number of object need to all quest")] public int MinNumberQuestObject { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of object need to all quest")] public int MaxNumberQuestObject { get; private set; }
     [field: SerializeField, Tooltip("Minimum number of component need to all quest")] public int MinNumberQuestComponent { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of component need to all quest")] public int MaxNumberQuestComponent { get; private set; }

    [Space, Header("Quest Instanciate")]
    [SerializeField] private GameObject _defaultQuest;
    [SerializeField] private Transform _questContainer;

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
        ItemStorage.Instance.StorageObjectChanged += QuestObjectAdvancement;
        ItemStorage.Instance.StorageComponentChanged += QuestComponentAdvancement;
    }

    /// <summary>
    /// Choisi une nouvelle quête, chaque quête garde son nom, sa description et son objet associer, seul le nombre d'objets nécessaire change
    /// </summary>
    public void GenerateNewQuest()
    {
        if (QuestList.Count > 0 && MaxCurrentQuest > CurrentQuestList.Count)
        {
            _newQuest = Instantiate(_defaultQuest.GetComponent<Quest>(), _questContainer);

            int randomQuest = Random.Range(0, QuestList.Count);
            _newQuest.GiveQuest(QuestList[randomQuest]);
            Debug.Log("CreateQuest");


            for (int i = 0; i < _newQuest.QuestData.NumberOfObject.Count; i++)
            {
                Debug.Log("NumberObject");

                int randomNumberOfThisObject = Random.Range(MinNumberQuestObject, MaxNumberQuestObject);
                _newQuest.QuestData.NumberOfObject[i] = randomNumberOfThisObject;
            }
            
            for (int j = 0; j < _newQuest.QuestData.NumberOfComponent.Count; j++)
            {
                Debug.Log("NumberComponent");

                int randomNumberOfThisComponent = Random.Range(MinNumberQuestComponent, MaxNumberQuestComponent);
                _newQuest.QuestData.NumberOfComponent[j] = randomNumberOfThisComponent;
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
    /// Donne l'avancement de la quête aec objets pour chaque objectifs + check si il est accompli
    /// </summary>
    /// <param name="_actualStockOfThisObject"></param>
    public void QuestObjectAdvancement(ObjectType _object, int _actualStockOfThisObject)
    {
        UpdateAdvancementQuest.Invoke();

        for (int i = 0; i < CurrentQuestList.Count; i++)
        {
            for (int j = 0; j < CurrentQuestList[i].QuestData.Objects.Count; j++)
            {
                if (CurrentQuestList[i].QuestData.Objects.Count > 1 && j == 0 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j]) >= CurrentQuestList[i].QuestData.NumberOfObject[j] && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j + 1]) >= CurrentQuestList[i].QuestData.NumberOfObject[j + 1])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                if (CurrentQuestList[i].QuestData.Objects.Count > 2 && j == 0 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j]) >= CurrentQuestList[i].QuestData.NumberOfObject[j] && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j + 1]) >= CurrentQuestList[i].QuestData.NumberOfObject[j + 1] && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j + 2]) >= CurrentQuestList[i].QuestData.NumberOfObject[j + 2])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                if (CurrentQuestList[i].QuestData.Objects.Count == 1 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].QuestData.Objects[j]) >= CurrentQuestList[i].QuestData.NumberOfObject[j])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                else
                {
                    Debug.Log("Objectifs incompleted");
                }
            }
        }
    }
    
    /// <summary>
    /// Donne l'avancement de la quête avec composant pour chaque objectifs + check si il est accompli
    /// </summary>
    /// <param name="_actualStockOfThisObject"></param>
    public void QuestComponentAdvancement(ComponentType _component, int _actualStockOfThisComponent)
    {
        UpdateAdvancementQuest.Invoke();

        for (int i = 0; i < CurrentQuestList.Count; i++)
        {
            for (int j = 0; j < CurrentQuestList[i].QuestData.Component.Count; j++)
            {
                if (CurrentQuestList[i].QuestData.Component.Count > 1 && j == 0 && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j] && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j + 1]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j + 1])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                if (CurrentQuestList[i].QuestData.Component.Count > 2 && j == 0 && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j] && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j + 1]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j + 1] && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j + 2]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j + 2])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                if (CurrentQuestList[i].QuestData.Component.Count == 1 && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].QuestData.Component[j]) >= CurrentQuestList[i].QuestData.NumberOfComponent[j])
                {
                    Debug.Log("Objectifs completed");
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].QuestData.PSWin);
                }
                else
                {
                    Debug.Log("Objectifs incompleted");
                }
            }
        }
    }

    /// <summary>
    /// Donne la commande au client, retire les objet et composants du stockage 
    /// </summary>
    /// <param name="_quest"></param>
    public void GiveQuestProduct(Quest _quest)
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
            ItemStorage.Instance.SubstractObjects(_quest.QuestData.Objects[j], _quest.QuestData.NumberOfObject[j]);
        }
        
        for (int j = 0; j < _quest.QuestData.NumberOfComponent.Count; j++)
        {
            ItemStorage.Instance.SubstractComponents(_quest.QuestData.Component[j], _quest.QuestData.NumberOfComponent[j]);
        }

        UpdateAdvancementQuest.Invoke();
    }
}