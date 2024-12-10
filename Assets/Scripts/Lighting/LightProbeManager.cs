using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct LightProbe
{
    public SphericalHarmonicsL2 Probe;

    public Vector3 ProbePosition;

    public int GlobalIndex;

    // Explicit constructor
    public LightProbe(SphericalHarmonicsL2 probe, Vector3 probePosition, int globalIndex)
    {
        Probe = probe;
        ProbePosition = probePosition;
        GlobalIndex = globalIndex;
    }
}

public class LightProbeManager : MonoBehaviour
{
    // Singleton
    private static LightProbeManager _instance = null;
    public static LightProbeManager Instance => _instance;

    [field: SerializeField] public float Attenuation { get; private set; }


    Vector3[] _probePositions;

    Dictionary<string, Dictionary<string, List<LightProbe>>> _lightProbeGrid = new();

    /// <summary>
    /// Row name format.
    /// </summary>
    const string _rowFormat = "row{0}";

    /// <summary>
    /// Column name format.
    /// </summary>
    const string _columnFormat = "column{0}";

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

    void Start()
    {
        InitGrid();

        // for (int i = 0; i < _lightProbeGrid[string.Format(_rowFormat, 0)][string.Format(_columnFormat, 0)].Count; i++)
        // {
        //     Debug.DrawRay(_lightProbeGrid[string.Format(_rowFormat, 0)][string.Format(_columnFormat, 0)][i].ProbePosition, Vector3.back * 100, Color.red, 1000);
        // }
    }

    public List<LightProbe> GetProbesForThisRoom(Room room)
    {
        Vector2 position = room.RoomPosition;
        int size = room.RoomData.Size;
        List<LightProbe> probes = new();

        for (int i = 0; i < size; i++)
        {
            probes.AddRange(_lightProbeGrid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x + i)]);
        }

        return probes;
    }

    void InitGrid()
    {
        // Create all slots in the dictionnary
        for (int i = 0; i < GridManager.Instance.GridSize.y; i++)
        {
            _lightProbeGrid.Add(string.Format(_rowFormat, i), new Dictionary<string, List<LightProbe>>());

            for (int j = 0; j < GridManager.Instance.GridSize.x; j++)
            {
                _lightProbeGrid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), new());
            }
        }

        SphericalHarmonicsL2[] probes = LightmapSettings.lightProbes.bakedProbes;
        _probePositions = LightmapSettings.lightProbes.positions;

        for (int i = 0; i < probes.Length; i++)
        {
            probes[i].Clear();
        }

        for (int i = 0; i < probes.Length; i++)
        {
            AssociateProbeToCell(probes[i], _probePositions[i], i);
        }
    }

    void AssociateProbeToCell(SphericalHarmonicsL2 probe, Vector3 position, int index)
    {
        LightProbe lightProbeCell = new();
        lightProbeCell.Probe = probe;
        lightProbeCell.ProbePosition = position;
        lightProbeCell.GlobalIndex = index;

        _lightProbeGrid[string.Format(_rowFormat, (int)(position.y / 4))][string.Format(_columnFormat, ((int)(position.x / 3)))]
        .Add(lightProbeCell);
    }
}
