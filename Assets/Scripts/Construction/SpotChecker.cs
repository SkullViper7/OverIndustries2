using System.Collections.Generic;
using UnityEngine;

public struct AvailableSpot
{
    public Vector2 Position;
    public bool IsToTheLeft;
}

public class SpotChecker : MonoBehaviour
{
    public Vector2 GridSize = new Vector2(16, 6);

    public Dictionary<string, Dictionary<string, Room>> OccupiedSpots = new();

    private static SpotChecker _instance;

    public static SpotChecker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpotChecker>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// List of rooms already instantiated.
    /// </summary>
    [SerializeField] 
    private List<Room> _instantiatedRooms;

    //public bool[,] OccupiedSpots;

    GameObject _uiSpots;

    const string _rowFormat = "row{0}";
    const string _columnFormat = "column{0}";

    private void Start()
    {
        InitGrid();
    }

    void InitGrid()
    {
        for (int i = 0; i < GridSize.y; i++)
        {
            OccupiedSpots.Add(string.Format(_rowFormat, i + 1), new Dictionary<string, Room>());
            for (int j = 0; j < GridSize.x; j++)
            {
                OccupiedSpots[string.Format(_rowFormat, i + 1)].Add(string.Format(_columnFormat, j + 1), null);
            }
        }
    }

    public bool CheckOccupiedSpots(Vector2 position)
    {
        return OccupiedSpots[string.Format(_rowFormat, position.x)][string.Format(_columnFormat, position.y)] != null;
    }

    public List<AvailableSpot> GetAvailableSpots(Room room)
    {
        List<AvailableSpot> availableSpots = new();
        RoomType roomType = room.RoomData.RoomType;
        int size = room.RoomData.Size;

        // Browse each row
        for (int i = 0; i < OccupiedSpots.Count; i++)
        {
            // Browse each column
            for (int j = 0; j < OccupiedSpots[string.Format(_rowFormat, i + 1)].Count; j++)
            {

                // If there is an occupied spot, check if left and right are available
                if (CheckOccupiedSpots(new Vector2(j, i)))
                {
                    bool isLeftAvailable = true;
                    bool isRightAvailable = true;

                    // Check to the left
                    for (int k = 1; k <= size; k++)
                    {
                        if (CheckOccupiedSpots(new Vector2(j - k, i)))
                        {
                            isLeftAvailable = false;
                            break;
                        }
                    }

                    if (isLeftAvailable)
                    {
                        availableSpots.Add(new AvailableSpot{Position = new Vector2(j - 1, i), IsToTheLeft = true});
                    }

                    // Check to the right
                    for (int k = 1; k <= size; k++)
                    {
                        if (CheckOccupiedSpots(new Vector2(j + (size-1) + k, i)))
                        {
                            isRightAvailable = false;
                            break;
                        }
                    }

                    if (isRightAvailable)
                    {
                        availableSpots.Add(new AvailableSpot{Position = new Vector2(j + (size-1) + 1, i), IsToTheLeft = false});
                    }

                    // Skip room's size checked
                    j += (size-1);
                }
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