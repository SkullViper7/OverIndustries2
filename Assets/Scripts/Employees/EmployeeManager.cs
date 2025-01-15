using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    // Singleton
    private static EmployeeManager _instance = null;
    public static EmployeeManager Instance => _instance;

    /// <summary>
    /// Capacity of the factory.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// List which contains employees.
    /// </summary>
    public List<Employee> Employees { get; private set; } = new();

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

    /// <summary>
    /// Called to get the number of employees.
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfEmployees()
    {
        return Employees.Count;
    }

    /// <summary>
    /// Called to add capacity.
    /// </summary>
    /// <param name="capacityToAdd"> Capacity to add. </param>
    public void AddCapacity(int capacityToAdd)
    {
        Capacity += capacityToAdd;
    }

    /// <summary>
    /// Called to remove capacity.
    /// </summary>
    /// <param name="capacityToRemove"> Capacity to remove. </param>
    public void RemoveCapacity(int capacityToRemove)
    {
        Capacity -= capacityToRemove;
    }

    /// <summary>
    /// Return true if there is remaining space for an employee.
    /// </summary>
    /// <returns></returns>
    public bool IsThereRemainingSpace()
    {
        return Employees.Count + 1 <= Capacity;
    }

    /// <summary>
    /// Called to add a new employee.
    /// </summary>
    /// <param name="newEmployee"> New employee to add. </param>
    public void AddNewEmployee(Employee newEmployee)
    {
        Employees.Add(newEmployee);
    }
}
