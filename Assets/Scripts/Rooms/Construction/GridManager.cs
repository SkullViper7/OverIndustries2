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

    GameObject _uiSpots;

    const string _rowFormat = "row{0}";
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

    private void Start()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        // Create all slots in the dictionnary
        for (int i = 1; i <= _gridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, Room>());

            for (int j = 1; j <= _gridSize.x; j++)
            {
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
            }
        }

        // Set the two default rooms in the grid
        for (int i = 0; i < _roomsByDefault.Count; i++)
        {
            if (_roomsByDefault[i].RoomData.RoomType == RoomType.Director)
            {
                AddARoomInTheGrid(_roomsByDefault[i], new Vector2(1, 1));
            }
            else if (_roomsByDefault[i].RoomData.RoomType == RoomType.Delivery)
            {
                AddARoomInTheGrid(_roomsByDefault[i], new Vector2(3, 1));
            }
        }
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
            _grid[string.Format(_rowFormat, position.x + i)][string.Format(_columnFormat, position.y)] = room;
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
            _grid[string.Format(_rowFormat, roomPosition.x + i)][string.Format(_columnFormat, roomPosition.y)] = null;
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
        return _grid[string.Format(_rowFormat, position.x)][string.Format(_columnFormat, position.y)] != null;
    }

    /// <summary>
    /// Called to get a list of available spots to construct a room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public List<Vector2> GetAvailableSpots(Room room)
    {
        List<Vector2> availableSpots = new();
        RoomType roomToConstructType = room.RoomData.RoomType;
        int roomToConstructSize = room.RoomData.Size;

        // Browse each room
        for (int i = 0; i < _instantiatedRooms.Count; i++)
        {
            Vector2 instantiatedRoomPosition = _instantiatedRooms[i].RoomPosition;
            bool isLeftAvailable = true;
            bool isRightAvailable = true;

            // Check to the left
            for (int j = 0; j < roomToConstructSize; j++)
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
            for (int j = 0; j < roomToConstructSize; j++)
            {
                // Check if position is not outside the limits.
                if (instantiatedRoomPosition.x + (_instantiatedRooms[i].RoomData.Size - 1) + j > _gridSize.x)
                {
                    isLeftAvailable = false;
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
                availableSpots.Add(new Vector2(instantiatedRoomPosition.x + (_instantiatedRooms[i].RoomData.Size - 1), instantiatedRoomPosition.y));
            }
        }

        return availableSpots;
    }

    // public void ShowSpotsUI(int roomSize)
    // {
    //     int k = 0;

    //     for (int i = 0; i < GridSize.x; i++)
    //     {
    //         for (int j = 0; j < GridSize.y; j++)
    //         {
    //             switch (roomSize) // instancie l'UI et transmet la taille et les donn�es de la salle adjacente
    //             {
    //                 case 0: //SmallSpots --> largeur = 6  --> size = 4
    //                     {
    //                         if (i != k && i < _instantiatedRooms.Count)
    //                         {
    //                             k = i;

    //                             if (!OccupiedSpots[i, j] && !OccupiedSpots[i - 1, j]) //check si il y a de la place apr�s une salle 
    //                             {
    //                                 if (_instantiatedRooms[i].Coordinates.y + 2 < GridSize.x) //v�rifie que la salle ne va pas d�passer de la grille
    //                                 {
    //                                     _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                     _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

    //                                     SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
    //                                     CheckOccupiedPosition(_uiSpots, i);

    //                                 }

    //                                 if (_instantiatedRooms[i].Coordinates.y - 2 >= 0) //v�rifie que la salle ne va pas d�passer de la grille
    //                                 {
    //                                     _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                     _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

    //                                     SetButtonParameter(i, 6);
    //                                     CheckOccupiedPosition(_uiSpots, i);
    //                                 }
    //                             }
    //                         }
    //                     }
    //                     break;

    //                 case 1: //LargeSpots --> largeur = 12  --> size = 6
    //                     {
    //                         if (i != k && i < _instantiatedRooms.Count) //check s'il y a de la place avant une salle 
    //                         {
    //                             k = i;

    //                             if (!OccupiedSpots[i, j] /*&& _occupiedSpots[i - 1, j]*/ && !OccupiedSpots[i + 1, j] && !OccupiedSpots[i + 2, j] && !OccupiedSpots[i + 3, j])
    //                             {
    //                                 if (_instantiatedRooms[i].Coordinates.y + 4 < GridSize.x)
    //                                 {
    //                                     Debug.Log(_instantiatedRooms[i].Coordinates.y + 4);
    //                                     _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                     _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

    //                                     SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
    //                                     CheckOccupiedPosition(_uiSpots, i);
    //                                 }

    //                                 if (_instantiatedRooms[i].Coordinates.y - 4 >= 0)
    //                                 {
    //                                     _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                     _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

    //                                     SetButtonParameter(i, 12);
    //                                     CheckOccupiedPosition(_uiSpots, i);
    //                                 }
    //                             }
    //                         }
    //                     }
    //                     break;

    //                 case 2: //ElevatorSpots --> largeur = 2  --> size = 1
    //                     {
    //                         if (i != k && !OccupiedSpots[i, j] /*&& _occupiedSpots[i - 1, j]*/ && i < _instantiatedRooms.Count) //check s'il y a de la place avant une salle 
    //                         {
    //                             k = i;
    //                             if (_instantiatedRooms[i].Coordinates.y + 1 < GridSize.x)
    //                             {
    //                                 _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                 _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

    //                                 SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
    //                                 CheckOccupiedPosition(_uiSpots, i);
    //                             }

    //                             if (_instantiatedRooms[i].Coordinates.y - 1 >= 0)
    //                             {
    //                                 _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
    //                                 _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

    //                                 SetButtonParameter(i, 3);
    //                                 CheckOccupiedPosition(_uiSpots, i);
    //                             }
    //                         }
    //                     }
    //                     break;
    //             }
    //         }
    //     }
    // }

    // public void SetButtonParameter(int i, int differenceXPos)
    // {
    //     if (_uiSpots.GetComponent<CreateNewRoom>().RightRoom)
    //     {
    //         _uiSpots.transform.position = new Vector3(_instantiatedRooms[i].transform.position.x + differenceXPos, _instantiatedRooms[i].transform.position.y, 0);
    //     }
    //     else
    //     {
    //         _uiSpots.transform.position = new Vector3(_instantiatedRooms[i].transform.position.x - differenceXPos, _instantiatedRooms[i].transform.position.y, 0);
    //     }

    //     _uiSpots.GetComponent<CreateNewRoom>().CoordinatesNewRoom = _instantiatedRooms[i].Coordinates;
    //     _uiSpots.GetComponent<CreateNewRoom>().SizeRoomPast = _instantiatedRooms[i].Size;
    // }

    // public void AddRoomToList(Room NewRoom)
    // {
    //     _instantiatedRooms.Add(NewRoom);
    // }

    // public void CheckOccupiedPosition(GameObject _uiSpots, int i)
    // {
    //     for (int l = 0; l < _instantiatedRooms.Count; l++) //recheck si la place est bien libre
    //     {
    //         if (_instantiatedRooms[l].Size > 2)
    //         {
    //             if (_uiSpots.transform.position == new Vector3(_instantiatedRooms[l].transform.position.x + _instantiatedRooms[l].DifferenceBetweenXPosition / 2, _instantiatedRooms[l].transform.position.y))
    //             {
    //                 _uiSpots.SetActive(false);
    //             }
    //         }
    //         if (_instantiatedRooms[l].Size < 2)
    //         {
    //             if (_uiSpots.transform.position == new Vector3(_instantiatedRooms[l].transform.position.x - _instantiatedRooms[l].DifferenceBetweenXPosition, _instantiatedRooms[l].transform.position.y))
    //             {
    //                 _uiSpots.SetActive(false);
    //             }
    //         }
    //         if (_uiSpots.transform.position == _instantiatedRooms[l].transform.position)
    //         {
    //             _uiSpots.SetActive(false);
    //         }
    //     }
    // }
}