using UnityEngine;

public class HireEmployee : MonoBehaviour
{
    private DirectorRoom _directorRoom;
    private int _poolEmployeeID;

    void Start()
    {
        _directorRoom = DirectorRoom.Instance;
    }

    /// <summary>
    /// Give the employee to the button 
    /// </summary>
    public void SetHireEmployee(int i)
    {
        HiredEmployee(_directorRoom.RecrutementList[i].GetComponent<Employee>());
    }

    /// <summary>
    /// Set this new employee
    /// </summary>
    /// <param name="_employeeToHire"></param>
    public void HiredEmployee(Employee _employeeToHire)
    {
        _employeeToHire.IsHired = true;
        _employeeToHire.transform.GetChild(0).gameObject.SetActive(true);
        EmployeeList.Instance.AddEmployee(_employeeToHire.gameObject);

        _employeeToHire.AssignRoom = this.gameObject;
        _directorRoom.RoomMain.EmployeeAssign.Add(_employeeToHire);

        _employeeToHire.SetEmployee();
    }

    public void RefuseEmployee(Employee _employee)
    {
        ObjectPoolManager.Instance.ReturnObjectToThePool(_poolEmployeeID, _employee.gameObject);
    }

    public void CloseHireEmployee()
    {
        for (int i = 0; i < _directorRoom.RecrutementList.Count; i++)
        {
            if (_directorRoom.RecrutementList[i].GetComponent<Employee>().IsHired == false)
            {

                Employee employee = _directorRoom.RecrutementList[i].GetComponent<Employee>();
                employee.EmployeeJob.Clear();
                employee.gameObject.SetActive(false);
                employee.EmployeeName = null;

                ObjectPoolManager.Instance.ReturnObjectToThePool(_poolEmployeeID, employee.gameObject);
            }
        }
        _directorRoom.RecrutementList.Clear();

    }
}
