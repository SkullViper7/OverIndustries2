using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobProfileUI : MonoBehaviour
{
    // Singleton
    private static JobProfileUI _instance = null;
    public static JobProfileUI Instance => _instance;

    [Header("UI Reference for hired job profile")]
    [SerializeField] private GameObject _jobProfileHired;
    [field: SerializeField] public List<GameObject> JobProfileList { get; private set; }
    [SerializeField] private List<TextMeshProUGUI> _employeeNameHired;
    [SerializeField] private List<GameObject> _jobTextParentHired;

    [Space, Header("UI Reference for job profile")]
    [SerializeField] private GameObject _jobProfile;
    [SerializeField] private TextMeshProUGUI _employeeName;
    [SerializeField] private GameObject _jobTextParent;

    [Space, Header("UI Reference for fiche métier")]
    [SerializeField] private TextMeshProUGUI _jobName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _pay;
    [SerializeField] private TextMeshProUGUI _studies;
    [SerializeField] private TextMeshProUGUI _careerDevelopment;
    [SerializeField] private TextMeshProUGUI _room;

    private GameObject _job;
    private int _jobChoose;

    private List<TextMeshProUGUI> _employee1JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee2JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee3JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee4JobTextList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _employee5JobTextList = new List<TextMeshProUGUI>();

    // reference
    private JobProfileGenerator _jobProfileGenerator;
    private DirectorRoom _directorRoom;

    public event System.Action UpdateProfilList;

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
            _job = _jobTextParentHired[0].transform.GetChild(i).GetChild(0).gameObject;
            _employee1JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[1].transform.childCount; i++)
        {
            _job = _jobTextParentHired[1].transform.GetChild(i).GetChild(0).gameObject;
            _employee2JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[2].transform.childCount; i++)
        {
            _job = _jobTextParentHired[2].transform.GetChild(i).GetChild(0).gameObject;
            _employee3JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[3].transform.childCount; i++)
        {
            _job = _jobTextParentHired[3].transform.GetChild(i).GetChild(0).gameObject;
            _employee4JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < _jobTextParentHired[4].transform.childCount; i++)
        {
            _job = _jobTextParentHired[4].transform.GetChild(i).GetChild(0).gameObject;
            _employee5JobTextList.Add(_job.GetComponent<TextMeshProUGUI>());
        }
    }

    /// <summary>
    /// Set job for job profile UI for this employee and show them
    /// </summary>
    public void ShowRecrutementProfile()
    {
        if (_directorRoom.RoomMain.EmployeeAssign.Count < _directorRoom.RoomMain.RoomData.Capacity)
        {
            ShowName();
            ShowJob();
            _jobProfileHired.SetActive(true);

            for (int i = 0; i < _directorRoom.RecrutementList.Count; i++)
            {
                _directorRoom.RecrutementList[i].SetActive(true);
                JobProfileList[i].SetActive(true);
            }
            SetTextList();
            UpdateProfilList.Invoke();
        }
    }

    /// <summary>
    /// Listen jobProfileGenerator for number of job has this employee
    /// </summary>
    public void ShowJob()
    {
        if (_directorRoom.RecrutementList.Count > 0)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[0].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee1JobTextList[i].text = _directorRoom.RecrutementList[0].GetComponent<Employee>().EmployeeJob[i].JobName;
                _employee1JobTextList[i].transform.parent.gameObject.SetActive(true);
            }
        }
        if (_directorRoom.RecrutementList.Count > 1)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[1].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee2JobTextList[i].text = _directorRoom.RecrutementList[1].GetComponent<Employee>().EmployeeJob[i].JobName;
                _employee2JobTextList[i].transform.parent.gameObject.SetActive(true);
            }
        }
        if (_directorRoom.RecrutementList.Count > 2)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[2].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee3JobTextList[i].text = _directorRoom.RecrutementList[2].GetComponent<Employee>().EmployeeJob[i].JobName;
                _employee3JobTextList[i].transform.parent.gameObject.SetActive(true);
            }
        }
        if (_directorRoom.RecrutementList.Count > 3)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[3].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee4JobTextList[i].text = _directorRoom.RecrutementList[3].GetComponent<Employee>().EmployeeJob[i].JobName;
                _employee4JobTextList[i].transform.parent.gameObject.SetActive(true);
            }
        }
        if (_directorRoom.RecrutementList.Count > 4)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[4].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                _employee5JobTextList[i].text = _directorRoom.RecrutementList[4].GetComponent<Employee>().EmployeeJob[i].JobName;
                _employee5JobTextList[i].transform.parent.gameObject.SetActive(true);
            }
        }
    }

    public void ShowName()
    {
        for (int i = 0; i < _directorRoom.RecrutementList.Count; i++)
        {
            _employeeNameHired[i].text = _directorRoom.RecrutementList[i].GetComponent<Employee>().EmployeeName;
        }
    }


    //show employee selected profil
    public void ShowProfile(Employee employee)
    {
        _employeeName.text = employee.EmployeeName;

        for (int i = 0; i < employee.EmployeeJob.Count; i++)
        {
            _jobTextParent.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = employee.EmployeeJob[i].JobName;
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

        if (_job.Studies.Count == 4)
        {
            _studies.text = $"{_job.Studies[0]} \n{_job.Studies[1]} \n{_job.Studies[2]} \n{_job.Studies[3]}";
        }
        else if (_job.Studies.Count == 5)
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
            _careerDevelopment.text = $"{_job.CareerDevelopment[0]} \n{_job.CareerDevelopment[1]} \n{_job.CareerDevelopment[2]} \n{_job.CareerDevelopment[3]}";
        }
        _room.text = _job.Room;
    }

    /// <summary>
    /// Close all text
    /// </summary>
    public void CloseJobProfile()
    {
        for (int j = 0; j < _directorRoom.RecrutementList.Count; j++)
        {
            for (int i = 0; i < _directorRoom.RecrutementList[j].GetComponent<Employee>().EmployeeJob.Count; i++)
            {
                JobProfileList[i].SetActive(false);
                _employee1JobTextList[i].transform.parent.gameObject.SetActive(false);
                _employee2JobTextList[i].transform.parent.gameObject.SetActive(false);
                _employee3JobTextList[i].transform.parent.gameObject.SetActive(false);
                _employee4JobTextList[i].transform.parent.gameObject.SetActive(false);
                _employee5JobTextList[i].transform.parent.gameObject.SetActive(false);

                _jobTextParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _jobTextParent.transform.childCount; i++)
        {
            _jobTextParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
