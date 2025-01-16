using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    /// <summary>
    /// Data of the room.
    /// </summary>
    public RoomData RoomData { get; private set; }

    /// <summary>
    /// Data of the room behaviour.
    /// </summary>
    public IRoomBehaviourData RoomBehaviourData { get; private set; }

    /// <summary>
    /// Component of the room behaviour.
    /// </summary>
    public IRoomBehaviour RoomBehaviour { get; private set; }

    /// <summary>
    /// Current lvl of the room.
    /// </summary>
    public int CurrentLvl { get; private set; }

    private bool _isUpgrading;

    private bool _isAlreadyUpgrading;

    /// <summary>
    /// Position of the room in the grid (leftmost slot of the room)
    /// </summary>
    public Vector2 RoomPosition { get; private set; }

    /// <summary>
    /// Prefab of the room with mesh in children.
    /// </summary>
    private GameObject _currentVisualRoom;

    /// <summary>
    /// Animator of the door.
    /// </summary>
    private Animator _doorAnimator;

    /// <summary>
    /// List of employee assign to the room.
    /// </summary>
    [field: SerializeField]
    public List<Employee> EmployeeAssign { get; private set; } = new List<Employee>();

    /// <summary>
    /// Events to get the lvl when the room is upgraded.
    /// </summary>
    public delegate void UpgradeDelegate(int newLvl);
    public event UpgradeDelegate NewLvl;

    public delegate void EmployeeDelegate();
    public event EmployeeDelegate EmployeesHaveChanged;

    public event Action OnInitialized, UpgradeStart, UpgradeEnd;

    /// <summary>
    /// Called at the start to initialize the room.
    /// </summary>
    /// <param name="roomData"> Generic data of the room. </param>
    /// <param name="roomBehaviourData"> Specific data of the room's behaviour. </param>
    /// <param name="gridPosition"> Position of the room in the world. </param>
    public void InitRoom(RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 gridPosition)
    {
        gameObject.name = roomData.RoomType.ToString();
        RoomData = roomData;
        RoomBehaviourData = roomBehaviourData;
        RoomPosition = gridPosition;

        transform.position = GridManager.Instance.ConvertGridPosIntoWorldPos(RoomPosition);

        switch (roomData.RoomType)
        {
            case RoomType.Delivery:
                DeliveryRoom deliveryRoom = (DeliveryRoom)gameObject.AddComponent(typeof(DeliveryRoom));
                RoomBehaviour = deliveryRoom;
                deliveryRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)gameObject.AddComponent(typeof(MachiningRoom));
                RoomBehaviour = machiningRoom;
                machiningRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Storage:
                StorageRoom storageRoom = (StorageRoom)gameObject.AddComponent(typeof(StorageRoom));
                RoomBehaviour = storageRoom;
                storageRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Assembly:
                AssemblyRoom assemblyRoom = (AssemblyRoom)gameObject.AddComponent(typeof(AssemblyRoom));
                RoomBehaviour = assemblyRoom;
                assemblyRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Elevator:
                Elevator elevator = (Elevator)gameObject.AddComponent(typeof(Elevator));
                RoomBehaviour = elevator;
                elevator.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Director:
                DirectorRoom directorRoom = (DirectorRoom)gameObject.AddComponent(typeof(DirectorRoom));
                RoomBehaviour = directorRoom;
                directorRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Research:
                ResearchRoom researchRoom = (ResearchRoom)gameObject.AddComponent(typeof(ResearchRoom));
                RoomBehaviour = researchRoom;
                researchRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Recycling:
                RecyclingRoom recyclingRoom = (RecyclingRoom)gameObject.AddComponent(typeof(RecyclingRoom));
                RoomBehaviour = recyclingRoom;
                recyclingRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.Rest:
                RestRoom restRoom = (RestRoom)gameObject.AddComponent(typeof(RestRoom));
                RoomBehaviour = restRoom;
                restRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;

            case RoomType.RawMaterialStorage:
                RawMaterialStorageRoom rawMaterialStorageRoom = (RawMaterialStorageRoom)gameObject.AddComponent(typeof(RawMaterialStorageRoom));
                RoomBehaviour = rawMaterialStorageRoom;
                rawMaterialStorageRoom.InitRoomBehaviour(roomBehaviourData, this);
                break;
        }

        OnInitialized?.Invoke();
    }

    /// <summary>
    /// Called to initialize the animator of the door.
    /// </summary>
    /// <param name="doorAnimator"></param>
    public void InitAnimator(Animator doorAnimator)
    {
        _doorAnimator = doorAnimator;
        UpgradeRoom();
    }

    /// <summary>
    /// Called to upgrade the room.
    /// </summary>
    public void UpgradeRoom()
    {
        switch (CurrentLvl)
        {
            case 0:
                CurrentLvl++;
                NewLvl?.Invoke(CurrentLvl);
                StartCoroutine(UpgradeVisualRoom(CurrentLvl));
                break;
            case 1:
                if (RawMaterialStorage.Instance.ThereIsEnoughRawMaterialInStorage(RoomData.UpgradeCostToLvl2))
                {
                    RawMaterialStorage.Instance.SubstractRawMaterials(RoomData.UpgradeCostToLvl2);
                    CurrentLvl++;
                    NewLvl?.Invoke(CurrentLvl);
                    StartCoroutine(UpgradeVisualRoom(CurrentLvl));
                }
                break;
            case 2:
                if (RawMaterialStorage.Instance.ThereIsEnoughRawMaterialInStorage(RoomData.UpgradeCostToLvl3))
                {
                    UIManager.Instance.UpgradeButton.SetActive(false);
                    RawMaterialStorage.Instance.SubstractRawMaterials(RoomData.UpgradeCostToLvl3);
                    CurrentLvl++;
                    NewLvl?.Invoke(CurrentLvl);
                    StartCoroutine(UpgradeVisualRoom(CurrentLvl));
                }
                break;
        }
    }

    /// <summary>
    /// Reset employee waypoint when the room is upgrade
    /// </summary>
    private void SetEmployeeAssign()
    {
        if (EmployeeAssign != null)
        {
            for (int i = 0; i < EmployeeAssign.Count; i++)
            {
                EmployeeAssign[i].SetRoutineParameter();
            }
        }
    }

    /// <summary>
    /// Add this employee in the list of employee assign in this room
    /// </summary>
    /// <param name="employee"></param>
    public void AssignEmployeeInThisRoom(Employee employee)
    {
        //Check if the room can add a employee
        if (EmployeeAssign.Count != RoomData.Capacity)
        {
            //Check if this employee is not already in this room
            if (employee.AssignRoom != this.gameObject)
            {
                EmployeeAssign.Add(employee);
                EmployeesHaveChanged?.Invoke();
            }
        }
    }

    /// <summary>
    /// Remove this employee in the list of employee assign in this room
    /// </summary>
    /// <param name="employee"></param>
    public void RemoveAssignEmployeeInThisRoom(Employee employee)
    {
        if (employee != null && EmployeeAssign.Contains(employee))
        {
            EmployeeAssign.Remove(employee);
            EmployeesHaveChanged?.Invoke();
        }
    }

    private IEnumerator UpgradeVisualRoom(int currentLvl)
    {
        switch (currentLvl)
        {
            case 1:
                {
                    if (_isUpgrading)
                    {
                        _isAlreadyUpgrading = true;
                    }
                    _isUpgrading = true;

                    UpgradeStart?.Invoke();

                    _doorAnimator.Play("RoomOpening", 0, 0f);

                    GameObject newVisualRoom = Instantiate(RoomData.RoomLvl1, transform);
                    _currentVisualRoom = newVisualRoom;

                    // Attendre un frame pour garantir que l'animation démarre
                    yield return null;

                    // Récupérer les informations sur l'état en cours dans le layer 0
                    AnimatorStateInfo stateInfo = _doorAnimator.GetCurrentAnimatorStateInfo(0);

                    // Attendre la moitié de l'animation
                    yield return new WaitForSeconds(stateInfo.length);

                    if (!_isAlreadyUpgrading)
                    {
                        UpgradeEnd?.Invoke();
                    }
                    else
                    {
                        _isAlreadyUpgrading = false;
                    }
                    _isUpgrading = false;

                    break;
                }
            case 2:
                {
                    if (_isUpgrading)
                    {
                        _isAlreadyUpgrading = true;
                    }
                    _isUpgrading = true;

                    UpgradeStart?.Invoke();

                    _doorAnimator.Play("RoomUpgrade", 0, 0f);

                    // Attendre un frame pour garantir que l'animation démarre
                    yield return null;

                    // Récupérer les informations sur l'état en cours dans le layer 0
                    AnimatorStateInfo stateInfo = _doorAnimator.GetCurrentAnimatorStateInfo(0);

                    // Attendre la moitié de l'animation
                    yield return new WaitForSeconds(stateInfo.length / 2f);

                    if (_currentVisualRoom != null)
                    {
                        _currentVisualRoom.SetActive(false);
                    }

                    GameObject newVisualRoom = Instantiate(RoomData.RoomLvl2, transform);
                    _currentVisualRoom = newVisualRoom;
                    SetEmployeeAssign();

                    // Attendre l'autre moitié de l'animation
                    yield return new WaitForSeconds(stateInfo.length / 2f);

                    if (!_isAlreadyUpgrading)
                    {
                        UpgradeEnd?.Invoke();
                    }
                    else
                    {
                        _isAlreadyUpgrading = false;
                    }
                    _isUpgrading = false;

                    break;
                }
            case 3:
                {
                    if (_isUpgrading)
                    {
                        _isAlreadyUpgrading = true;
                    }
                    _isUpgrading = true;

                    UpgradeStart?.Invoke();

                    _doorAnimator.Play("RoomUpgrade", 0, 0f);

                    // Attendre un frame pour garantir que l'animation démarre
                    yield return null;

                    // Récupérer les informations sur l'état en cours dans le layer 0
                    AnimatorStateInfo stateInfo = _doorAnimator.GetCurrentAnimatorStateInfo(0);

                    // Attendre la moitié de l'animation
                    yield return new WaitForSeconds(stateInfo.length / 2f);

                    if (_currentVisualRoom != null)
                    {
                        _currentVisualRoom.SetActive(false);
                    }

                    GameObject newVisualRoom = Instantiate(RoomData.RoomLvl3, transform);
                    _currentVisualRoom = newVisualRoom;
                    SetEmployeeAssign();

                    // Attendre l'autre moitié de l'animation
                    yield return new WaitForSeconds(stateInfo.length / 2f);

                    if (!_isAlreadyUpgrading)
                    {
                        UpgradeEnd?.Invoke();
                    }
                    else
                    {
                        _isAlreadyUpgrading = false;
                    }
                    _isUpgrading = false;

                    break;
                }
        }
    }
}