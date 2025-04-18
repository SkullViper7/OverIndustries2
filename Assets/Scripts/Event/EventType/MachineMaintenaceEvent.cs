using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineMaintenaceEvent : MonoBehaviour
{
    /// <summary>
    /// Cet event alerte le joueur qu'une salle de production a besoin d'une maintenace, 
    /// si la moiti� de son temps est �coul� et que le joueur n'as toujours pas 
    /// fait appel au technicien de maintenance la production de la salle s'arr�te, 
    /// la production de cette salle reprend qlq seconde apr�s l'intervention du technicien.
    /// </summary>

    [SerializeField] private EventData _eventData;
    //[SerializeField] private Employee _maintenanceTechnician;
    [SerializeField] private int _timeToMaintenanceTechnicienToRepaireMachine;

    private List<MachiningRoom> _machiningRooms = new List<MachiningRoom>();
    private int _machiningMaintenance;

    void Start()
    {
        EventManager.Instance.CheckCondition(_eventData);
        EventManager.Instance.EventConditionCompleted += EventComportement;
    }

    public void EventComportement()
    {
        Debug.Log("machine maintenance event");

        for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
        {
            if (GridManager.Instance.InstantiatedRooms[i].RoomData.RoomType == RoomType.Machining)
            {
                _machiningRooms.Add(GridManager.Instance.InstantiatedRooms[i].gameObject.GetComponent<MachiningRoom>());
            }
        }

        _machiningMaintenance = Random.Range(0, _machiningRooms.Count);
        //Debug.Log("*pop up /!|* This machining room need a maintenace, call a Maintenance Technician");

        _machiningRooms[_machiningMaintenance].StopProduction();
        Debug.Log("*pop up /!|* This machining room stop here production, call a Maintenance Technician immediately !");

        //GenerateMaintenaceTechnician();
        //StartCoroutine(WaitMaintenanceTechnician(_machiningMaintenance));
        StartCoroutine(WaitMachineMaintenace());
    }

    //public void GenerateMaintenaceTechnician()
    //{
    //    //Random Name
    //    int i = Random.Range(0, JobProfileGenerator.Instance.RandomNameList.Count);
    //    _maintenanceTechnician.EmployeeName = JobProfileGenerator.Instance.RandomNameList[i];
    //}

    //public void SpawnMaintenaceTechnician()
    //{
    //    _maintenanceTechnician.gameObject.transform.position = _machiningRooms[_machiningMaintenance].transform.position;
    //    _maintenanceTechnician.gameObject.SetActive(true);
    //    StartCoroutine(WaitMachineMaintenace());
    //}

    //IEnumerator WaitMaintenanceTechnician(int _machiningMaintenance)
    //{
    //    yield return new WaitForSeconds(_eventData.Duration / 2);
    //    _machiningRooms[_machiningMaintenance].StopProduction();
    //    Debug.Log("*pop up /!|* This machining room stop here production, call a Maintenance Technician immediately !");
    //}

    IEnumerator WaitMachineMaintenace()
    {
        yield return new WaitForSeconds(_timeToMaintenanceTechnicienToRepaireMachine);
        Debug.Log("*pop up /!|* This machining room maintenance is finish");
        EventManager.Instance.CloseCurrentEvent();
    }
}