using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobProfileUI : MonoBehaviour
{
    /// <summary>
    /// Profile UI
    /// </summary>
    public GameObject JobProfile;
    public TextMeshProUGUI EmployeeName;
    public GameObject JobTextParent;
    private GameObject _job;
    private Employee _employee;

    /// <summary>
    /// Fiche Metier UI
    /// </summary>
    [field: SerializeField] public TextMeshProUGUI JobName { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Pay { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Studies { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CareerDevelopment { get; private set; }


    private List<TextMeshProUGUI> _jobTextList = new List<TextMeshProUGUI>();
    public List<JobData> _jobList { get; private set; }

    [SerializeField] private JobProfileGenerator _jobProfileGenerator;

    public void Awake()
    {
        _jobList = new List<JobData>();

        for (int i = 0; i < JobTextParent.transform.childCount; i++)
        {
            _job = JobTextParent.transform.GetChild(i).gameObject;
            _jobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        _jobProfileGenerator.NewName += ShowName;
        _jobProfileGenerator.NumberOfJobs += JobNumber;
        _jobProfileGenerator.NewJob += AddNewShowJob;
    }

    public void ShowProfile(Employee _newEmployee)
    {
        _employee = _newEmployee;

        if (_employee.EmployeeJob.Count == 0)
        {
            _jobProfileGenerator.GenerateProfile();
            JobProfile.SetActive(true);
            _jobList.Clear();
        }
        else
        {
            EmployeeName.text = _employee.EmployeeName;
            for (int i = 0; i < _employee.EmployeeJob.Count; i++)
            {
                _jobTextList[i].text = _employee.EmployeeJob[i].JobName;
                _jobTextList[i].gameObject.SetActive(true);
            }
            JobProfile.SetActive(true);
        }
    }

    public void ShowName(string _name)
    {
        EmployeeName.text = _name;
    }
    public void JobNumber(int _jobNumber)
    {
        for (int i = 0; i < _jobNumber; i++)
        {
            _jobTextList[i].text = _jobList[i].JobName;
            _jobTextList[i].gameObject.SetActive(true);
        }
    }

    public void AddNewShowJob(JobData _job)
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

    public void ShowFicheMetier(JobData _job)
    {
        JobName.text = _job.JobName;
        Description.text = _job.Description;

        if (_job.Pay.Count == 3)
        {
            Pay.text = $"{_job.Pay[0]} \n{_job.Pay[1]} \n{_job.Pay[2]}";
        }
        else
        {
            Pay.text = $"{_job.Pay[0]} \n{_job.Pay[1]}";
        }

        if (_job.Studies.Count == 5)
        {
            Studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]}";
        }
        else if (_job.Studies.Count == 6)
        {
            Studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]} \n{_job.Studies[5]}";
        }
        else
        {
            Studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]} \n{_job.Studies[4]} \n{_job.Studies[5]} \n{_job.Studies[6]}";
        }

        if (_job.CareerDevelopment.Count == 3)
        {
            CareerDevelopment.text = $"{_job.CareerDevelopment[0]} \n{_job.CareerDevelopment[1]} \n{_job.CareerDevelopment[2]}";
        }
        else
        {
            CareerDevelopment.text = $"{_job.CareerDevelopment[0]} \n{_job.CareerDevelopment[1]} \n{_job.CareerDevelopment[2]} \n{_job.CareerDevelopment[3]} \n{_job.CareerDevelopment[4]}";
        }
    }
}
