using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchRoom : MonoBehaviour, IRoomBehaviour
{
    [field: SerializeField] public ResearchRoomData ResearchRoomData { get; private set; }
    private Room _roomMain;

    /// <summary>
    /// Reference to the assembly and machining room for add the object ans components unlock
    /// </summary>
    private AssemblyRoom _assemblyRoom;
    private MachiningRoom _machiningRoom;

    [field: SerializeField] public List<ObjectData> ObjectToUnlockList { get; private set; } = new List<ObjectData>();

    /// <summary>
    /// Event for UI
    /// </summary>
    public event System.Action CantUnlockThisObject;
    public event System.Action NewObjectUnlock;
    public event System.Action<int> RoomUpgrade;

    public void Start()
    {
        for (int i = 0; i < ResearchRoomData.ObjectToUnlock.Count; i++)
        {
            ObjectToUnlockList.Add(ResearchRoomData.ObjectToUnlock[i]);
        }
        _assemblyRoom = FindAnyObjectByType<AssemblyRoom>();
        _machiningRoom = FindAnyObjectByType<MachiningRoom>();
    }

    public void InitRoomBehaviour(IRoomBehaviourData behaviourData, Room roomMain)
    {
        ResearchRoomData = (ResearchRoomData)behaviourData;
        _roomMain = roomMain;
    }

    /// <summary>
    /// Check if player can unlock this object
    /// </summary>
    /// <param name="_objectToUnlock"></param>
    public void StartNewResearch(ObjectData _objectToUnlock)
    {
        if (_objectToUnlock.Name == ObjectToUnlockList[0].Name && RawMaterialStorage.Instance.ThereIsEnoughRawMaterialInStorage(_objectToUnlock.CostToUnlock))
        {
            StartCoroutine(WaitUnlockTime(_objectToUnlock));
        }
        else
        {
            CantUnlockThisObject.Invoke();
        }
    }

    /// <summary>
    /// Call UI event and call assembly room and machining room to add new object and new component to here list.
    /// </summary>
    /// <param name="_objectToUnlock"></param>
    public void UnlockObject(ObjectData _objectToUnlock)
    {
        ObjectToUnlockList.Remove(ObjectToUnlockList[0]);
        _assemblyRoom.AddNewAssemblableObject(_objectToUnlock);
        RawMaterialStorage.Instance.SubstractRawMaterials(_objectToUnlock.CostToUnlock);

        for (int i = 0; i < _objectToUnlock.Ingredients.Count; i++)
        {
            _machiningRoom.AddNewManufacturableComponents(_objectToUnlock.Ingredients[i].ComponentData);
        }
        NewObjectUnlock.Invoke();
    }


    /// <summary>
    /// A completer avec l'amélioration des salles
    /// </summary>
    public void UpgradeRoom()
    {

        //RoomUpgrade.Invoke(_level);
    }

    IEnumerator WaitUnlockTime(ObjectData _objectToUnlock)
    {
        RawMaterialStorage.Instance.SubstractRawMaterials(_objectToUnlock.CostToUnlock);
        Debug.Log($"- {_objectToUnlock.CostToUnlock} ");

        yield return new WaitForSeconds(_objectToUnlock.TimeToUnlock);
        UnlockObject(_objectToUnlock);
    }
}
