using System.Collections.Generic;
using UnityEngine;

public class EmployeeList : MonoBehaviour
{
    private static EmployeeList _instance;

    public static EmployeeList Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EmployeeList>();
            }
            return _instance;
        }
    }

    [field :SerializeField] public List<GameObject> Employee { get; private set; }
    
    public void AddEmployee(GameObject i)
    {
        Employee.Add(i);
        i.transform.parent = this.transform;
    }
    
    public void RemoveEmployee(GameObject i)
    {
        Employee.Remove(i);
        Destroy(i.transform);
    }
}
