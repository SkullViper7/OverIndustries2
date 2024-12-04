using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{

    /// <summary>
    /// Job of the employee.
    /// </summary>
    public List<JobData> EmployeeJob;

    /// <summary>
    /// Name of the employee.
    /// </summary>
    public string EmployeeName;
    
    /// <summary>
    /// Room who employee is assign
    /// </summary>
    public GameObject AssignRoom;
}
