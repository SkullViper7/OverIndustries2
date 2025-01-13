using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance = null;
    public static QuestManager Instance => _instance;

    [field: SerializeField] public List<QuestData> QuestList { get; private set; }
    [field: SerializeField] public List<QuestData> CurrentQuestList { get; private set; } = new List<QuestData>();
    [field: SerializeField, Tooltip("Maximum number of current quest")] public int MaxCurrentQuest { get; private set; }

    [Space, Header("Quest Stats")]
    private int _objectUnlock;
    private int _componentUnlock;
    private int _checkQuest;

    [field: SerializeField, Tooltip("Minimum number of object need to all quest")] public int MinNumberQuestObject { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of object need to all quest")] public int MaxNumberQuestObject { get; private set; }
    [field: SerializeField, Tooltip("Minimum number of component need to all quest")] public int MinNumberQuestComponent { get; private set; }
    [field: SerializeField, Tooltip("Maximum number of component need to all quest")] public int MaxNumberQuestComponent { get; private set; }

    public event System.Action<QuestData> NewQuestGenerate;
    public event System.Action UpdateAdvancementQuest, ResetQuestText, MaxQuest;
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
            int randomQuest = Random.Range(0, QuestList.Count);
            QuestData _newQuest = QuestList[randomQuest];

            for (int i = 0; i < _newQuest.NumberOfObject.Count; i++)
            {
                int randomNumberOfThisObject = Random.Range(MinNumberQuestObject, MaxNumberQuestObject);
                _newQuest.NumberOfObject[i] = randomNumberOfThisObject;
            }

            for (int j = 0; j < _newQuest.NumberOfComponent.Count; j++)
            {
                int randomNumberOfThisComponent = Random.Range(MinNumberQuestComponent, MaxNumberQuestComponent);
                _newQuest.NumberOfComponent[j] = randomNumberOfThisComponent;
            }

            for (int a = 0; a < _newQuest.Objects.Count; a++)
            {
                for (int b = 0; b < ResearchManager.Instance.ManufacturableObjects.Count; b++)
                {
                    if (_newQuest.Objects[a].Name == ResearchManager.Instance.ManufacturableObjects[b].Name)
                    {
                        _objectUnlock++;
                    }
                }
            }

            for (int c = 0; c < _newQuest.Component.Count; c++)
            {
                for (int d = 0; d < ResearchManager.Instance.ManufacturableComponents.Count; d++)
                {
                    if (_newQuest.Component[c].Name == ResearchManager.Instance.ManufacturableComponents[d].Name)
                    {
                        _componentUnlock++;
                    }
                }
            }

            //check if quest product are unlock
            if (_newQuest.Component.Count == _componentUnlock && _newQuest.Objects.Count == _objectUnlock)
            {
                NewQuestGenerate.Invoke(_newQuest);

                _componentUnlock = 0;
                _objectUnlock = 0;
            }
            else
            {
                _checkQuest++;

                _componentUnlock = 0;
                _objectUnlock = 0;

                if(_checkQuest < QuestList.Count)
                {
                    GenerateNewQuest();
                }
                else
                {
                    _checkQuest = 0;

                    MaxQuest?.Invoke();
                }
            }
        }
        else
        {
            MaxQuest?.Invoke();
        }
    }

    public void NewCurrentQuest(QuestData _quest)
    {
        CurrentQuestList.Add(_quest);
        QuestList.Remove(_quest);
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
            for (int j = 0; j < CurrentQuestList[i].Objects.Count; j++)
            {
                if (CurrentQuestList[i].Objects.Count > 1
                    && j == 0 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j])
                    >= CurrentQuestList[i].NumberOfObject[j]
                    && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j + 1])
                    >= CurrentQuestList[i].NumberOfObject[j + 1])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
                }
                if (CurrentQuestList[i].Objects.Count > 2
                    && j == 0 && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j])
                    >= CurrentQuestList[i].NumberOfObject[j]
                    && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j + 1])
                    >= CurrentQuestList[i].NumberOfObject[j + 1]
                    && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j + 2])
                    >= CurrentQuestList[i].NumberOfObject[j + 2])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
                }
                if (CurrentQuestList[i].Objects.Count == 1
                    && ItemStorage.Instance.ReturnNumberOfThisObject(CurrentQuestList[i].Objects[j])
                    >= CurrentQuestList[i].NumberOfObject[j])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
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
            for (int j = 0; j < CurrentQuestList[i].Component.Count; j++)
            {
                if (CurrentQuestList[i].Component.Count > 1 && j == 0 &&
                    ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j])
                    >= CurrentQuestList[i].NumberOfComponent[j] &&
                    ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j + 1])
                    >= CurrentQuestList[i].NumberOfComponent[j + 1])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
                }
                if (CurrentQuestList[i].Component.Count > 2 && j == 0 &&
                    ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j])
                    >= CurrentQuestList[i].NumberOfComponent[j]
                    && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j + 1])
                    >= CurrentQuestList[i].NumberOfComponent[j + 1]
                    && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j + 2])
                    >= CurrentQuestList[i].NumberOfComponent[j + 2])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
                }
                if (CurrentQuestList[i].Component.Count == 1
                    && ItemStorage.Instance.ReturnNumberOfThisComponent(CurrentQuestList[i].Component[j])
                    >= CurrentQuestList[i].NumberOfComponent[j])
                {
                    QuestComplited.Invoke(i);
                    ScoreManager.Instance.AddPS(CurrentQuestList[i].PSWin);
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
            if (_quest.QuestData.Name == CurrentQuestList[i].Name)
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