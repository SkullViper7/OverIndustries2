using System.Collections.Generic;
using UnityEngine;

public class SetEmployeeParameter : MonoBehaviour
{
    private List<JobData> _jobList = new List<JobData>();

    private JobProfileGenerator _jobProfileGenerator;

    private string _employeeName;
    private int _jobsNumber;


    public void Start()
    {
        _jobProfileGenerator = gameObject.GetComponent<JobProfileGenerator>();

        _jobProfileGenerator.NewName += SetName;
        _jobProfileGenerator.NumberOfJobs += JobNumber;
        _jobProfileGenerator.NewJob += AddJob;
    }

    public void SetThisEmployee(Employee _employee)
    {
        _employee.EmployeeName = _employeeName;
        for (int i = 0; i < _jobsNumber; i++)
        {
            _employee.EmployeeJob.Add(_jobList[i]);
        }
    }
    public void SetName(string _name)
    {
        _employeeName = _name;
    }
    public void JobNumber(int _jobNumber)
    {
        _jobsNumber = _jobNumber;
    }

    public void AddJob(JobData _newJob)
    {
        _jobList.Add(_newJob);
    }
}
