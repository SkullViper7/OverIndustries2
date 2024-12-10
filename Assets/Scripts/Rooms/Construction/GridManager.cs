using System;
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
    public Vector2 GridSize;

    public event Action GridInitializedEvent;

    public delegate void GridModificationDelegate(Vector2 position);
    public event GridModificationDelegate RoomAddEvent, RoomRemoveEvent;

    /// <summary>
    /// List of the room already in the scene at the start of the game.
    /// </summary>
    [SerializeField]
    private List<GameObject> _roomsByDefault = new();

    /// <summary>
    /// Dictionnary which represents the grid.
    /// </summary>
    private Dictionary<string, Dictionary<string, Room>> _grid = new();

    /// <summary>
    /// List of rooms already instantiated.
    /// </summary>
    public List<Room> InstantiatedRooms { get; private set; } = new();

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
        for (int i = 0; i < GridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, Room>());

            for (int j = 0; j < GridSize.x; j++)
            {
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
            }
        }

        // Set the two default rooms in the grid
        for (int i = 0; i < _roomsByDefault.Count; i++)
        {
            // Get component of the room.
            Room _room = _roomsByDefault[i].GetComponent<Room>();
            RoomByDefault _roomsByDefaultComponent = _roomsByDefault[i].GetComponent<RoomByDefault>();

            AddARoomInTheGrid(_room, _roomsByDefaultComponent.RoomData, (IRoomBehaviourData)_roomsByDefaultComponent.RoomBehaviourData, _roomsByDefaultComponent.RoomPosition);
        }

        GridInitializedEvent?.Invoke();
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
    /// <param name="roomData"> Data of the room. </param> 
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="position"> Position of the room. </param>
    public void AddARoomInTheGrid(Room room, RoomData roomData, IRoomBehaviourData roomBehaviourData, Vector2 position)
    {
        room.InitRoom(roomData, roomBehaviourData, position);

        // Set all slots of the room as not available
        for (int i = 0; i < room.RoomData.Size; i++)
        {
            _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x + i)] = room;
            RoomRemoveEvent?.Invoke(position + new Vector2(i, 0));
        }

        // Add the room to the list of instantiated rooms
        InstantiatedRooms.Add(room);
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
            RoomAddEvent?.Invoke(roomPosition + new Vector2(i, 0));
        }

        // Add the room to the list of instantiated rooms
        InstantiatedRooms.Remove(room);
    }

    /// <summary>
    /// Return true if a spot at a position is occupied.
    /// </summary>
    /// <param name="position"> Position to check. </param>
    /// <returns></returns>
    public bool CheckOccupiedSpots(Vector2 position)
    {
        return _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)] != null;
    }

    /// <summary>
    /// Return a room at a position given if there is one.
    /// </summary>
    /// <param name="position"> Position to check. </param>
    /// <param name="result"> A room. </param>
    /// <returns></returns>
    public bool TryGetRoomAtPosition(Vector2 position, out Room room)
    {
        room = null;

        if (CheckOccupiedSpots(position))
        {
            room = _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)];
            return true;
        }

        return false;
    }

    /// <summary>
    /// Return a list of rooms with the given type.
    /// </summary>
    /// <param name="roomType"> Type of the room to search. </param>
    /// <param name="rooms"> List of rooms with the type given. </param>
    /// <returns></returns>
    public bool TryGetRoomsWithThisType(RoomType roomType, out List<Room> rooms)
    {
        rooms = new();

        for (int i = 0; i < InstantiatedRooms.Count; i++)
        {
            if (InstantiatedRooms[i].RoomData.RoomType == roomType)
            {
                rooms.Add(InstantiatedRooms[i]);
            }
        }

        if (rooms.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
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
        for (int i = 0; i < InstantiatedRooms.Count; i++)
        {
            Vector2 instantiatedRoomPosition = InstantiatedRooms[i].RoomPosition;
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
                if (instantiatedRoomPosition.x + (InstantiatedRooms[i].RoomData.Size - 1) + j > GridSize.x - 1)
                {
                    isRightAvailable = false;
                    break;
                }

                if (CheckOccupiedSpots(new Vector2(instantiatedRoomPosition.x + (InstantiatedRooms[i].RoomData.Size - 1) + j, instantiatedRoomPosition.y)))
                {
                    isRightAvailable = false;
                    break;
                }
            }

            if (isRightAvailable)
            {
                availableSpots.Add(new Vector2(instantiatedRoomPosition.x + (InstantiatedRooms[i].RoomData.Size), instantiatedRoomPosition.y));
            }

            // Special cases with elevators
            if (roomToConstructType == RoomType.Elevator && InstantiatedRooms[i].RoomData.RoomType == RoomType.Elevator)
            {
                // Check upward
                if (instantiatedRoomPosition.y + 1 <= GridSize.y - 1)
                {
                    if (!CheckOccupiedSpots(new Vector2(instantiatedRoomPosition.x, instantiatedRoomPosition.y + 1)))
                    {
                        availableSpots.Add(new Vector2(instantiatedRoomPosition.x, instantiatedRoomPosition.y + 1));
                    }
                }

                // Check downward
                if (instantiatedRoomPosition.y - 1 >= 0)
                {
                    if (!CheckOccupiedSpots(new Vector2(instantiatedRoomPosition.x, instantiatedRoomPosition.y - 1)))
                    {
                        availableSpots.Add(new Vector2(instantiatedRoomPosition.x, instantiatedRoomPosition.y - 1));
                    }
                }
            }
        }

        return availableSpots;
    }

    /// <summary>
    /// Called to convert a grid position into a world position.
    /// </summary>
    /// <param name="gridPos"> Position in the grid. </param>
    /// <returns></returns>
    public Vector2 ConvertGridPosIntoWorldPos(Vector2 gridPos)
    {
        return new Vector2(gridPos.x * 3, gridPos.y * 4);
    }

    /// <summary>
    /// Converts a world position into a grid position.
    /// </summary>
    /// <param name="worldPos">The position in the world coordinates.</param>
    /// <returns>The equivalent position in grid coordinates.</returns>
    public Vector2 ConvertWorldPosIntoGridPos(Vector2 worldPos)
    {
        // Divide the world position by the scale factors to get grid position
        return new Vector2(worldPos.x / 3, worldPos.y / 4);
    }
}