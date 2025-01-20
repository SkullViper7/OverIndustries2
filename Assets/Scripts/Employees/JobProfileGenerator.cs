using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class JobProfileGenerator : MonoBehaviour
{
    // Singleton
    private static JobProfileGenerator _instance = null;
    public static JobProfileGenerator Instance => _instance;

    [field: SerializeField, Tooltip("Tout les métiers de production !")] public List<JobData> ProductionJobs;
    [field: SerializeField, Tooltip("Tout les autres métiers")] public List<JobData> OtherJobs;
    private List<JobData> _jobsProductionList = new List<JobData>();
    private List<JobData> _jobsOtherList = new List<JobData>();

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
    public void GenerateProductProfile(Employee employee)
    {
        _employeeGenerate = employee;

        //Random Name
        int n = Random.Range(0, RandomNameList.Count);
        _name = RandomNameList[n];
        _employeeGenerate.EmployeeName = _name;

        //Calcule pourcentage d'avoir tant de jobs
        //int k = Random.Range(0, 100);

        //if (k < _directorRoom.PourcentageHasThreeJob)
        //{
        //    _numberOfJob = 3;
        //    RandomJob(_numberOfJob);
        //}
        //else if (k < _directorRoom.PourcentageHasTwoJob)
        //{
        //    _numberOfJob = 2;
        //    RandomJob(_numberOfJob);
        //}
        //else
        //{
        _numberOfJob = 1;
        RandomProductJob(_numberOfJob);
        //}
    }

    /// <summary>
    /// Generate random name + random number of jobs
    /// </summary>
    public void GenerateOtherProfile(Employee employee)
    {
        _employeeGenerate = employee;

        //Random Name
        int n = Random.Range(0, RandomNameList.Count);
        _name = RandomNameList[n];
        _employeeGenerate.EmployeeName = _name;

        _numberOfJob = 1;
        RandomOtherJob(_numberOfJob);

    }
    
    /// <summary>
    /// Generate random name
    /// </summary>
    public void GenerateMaintenanceProfile(Employee employee)
    {
        _employeeGenerate = employee;

        //Random Name
        int n = Random.Range(0, RandomNameList.Count);
        _name = RandomNameList[n];
        _employeeGenerate.EmployeeName = _name;
    }

    /// <summary>
    /// Add random job to employee, with number of jobs
    /// </summary>
    /// <param name="_numberOfJobs"></param>
    public void RandomProductJob(int _numberOfJobs)
    {
        for (int i = 0; i < _numberOfJobs; i++)
        {
            int _job = Random.Range(0, _jobsProductionList.Count);
            _employeeGenerate.EmployeeJob.Add(_jobsProductionList[_job]);

            _jobsProductionList.Remove(_jobsProductionList[_job]);
        }
    }

    /// <summary>
    /// Add random job to employee, with number of jobs
    /// </summary>
    /// <param name="_numberOfJobs"></param>
    public void RandomOtherJob(int _numberOfJobs)
    {
        for (int i = 0; i < _numberOfJobs; i++)
        {
            int _job = Random.Range(0, _jobsOtherList.Count);
            _employeeGenerate.EmployeeJob.Add(_jobsOtherList[_job]);

            _jobsOtherList.Remove(_jobsOtherList[_job]);
        }
    }

    /// <summary>
    /// Reset parameter for create a new profile
    /// </summary>
    public void ResetForNewProductProfile()
    {
        _numberOfJob = 0;
        _jobsProductionList.Clear();

        for (int i = 0; i < ProductionJobs.Count; i++)
        {
            _jobsProductionList.Add(ProductionJobs[i]);
        }
    }

    /// <summary>
    /// Reset parameter for create a new profile
    /// </summary>
    public void ResetForNewOtherProfile()
    {
        _numberOfJob = 0;
        _jobsOtherList.Clear();

        for (int i = 0; i < OtherJobs.Count; i++)
        {
            _jobsOtherList.Add(OtherJobs[i]);
        }
    }
}
