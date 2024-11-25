using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [SerializeField] string _fileName;

    GameData _gameData;

    List<IDataPersistance> _dataPersistenceObjects;

    FileDataHandler _fileDataHandler;

    [SerializeField] bool _useEncrytion;

    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Called when the object is initialized.
    /// </summary>
    void Start()
    {
        // Set up the file data handler
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncrytion);

        // Find all components that implement IDataPersistance
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Load the game data
        LoadGame();
    }

    /// <summary>
    /// Finds all MonoBehaviours that implement IDataPersistance.
    /// </summary>
    /// <returns>A list of IDataPersistance objects.</returns>
    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        // Find all MonoBehaviours that implement IDataPersistance
        IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    /// <summary>
    /// Loads the game data from the file. If no data is found, initializes new game data.
    /// </summary>
    public void LoadGame()
    {
        // Attempt to load the game data from the file
        this._gameData = _fileDataHandler.Load();

        // Check if the loaded data is null
        if (this._gameData == null)
        {
            Debug.Log("No game data found. Creating new game data.");
            // Initialize new game data if none is found
            NewGame();
        }

        // Load data into all persistence objects
        foreach (IDataPersistance dataPersistanceObj in _dataPersistenceObjects)
        {
            dataPersistanceObj.LoadData(_gameData);
        }
    }

    /// <summary>
    /// Saves the game data to the file.
    /// </summary>
    public void SaveGame()
    {
        // Save data from all persistence objects
        foreach (IDataPersistance dataPersistanceObj in _dataPersistenceObjects)
        {
            // Save data from this object into the game data
            dataPersistanceObj.SaveData(ref _gameData);
        }

        // Save the game data to the file
        _fileDataHandler.Save(_gameData);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
