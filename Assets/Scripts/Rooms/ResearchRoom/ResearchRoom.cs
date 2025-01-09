using System;
using UnityEngine;
using UnityEngine.UI;

public class ResearchRoom : MonoBehaviour, IRoomBehaviour
{
    /// <summary>
    /// Datas of the assembly room.
    /// </summary>
    public ResearchRoomData ResearchRoomData { get; private set; }

    /// <summary>
    /// Reference to the main component of the room.
    /// </summary>
    private Room _roomMain;

    /// <summary>
    /// Current component that the room is researching.
    /// </summary>
    public ComponentData CurrentComponentResearched { get; private set; }

    /// <summary>
    /// Current object that the room is researching.
    /// </summary>
    public ObjectData CurrentObjectResearched { get; private set; }

    /// <summary>
    /// Current chrono of the research.
    /// </summary>
    private int _currentChrono;

    /// <summary>
    /// Current time to research the current object researched.
    /// </summary>
    public int CurrentResearchTime { get; private set; }

    /// <summary>
    /// Notification of the room when a research is launched.
    /// </summary>
    private RoomNotifiction _roomNotification;

    public event Action ResearchCompleted;
    public event Action<int> NewChrono;
    public event Action<ComponentData> ComponentResearchStarted, ComponentResearchStoped, ComponentResearchCompleted;
    public event Action<ObjectData> ObjectResearchStarted, ObjectResearchStoped, ObjectResearchCompleted;

    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        ResearchRoomData = (ResearchRoomData)behaviourData;
        _roomMain = roomMain;

        ResearchManager.Instance.InitNewResearchRoomListeners(this);

        UpgradeRoom(1);
        _roomMain.NewLvl += UpgradeRoom;
    }

    /// <summary>
    /// Called to start the research of a component.
    /// </summary>
    /// <param name="componentToUnlock"> The component to unlock. </param>
    public void StartNewComponentResearch(ComponentData componentToUnlock)
    {
        // Set object researched
        CurrentComponentResearched = componentToUnlock;

        ComponentResearchStarted?.Invoke(CurrentComponentResearched);

        // Set research time
        CurrentResearchTime = CurrentComponentResearched.ResearchTime;

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(ValidateResearch);
        }

        // Consume ressources
        RawMaterialStorage.Instance.SubstractRawMaterials(CurrentComponentResearched.ResearchCost);

        // Launch research
        ChronoManager.Instance.NewSecondTick += ResearchUpdateChrono;
    }

    /// <summary>
    /// Called to start the research of an object.
    /// </summary>
    /// <param name="objectToUnlock"> The object to unlock. </param>
    public void StartNewObjectResearch(ObjectData objectToUnlock)
    {
        // Set object researched
        CurrentObjectResearched = objectToUnlock;

        ObjectResearchStarted?.Invoke(CurrentObjectResearched);

        // Set research time
        CurrentResearchTime = CurrentObjectResearched.ResearchTime;

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(ValidateResearch);
        }

        // Consume ressources
        RawMaterialStorage.Instance.SubstractRawMaterials(CurrentObjectResearched.ResearchCost.RawMaterialCost);
        ItemStorage.Instance.SubstractRecipe(CurrentObjectResearched.ResearchCost.IngredientsCost);

        // Launch research
        ChronoManager.Instance.NewSecondTick += ResearchUpdateChrono;
    }

    /// <summary>
    /// Called each second to update the research chrono.
    /// </summary>
    private void ResearchUpdateChrono()
    {
        if (_currentChrono + 1 >= CurrentResearchTime)
        {
            _currentChrono = 0;
            NewChrono?.Invoke(_currentChrono);

            ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;
            ResearchCompleted?.Invoke();
        }
        else
        {
            _currentChrono++;
            NewChrono?.Invoke(_currentChrono);
        }
    }

    /// <summary>
    /// Called when the player clicked on the notification when the research is completed.
    /// </summary>
    private void ValidateResearch()
    {
        if (CurrentComponentResearched != null)
        {
            ComponentResearchCompleted?.Invoke(CurrentComponentResearched);
            StopResearch();
        }
        else if (CurrentObjectResearched != null)
        {
            ObjectResearchCompleted?.Invoke(CurrentObjectResearched);
            StopResearch();
        }
    }

    /// <summary>
    /// Called to stop the current research.
    /// </summary>
    public void StopResearch()
    {
        ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;

        if (CurrentComponentResearched != null)
        {
            ComponentResearchStoped?.Invoke(CurrentComponentResearched);
        }
        else if (CurrentObjectResearched != null)
        {
            ObjectResearchStoped?.Invoke(CurrentObjectResearched);
        }

        _roomNotification.DesactivateNotification();
        _roomNotification = null;

        _currentChrono = 0;
        CurrentComponentResearched = null;
        CurrentObjectResearched = null;
    }

    private void UpgradeRoom(int newLvl)
    {
        switch (newLvl)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                ScoreManager.Instance.AddRoomLevelMax();
                break;
        }
    }
}
