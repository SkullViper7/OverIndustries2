using System.Collections.Generic;
using UnityEngine;

public class JobProfileGenerator : MonoBehaviour
{
    // Singleton
    private static JobProfileGenerator _instance = null;
    public static JobProfileGenerator Instance => _instance;

    [field: SerializeField, Tooltip("Tout les métier sauf technicien de maintenace")] public List<JobData> JobsListSave;
    private List<JobData> _jobsList = new List<JobData>();

    [field: SerializeField] public List<string> RandomNameList { get; private set; }

    private string _name;
    private int _numberOfJob;
    private Employee _employeeGenerate;
    private DirectorRoom _directorRoom;

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

    private void Start()
    {
        GridManager.Instance.GridInitializedEvent += GetDirectorRoom;
    }
    private void GetDirectorRoom()
    {
        _directorRoom = DirectorRoom.Instance;
    }

    /// <summary>
    /// Generate random name + random number of jobs
    /// </summary>
    public void GenerateProfile(Employee employee)
    {
        Debug.Log("two "+_directorRoom.PourcentageHasTwoJob);
        Debug.Log("three "+_directorRoom.PourcentageHasThreeJob);

        _employeeGenerate = employee;
        ResetForNewProfile();

        //Random Name
        int i = Random.Range(0, RandomNameList.Count);
        _name = RandomNameList[i];
        _employeeGenerate.EmployeeName = _name;

        //Calcule pourcentage d'avoir tant de jobs
        int k = Random.Range(0, 100);
        if (k < _directorRoom.PourcentageHasThreeJob)
        {
            _numberOfJob = 3;
            RandomJob(_numberOfJob);
        }
        else if (k < _directorRoom.PourcentageHasTwoJob)
        {
            _numberOfJob = 2;
            RandomJob(_numberOfJob);
        }
        else
        {
            _numberOfJob = 1;
            RandomJob(_numberOfJob);
        }
    }

    /// <summary>
    /// Add random job to employee, with number of jobs
    /// </summary>
    /// <param name="_numberOfJobs"></param>
    public void RandomJob(int _numberOfJobs)
    {
        for (int i = 0; i < _numberOfJobs; i++)
        {
            int _job = Random.Range(0, _jobsList.Count);
            _employeeGenerate.EmployeeJob.Add(_jobsList[_job]);

            _jobsList.Remove(_jobsList[_job]);
        }
    }

    /// <summary>
    /// Reset parameter for create a new profile
    /// </summary>
    public void ResetForNewProfile()
    {
        _numberOfJob = 0;
        _jobsList.Clear();

        for (int i = 0; i < JobsListSave.Count; i++)
        {
            _jobsList.Add(JobsListSave[i]);
        }
    }
}
