using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Singleton
    private static GridManager _instance = null;
    public static GridManager Instance => _instance;

    /// <summary>
    /// Size of the grid.
    /// </summary>
    [SerializeField]
    private Vector2 _gridSize;

    /// <summary>
    /// List of the room already in the scene at the start of the game.
    /// </summary>
    [SerializeField]
    private List<Room> _roomsByDefault = new();

    /// <summary>
    /// Dictionnary which represents the grid.
    /// </summary>
    private Dictionary<string, Dictionary<string, Room>> _grid = new();

    /// <summary>
    /// List of rooms already instantiated.
    /// </summary>
    private List<Room> _instantiatedRooms = new();

    /// <summary>
    /// Row name format.
    /// </summary>
    const string _rowFormat = "row{0}";

    /// <summary>
    /// Column name format.
    /// </summary>
    const string _columnFormat = "column{0}";

    /// <summary>
    /// Events to get the list of available spots.
    /// </summary>
    /// <param name="roomData"> Data of the room to construct. </param>
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="availableSpots"> List of available spot positions. </param>
    public delegate void AvailableSpotsDelegate(RoomData roomData, IRoomBehaviourData roomBehaviourData, List<Vector2> availableSpots);
    public event AvailableSpotsDelegate AvailableSpotsResult;

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
        InitGrid();
    }

    private void InitGrid()
    {
        // Create all slots in the dictionnary
        for (int i = 0; i < _gridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, Room>());

            for (int j = 0; j < _gridSize.x; j++)
            {
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
            }
        }

        // Set the two default rooms in the grid
        for (int i = 0; i < _roomsByDefault.Count; i++)
        {
            if (_roomsByDefault[i].RoomData.RoomType == RoomType.Director)
            {
                AddARoomInTheGrid(_roomsByDefault[i], new Vector2(0, 0));
            }
            else if (_roomsByDefault[i].RoomData.RoomType == RoomType.Delivery)
            {
                AddARoomInTheGrid(_roomsByDefault[i], new Vector2(4, 0));
            }
        }
    }

    /// <summary>
    /// Called to Launch a research of available spots.
    /// </summary>
    /// <param name="roomData"> Data of the room to construct. </param>
    ///     /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    public void LaunchAResearch(RoomData roomData, IRoomBehaviourData roomBehaviourData)
    {
        AvailableSpotsResult?.Invoke(roomData, roomBehaviourData, GetAvailableSpots(roomData));
    }

    /// <summary>
    /// Called to add a new room in the grid.
    /// </summary>
    /// <param name="room"> Main component of the room. </param>
    /// <param name="position"> Position of the room. </param>
    public void AddARoomInTheGrid(Room room, Vector2 position)
    {
        room.SetRoomPosition(position);

        // Set all slots of the room as not available
        for (int i = 0; i < room.RoomData.Size; i++)
        {
            _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x + i)] = room;
        }

        // Add the room to the list of instantiated rooms
        _instantiatedRooms.Add(room);
    }

    /// <summary>
    /// Called to remove a room of the grid.
    /// </summary>
    /// <param name="room"> Main component of the room. </param>
    public void RemoveARoomofTheGrid(Room room)
    {
        Vector2 roomPosition = room.RoomPosition;

        // Set all slots of the room as available
        for (int i = 0; i < room.RoomData.Size; i++)
        {
            _grid[string.Format(_rowFormat, roomPosition.y)][string.Format(_columnFormat, roomPosition.x + i)] = null;
        }

        // Add the room to the list of instantiated rooms
        _instantiatedRooms.Remove(room);
    }

    /// <summary>
    /// Return if a spot at a position is available.
    /// </summary>
    /// <param name="position"> Position to check. </param>
    /// <returns></returns>
    public bool CheckOccupiedSpots(Vector2 position)
    {
        return _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)] != null;
    }

    /// <summary>
    /// Called to get a list of available spots to construct a room.
    /// </summary>
    /// <param name="roomData"> Data of the room to construct. </param>
    /// <returns></returns>
    public List<Vector2> GetAvailableSpots(RoomData roomData)
    {
        List<Vector2> availableSpots = new();
        RoomType roomToConstructType = roomData.RoomType;
        int roomToConstructSize = roomData.Size;

        // Browse each room
        for (int i = 0; i < _instantiatedRooms.Count; i++)
        {
            Vector2 instantiatedRoomPosition = _instantiatedRooms[i].RoomPosition;
            bool isLeftAvailable = true;
            bool isRightAvailable = true;

            // Check to the left
            for (int j = 1; j <= roomToConstructSize; j++)
            {
                // Check if position is not outside the limits.
                if (instantiatedRoomPosition.x - j < 0)
                {
                    isLeftAvailable = false;
                    break;
                }

                // Check if their is enough space
                if (CheckOccupiedSpots(new Vector2(instantiatedRoomPosition.x - j, instantiatedRoomPosition.y)))
                {
                    isLeftAvailable = false;
                    break;
                }
            }

            if (isLeftAvailable)
            {
                availableSpots.Add(new Vector2(instantiatedRoomPosition.x - roomToConstructSize, instantiatedRoomPosition.y));
            }

            // Check to the right
            for (int j = 1; j <= roomToConstructSize; j++)
            {
                // Check if position is not outside the limits.
                if (instantiatedRoomPosition.x + (_instantiatedRooms[i].RoomData.Size - 1) + j > _gridSize.x)
                {
                    isRightAvailable = false;
                    break;
                }

                if (CheckOccupiedSpots(new Vector2(instantiatedRoomPosition.x + (_instantiatedRooms[i].RoomData.Size - 1) + j, instantiatedRoomPosition.y)))
                {
                    isRightAvailable = false;
                    break;
                }
            }

            if (isRightAvailable)
            {
                availableSpots.Add(new Vector2(instantiatedRoomPosition.x + (_instantiatedRooms[i].RoomData.Size), instantiatedRoomPosition.y));
            }
        }

        return availableSpots;
    }

    public Vector2 ConvertGridPosIntoWorldPos(Vector2 gridPos)
    {
        return new Vector2(gridPos.x * 3, gridPos.y * 4);
    }
}