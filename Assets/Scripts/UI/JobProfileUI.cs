using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobProfileUI : MonoBehaviour
{
    [Header("UI Reference for job profile")]
    [SerializeField] private GameObject _jobProfile;
    [SerializeField] private TextMeshProUGUI _employeeName;
    [SerializeField] private GameObject _jobTextParent;

    [Space, Header("UI Reference for fiche métier")]
    [SerializeField] private TextMeshProUGUI _jobName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _pay;
    [SerializeField] private TextMeshProUGUI _studies;
    [SerializeField] private TextMeshProUGUI _careerDevelopment;

    private GameObject _job;
    private Employee _employee;
    private List<TextMeshProUGUI> _jobTextList = new List<TextMeshProUGUI>();

    /// <summary>
    /// Jobs list of  this employee 
    /// </summary>
    public List<JobData> _jobList { get; private set; } = new List<JobData>();

    [SerializeField] private JobProfileGenerator _jobProfileGenerator;

    public void Awake()
    {
        for (int i = 0; i < _jobTextParent.transform.childCount; i++)
        {
            _job = _jobTextParent.transform.GetChild(i).gameObject;
            _jobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        _jobProfileGenerator.NewName += ShowName;
        _jobProfileGenerator.NumberOfJobs += JobNumber;
        _jobProfileGenerator.NewJob += AddNewJob;
    }

    /// <summary>
    /// Set job for job profile UI for this employee and show them
    /// </summary>
    /// <param name="_employeeSelected"></param>
    public void ShowProfile(Employee _employeeSelected)
    {
        _employee = _employeeSelected;

        if (_employee.EmployeeJob.Count == 0)
        {
            _jobProfileGenerator.GenerateProfile();
            _jobProfile.SetActive(true);
            _jobList.Clear();
        }
        else
        {
            _employeeName.text = _employee.EmployeeName;
            for (int i = 0; i < _employee.EmployeeJob.Count; i++)
            {
                _jobTextList[i].text = _employee.EmployeeJob[i].JobName;
                _jobTextList[i].gameObject.SetActive(true);
            }
            _jobProfile.SetActive(true);
        }
    }

    /// <summary>
    /// Listen jobProfileGenerator and set employee name
    /// </summary>
    /// <param name="_name"></param>
    public void ShowName(string _name)
    {
        _employeeName.text = _name;
    }

    /// <summary>
    /// Listen jobProfileGenerator for number of job has this employee
    /// </summary>
    /// <param name="_jobNumber"></param>
    public void JobNumber(int _jobNumber)
    {
        for (int i = 0; i < _jobNumber; i++)
        {
            _jobTextList[i].text = _jobList[i].JobName;
            _jobTextList[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Listen jobProfileGenerator, add all employee job to job list
    /// </summary>
    /// <param name="_job"></param>
    public void AddNewJob(JobData _job)
    {
        _jobList.Add(_job);
    }

    /// <summary>
    /// Give the job data to the button 
    /// </summary>
    public void SetButtonFicheMetier(int i)
    {
        ShowFicheMetier(_employee.EmployeeJob[i]);
    }

    /// <summary>
    /// Show this job here fiche métier
    /// </summary>
    /// <param name="_job"></param>
    public void ShowFicheMetier(JobData _job)
    {
        _jobName.text = _job.JobName;
        _description.text = _job.Description;

        if (_job.Pay.Count == 3)
        {
            _pay.text = $"{_job.Pay[0]} \n{_job.Pay[1]} \n{_job.Pay[2]}";
        }
        else
        {
            _pay.text = $"{_job.Pay[0]} \n{_job.Pay[1]}";
        }

        if (_job.Studies.Count == 5)
        {
            _studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]}";
        }
        else if (_job.Studies.Count == 6)
        {
            _studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]} \n{_job.Studies[5]}";
        }
        else
        {
            _studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]} \n{_job.Studies[5]} \n{_job.Studies[6]}";
        }

        if (_job.CareerDevelopment.Count == 3)
        {
            _careerDevelopment.text = $"{_job.CareerDevelopment[0]} \n{_job.CareerDevelopment[1]} \n{_job.CareerDevelopment[2]}";
        }
        else
        {
            _careerDevelopment.text = $"{_job.CareerDevelopment[0]} \n{_job.CareerDevelopment[1]} \n{_job.CareerDevelopment[2]} \n{_job.CareerDevelopment[3]} \n{_job.CareerDevelopment[4]}";
        }
    }
}
