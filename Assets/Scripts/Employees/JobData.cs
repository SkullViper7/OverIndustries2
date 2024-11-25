using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Job", menuName = "Job/Create new job")]
public class JobData : ScriptableObject
{
    /// <summary>
    /// Type of the job.
    /// </summary>
    [SerializeField]
    private Job _jobType;

    /// <summary>
    /// Name of the job.
    /// </summary>
    [SerializeField]
    private string _jobName;
    
    /// <summary>
    /// Description of the job.
    /// </summary>
    [SerializeField]
    private string _description;

    /// <summary>
    /// Pay of the job.
    /// </summary>
    [SerializeField]
    private List<string> _pay;

    /// <summary>
    /// Possible studies for the job
    /// </summary>
    [SerializeField]
    private List<string> _studies;

    /// <summary>
    /// List of Career development of the job.
    /// </summary>
    [SerializeField]
    private List<string> _careerDevelopment;

    /// <summary>
    /// Icone of the job.
    /// </summary>
    [SerializeField]
    private Texture2D _icone;

    /// <summary>
    /// Gets the type of the job.
    /// </summary>
    public Job JobType { get { return _jobType; } private set { } }

    /// <summary>
    /// Gets the name of the job.
    /// </summary>
    public string JobName { get { return _jobName; } private set { } }
    
    /// <summary>
    /// Gets the description of the job.
    /// </summary>
    public string Description { get { return _description; } private set { } }

    /// <summary>
    /// Gets the pay of the job.
    /// </summary>
    public List<string> Pay { get { return _pay; } private set { } }

    /// <summary>
    /// Gets the List of possible studies for the job.
    /// </summary>
    public List<string> Studies { get { return _studies; } private set { } }

    /// <summary>
    /// Gets the List of Career development of the job.
    /// </summary>
    public List<string> CareerDevelopment { get { return _careerDevelopment; } private set { } }

    /// <summary>
    /// Gets the icone of the room.
    /// </summary>
    public Texture2D Icone { get { return _icone; } private set { } }
}
