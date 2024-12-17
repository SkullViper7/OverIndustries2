using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobProfileUI : MonoBehaviour
{
    [Header("UI Reference for hired job profile")]
    [SerializeField] private GameObject _jobProfileHired;
    [SerializeField] private List<TextMeshProUGUI> _employeeNameHired;
    [SerializeField] private List<GameObject> _jobTextParentHired;

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
    private int _jobChoose;

    private List<TextMeshProUGUI> _employee1JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee2JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee3JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee4JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee5JobTextList = new List<TextMeshProUGUI>();

    /// <summary>
    /// reference
    /// </summary>
    private JobProfileGenerator _jobProfileGenerator;
    private DirectorRoom _directorRoom;

    public void Start()
    {
        SetTextList();

        _jobProfileGenerator = JobProfileGenerator.Instance;
        _directorRoom = DirectorRoom.Instance;
    }

    public void SetTextList()
    {
        _employee1JobTextList.Clear();
        _employee2JobTextList.Clear();
        _employee3JobTextList.Clear();
        _employee4JobTextList.Clear();
        _employee5JobTextList.Clear();

        for (int i = 0; i < _jobTextParentHired[0].transform.childCount; i++)
        {
            _job = _jobTextParentHired[0].transform.GetChild(i).gameObject;
            _employee1JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[1].transform.childCount; i++)
        {
            _job = _jobTextParentHired[1].transform.GetChild(i).gameObject;
            _employee2JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[2].transform.childCount; i++)
        {
            _job = _jobTextParentHired[2].transform.GetChild(i).gameObject;
            _employee3JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[3].transform.childCount; i++)
        {
            _job = _jobTextParentHired[3].transform.GetChild(i).gameObject;
            _employee4JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[4].transform.childCount; i++)
        {
            _job = _jobTextParentHired[4].transform.GetChild(i).gameObject;
            _employee5JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }
    }

    /// <summary>
    /// Set job for job profile UI for this employee and show them
    /// </summary>
    /// <param name="_employeeSelected"></param>
    public void ShowRecrutementProfile()
    {
        for (int i = 0; i < _directorRoom.RecrutementList.Count; i++)
        {
            ShowName();
            ShowJob();
            _jobProfileHired.SetActive(true);

            SetTextList();
        }
    }

    /// <summary>
    /// Listen jobProfileGenerator for number of job has this employee
    /// </summary>
    /// <param name="_jobNumber"></param>
    public void ShowJob()
    {
        for (int i = 0; i < _directorRoom.RecrutementList[0].GetComponent<Employee>().EmployeeJob.Count; i++)
        {
            _employee1JobTextList[i].text = _directorRoom.RecrutementList[0].GetComponent<Employee>().EmployeeJob[i].JobName;
            _employee1JobTextList[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _directorRoom.RecrutementList[1].GetComponent<Employee>().EmployeeJob.Count; i++)
        {
            _employee2JobTextList[i].text = _directorRoom.RecrutementList[1].GetComponent<Employee>().EmployeeJob[i].JobName;
            _employee2JobTextList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < _directorRoom.RecrutementList[2].GetComponent<Employee>().EmployeeJob.Count; i++)
        {
            _employee3JobTextList[i].text = _directorRoom.RecrutementList[2].GetComponent<Employee>().EmployeeJob[i].JobName;
            _employee3JobTextList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < _directorRoom.RecrutementList[3].GetComponent<Employee>().EmployeeJob.Count; i++)
        {
            _employee4JobTextList[i].text = _directorRoom.RecrutementList[3].GetComponent<Employee>().EmployeeJob[i].JobName;
            _employee4JobTextList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < _directorRoom.RecrutementList[4].GetComponent<Employee>().EmployeeJob.Count; i++)
        {
            _employee5JobTextList[i].text = _directorRoom.RecrutementList[4].GetComponent<Employee>().EmployeeJob[i].JobName;
            _employee5JobTextList[i].gameObject.SetActive(true);
        }
    }

    public void ShowName()
    {
        for (int i = 0; i < _directorRoom.RecrutementList.Count; i++)
        {
            _employeeNameHired[i].text = _directorRoom.RecrutementList[i].GetComponent<Employee>().EmployeeName;
        }
    }

    public void ShowProfile(Employee employee)
    {
        _employeeName.text = employee.EmployeeName;

        for (int i = 0; i < employee.EmployeeJob.Count; i++)
        {
            _jobTextParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = employee.EmployeeJob[i].JobName;
            _jobTextParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Give the job data to the button 
    /// </summary>
    public void SetButtonJobChoose(int i)
    {
        _jobChoose = i;
    }

    /// <summary>
    /// for employee are not hired
    /// </summary>
    /// <param name="i"></param>
    public void SetButtonFicheMetier(int i)
    {
        ShowFicheMetier(_directorRoom.RecrutementList[i].GetComponent<Employee>().EmployeeJob[_jobChoose]);
    }

    /// <summary>
    /// Get employee and job choose and call show fiche metier
    /// </summary>
    /// <param name="_employee"></param>
    public void ShowHiredEmployeeFicheMetier(Employee _employee)
    {
        ShowFicheMetier(_employee.EmployeeJob[_jobChoose]);
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

    /// <summary>
    /// Close all text
    /// </summary>
    public void CloseJobProfile()
    {
        for (int j = 0; j <_directorRoom.RecrutementList.Count; j++)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[j].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee1JobTextList[i].gameObject.SetActive(false);
                _employee2JobTextList[i].gameObject.SetActive(false);
                _employee3JobTextList[i].gameObject.SetActive(false);
                _employee4JobTextList[i].gameObject.SetActive(false);
                _employee5JobTextList[i].gameObject.SetActive(false);

                _jobTextParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _jobTextParent.transform.childCount; i++)
        {
            _jobTextParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
