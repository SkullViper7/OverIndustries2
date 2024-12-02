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

    [field: SerializeField] public float PourcentageHasTwoJob { get; private set; }
    [field: SerializeField] public float PourcentageHasThreeJob { get; private set; }

    public event System.Action<string> NewName;
    public event System.Action<JobData> NewJob;
    public event System.Action<int> NumberOfJobs;

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
    /// Generate random name + random number of jobs
    /// </summary>
    public void GenerateProfile()
    {
        ResetForNewProfile();

        //Random Name
        int i = Random.Range(0, RandomNameList.Count);
        _name = RandomNameList[i];
        NewName.Invoke(_name);

        //Calcule pourcentage d'avoir tant de jobs
        int k = Random.Range(0, 100);
        if (k < PourcentageHasThreeJob)
        {
            _numberOfJob = 3;
            RandomJob(_numberOfJob);
            Debug.Log("3 Jobs");
        }
        else if (k < PourcentageHasTwoJob)
        {
            _numberOfJob = 2;
            RandomJob(_numberOfJob);
            Debug.Log("2 Jobs");
        }
        else
        {
            _numberOfJob = 1;
            RandomJob(_numberOfJob);
            Debug.Log("1 Jobs");
        }

        NumberOfJobs.Invoke(_numberOfJob);
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
            NewJob.Invoke(_jobsList[_job]);
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
