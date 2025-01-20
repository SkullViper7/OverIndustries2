using System;
using System.Collections.Generic;
using System.Linq;
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
    [field : SerializeField]
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

    /// <summary>
    /// Called to sort employees by ascending or descending order of their names.
    /// </summary>
    /// <param name="inAscendingOrder"></param>
    public void SortByName(bool inAscendingOrder)
    {
        if (inAscendingOrder)
        {
            Employees = Employees.OrderBy(employee => employee.EmployeeName, StringComparer.OrdinalIgnoreCase).ToList();
        }
        else
        {
            Employees = Employees.OrderByDescending(employee => employee.EmployeeName, StringComparer.OrdinalIgnoreCase).ToList();
        }
    }

    /// <summary>
    /// Called to sort employees by ascending or descending order of their jobs.
    /// </summary>
    /// <param name="inAscendingOrder"></param>
    public void SortByJob(bool inAscendingOrder)
    {
        if (inAscendingOrder)
        {
            Employees = Employees.OrderBy(employee => employee.EmployeeJob[0].JobName, StringComparer.OrdinalIgnoreCase).ToList();
        }
        else
        {
            Employees = Employees.OrderByDescending(employee => employee.EmployeeJob[0].JobName, StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
