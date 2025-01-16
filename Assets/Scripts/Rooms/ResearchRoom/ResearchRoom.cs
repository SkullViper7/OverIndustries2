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
    /// A value indicating if a research has started.
    /// </summary>
    public bool ResearchHasStarted { get; private set; }

    /// <summary>
    /// Notification of the room when a research is launched.
    /// </summary>
    private RoomNotifiction _roomNotification;

    /// <summary>
    /// A reference to the lambda who tries to launch a research when there is a change with employess.
    /// </summary>
    private Room.EmployeeDelegate _researchOnHold;

    public event Action ResearchStart, ResearchCompleted, ResearchCantStart;
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

    public void TryStartComponentResearch(ComponentData componentToUnlock)
    {
        // Set component researched
        CurrentComponentResearched = componentToUnlock;

        ComponentResearchStarted?.Invoke(CurrentComponentResearched);

        // Remove research on hold if there is one
        if (_researchOnHold != null)
        {
            _roomMain.EmployeesHaveChanged -= _researchOnHold;
            _researchOnHold = null;
        }

        // Set listener
        _researchOnHold = () => TryStartComponentResearch(CurrentComponentResearched);
        _roomMain.EmployeesHaveChanged += _researchOnHold;

        // Set research time
        CurrentResearchTime = CurrentComponentResearched.ResearchTime;

        // Consume ressources
        RawMaterialStorage.Instance.SubstractRawMaterials(CurrentComponentResearched.ResearchCost);

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(ValidateResearch);
        }

        if (_roomMain.EmployeeAssign.Count > 0)
        {
            // If there is the good employee in the room launch the research
            for (int i = 0; i < _roomMain.EmployeeAssign.Count; i++)
            {
                if (_roomMain.EmployeeAssign[i].EmployeeJob[0].JobType == ResearchRoomData.JobNeeded.JobType)
                {
                    if (!ResearchHasStarted)
                    {
                        ResearchHasStarted = true;

                        ResearchStart?.Invoke();

                        // Launch research
                        ChronoManager.Instance.NewSecondTick += ResearchUpdateChrono;
                    }

                    break;
                }
                else
                {
                    ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;
                    _currentChrono = 0;

                    ResearchCantStart?.Invoke();

                    ResearchHasStarted = false;
                }
            }
        }
        else
        {
            ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;
            _currentChrono = 0;

            ResearchCantStart?.Invoke();

            ResearchHasStarted = false;
        }
    }

    public void TryStartObjectResearch(ObjectData objectToUnlock)
    {
        // Set object researched
        CurrentObjectResearched = objectToUnlock;

        ObjectResearchStarted?.Invoke(CurrentObjectResearched);

        // Remove research on hold if there is one
        if (_researchOnHold != null)
        {
            _roomMain.EmployeesHaveChanged -= _researchOnHold;
            _researchOnHold = null;
        }

        // Set listener
        _researchOnHold = () => TryStartObjectResearch(CurrentObjectResearched);
        _roomMain.EmployeesHaveChanged += _researchOnHold;

        // Set research time
        CurrentResearchTime = CurrentObjectResearched.ResearchTime;

        // Consume ressources
        RawMaterialStorage.Instance.SubstractRawMaterials(CurrentObjectResearched.ResearchCost.RawMaterialCost);
        ItemStorage.Instance.SubstractRecipe(CurrentObjectResearched.ResearchCost.IngredientsCost);

        // If there is already not a notification on the room add a notification and add a listener for when player will clicks on.
        if (_roomNotification == null)
        {
            _roomNotification = RoomNotificationManager.Instance.NewNotification(_roomMain);
            _roomNotification.GetComponent<Button>().onClick.AddListener(ValidateResearch);
        }

        if (_roomMain.EmployeeAssign.Count > 0)
        {
            // If there is the good employee in the room launch the research
            for (int i = 0; i < _roomMain.EmployeeAssign.Count; i++)
            {
                if (_roomMain.EmployeeAssign[i].EmployeeJob[0].JobType == ResearchRoomData.JobNeeded.JobType)
                {
                    if (!ResearchHasStarted)
                    {
                        ResearchHasStarted = true;

                        ResearchStart?.Invoke();

                        // Launch research
                        ChronoManager.Instance.NewSecondTick += ResearchUpdateChrono;
                    }

                    break;
                }
                else
                {
                    ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;
                    _currentChrono = 0;

                    ResearchCantStart?.Invoke();

                    ResearchHasStarted = false;
                }
            }
        }
        else
        {
            ChronoManager.Instance.NewSecondTick -= ResearchUpdateChrono;
            _currentChrono = 0;

            ResearchCantStart?.Invoke();

            ResearchHasStarted = false;
        }
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

        ResearchHasStarted = false;

        // Remove research on hold if there is one
        if (_researchOnHold != null)
        {
            _roomMain.EmployeesHaveChanged -= _researchOnHold;
            _researchOnHold = null;
        }

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
