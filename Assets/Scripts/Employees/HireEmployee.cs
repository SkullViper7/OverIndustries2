using System.Collections.Generic;
using UnityEngine;

public class HireEmployee : MonoBehaviour
{
    [field: SerializeField] private List<SetButtonHireEmployee> _buttonHireList;
    [field: SerializeField] private List<SetButtonHireEmployee> _buttonRefuseList;

    void Start()
    {
        JobProfileUI.Instance.UpdateProfilList += SetButton;
    }

    /// <summary>
    /// Give the employee to the button 
    /// </summary>
    public void SetButton()
    {
        int j = 0;
        for (int i = 0; i < JobProfileUI.Instance.JobProfileList.Count; i++)
        {
            if (JobProfileUI.Instance.JobProfileList[i].activeInHierarchy)
            {
                _buttonHireList[i].Employee = DirectorRoom.Instance.RecrutementList[j].GetComponent<Employee>();
                _buttonRefuseList[i].Employee = DirectorRoom.Instance.RecrutementList[j].GetComponent<Employee>();
                j++;
            }
        }
    }

    public void CheckRoomCapacity(GameObject jobProfile)
    {
        if (DirectorRoom.Instance.RoomMain.EmployeeAssign.Count < DirectorRoom.Instance.RoomMain.RoomData.Capacity)
        {
            jobProfile.SetActive(false);
        }
        else
        {
            DirectorRoom.Instance.MaxEmployeeAssign();
        }
    }

    /// <summary>
    /// Start HiredEmployee with button
    /// </summary>
    public void HiredEmployeeButton(int i)
    {
        HiredEmployee(_buttonHireList[i].Employee);
    }

    /// <summary>
    /// Set this new employee
    /// </summary>
    /// <param name="employeeToHire"></param>
    void HiredEmployee(Employee employeeToHire)
    {
        if (DirectorRoom.Instance.RoomMain.EmployeeAssign.Count < DirectorRoom.Instance.RoomMain.RoomData.Capacity)
        {
            employeeToHire.IsHired = true;
            employeeToHire.transform.GetChild(0).gameObject.SetActive(true);
            EmployeeList.Instance.AddEmployee(employeeToHire.gameObject);
            DirectorRoom.Instance.RecrutementList.Remove(employeeToHire.gameObject);

            employeeToHire.AssignRoom = DirectorRoom.Instance.gameObject;
            DirectorRoom.Instance.RoomMain.EmployeeAssign.Add(employeeToHire);

            employeeToHire.SetEmployee();
            employeeToHire.SetHatColor();

            SetButton();
            ScoreManager.Instance.AddEmployee();
        }
        else
        {
            DirectorRoom.Instance.MaxEmployeeAssign();
        }
    }

    /// <summary>
    /// Start refuse employee with button
    /// </summary>
    public void RefuseEmployeeButton(int i)
    {
        RefuseEmployee(_buttonRefuseList[i].Employee);
    }

    void RefuseEmployee(Employee employee)
    {
        employee.EmployeeJob.Clear();
        employee.gameObject.SetActive(false);
        employee.EmployeeName = null;

        ObjectPoolManager.Instance.ReturnObjectToThePool(DirectorRoom.Instance.PoolEmployeeID, employee.gameObject);
        DirectorRoom.Instance.RecrutementList.Remove(employee.gameObject);

        SetButton();
    }

    public void CloseHireEmployee()
    {
        for (int i = 0; i < DirectorRoom.Instance.RecrutementList.Count; i++)
        {
            DirectorRoom.Instance.RecrutementList[i].SetActive(false);
        }
    }
}
