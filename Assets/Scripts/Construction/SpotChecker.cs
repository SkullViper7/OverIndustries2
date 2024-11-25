using System.Collections.Generic;
using UnityEngine;

public class SpotChecker : MonoBehaviour
{
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

    [field: SerializeField] public List<Room> _instantiatedRooms { get; private set; }

    public int GridSize = 16;

    public bool[,] _occupiedSpots;

    GameObject _uiSpots;

    void Awake()
    {
        _occupiedSpots = new bool[GridSize, GridSize];

        ResetSpots();
    }

    public void ResetSpots()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                _occupiedSpots[i, j] = false;
            }
        }
    }

    public void CheckOccupiedSpots()
    {
        for (int i = 0; i < _instantiatedRooms.Count; i++)
        {
            for (int j = 0; j < _instantiatedRooms[i].Size; j++)
            {
                _occupiedSpots[(int)_instantiatedRooms[i].Coordinates.x, (int)_instantiatedRooms[i].Coordinates.y] = true;
                _instantiatedRooms[i].Coordinates += new Vector2(0, 1);
            }
            _instantiatedRooms[i].Coordinates.y = _instantiatedRooms[i].Coordinates.y - _instantiatedRooms[i].Size;
        }

        //Debug
        string s = "";
        for (int k = 0; k < GridSize; k++)
        {
            for (int l = 0; l < GridSize; l++)
            {
                s += _occupiedSpots[k, l] ? "1 " : "0 ";
            }
            s += "\n";
        }
        Debug.Log($"Occupied spots: \n{s}");

    }

    public void ShowSpotsUI(int roomSize)
    {
        int k = 0;

        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                switch (roomSize) // instancie l'UI et transmet la taille et les données de la salle adjacente
                {
                    case 0: //SmallSpots --> largeur = 6  --> size = 4
                        {
                            if (i != k && i < _instantiatedRooms.Count)
                            {
                                k = i;

                                if (!_occupiedSpots[i, j] && !_occupiedSpots[i - 1, j]) //check si il y a de la place après une salle 
                                {
                                    if (_instantiatedRooms[i].Coordinates.y + 2 < GridSize) //vérifie que la salle ne va pas dépasser de la grille
                                    {
                                        _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                        _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

                                        SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
                                        CheckOccupiedPosition(_uiSpots, i);

                                    }

                                    if (_instantiatedRooms[i].Coordinates.y - 2 >= 0) //vérifie que la salle ne va pas dépasser de la grille
                                    {
                                        _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                        _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

                                        SetButtonParameter(i, 6);
                                        CheckOccupiedPosition(_uiSpots, i);
                                    }
                                }
                            }
                        }
                        break;

                    case 1: //LargeSpots --> largeur = 12  --> size = 6
                        {
                            if (i != k && i < _instantiatedRooms.Count) //check s'il y a de la place avant une salle 
                            {
                                k = i;

                                if (!_occupiedSpots[i, j] /*&& _occupiedSpots[i - 1, j]*/ && !_occupiedSpots[i + 1, j] && !_occupiedSpots[i + 2, j] && !_occupiedSpots[i + 3, j])
                                {
                                    if (_instantiatedRooms[i].Coordinates.y + 4 < GridSize)
                                    {
                                        Debug.Log(_instantiatedRooms[i].Coordinates.y + 4);
                                        _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                        _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

                                        SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
                                        CheckOccupiedPosition(_uiSpots, i);
                                    }

                                    if (_instantiatedRooms[i].Coordinates.y - 4 >= 0)
                                    {
                                        _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                        _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

                                        SetButtonParameter(i, 12);
                                        CheckOccupiedPosition(_uiSpots, i);
                                    }
                                }
                            }
                        }
                        break;

                    case 2: //ElevatorSpots --> largeur = 2  --> size = 1
                        {
                            if (i != k && !_occupiedSpots[i, j] /*&& _occupiedSpots[i - 1, j]*/ && i < _instantiatedRooms.Count) //check s'il y a de la place avant une salle 
                            {
                                k = i;
                                if (_instantiatedRooms[i].Coordinates.y + 1 < GridSize)
                                {
                                    _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                    _uiSpots.GetComponent<CreateNewRoom>().RightRoom = true;

                                    SetButtonParameter(i, _instantiatedRooms[i].DifferenceBetweenXPosition);
                                    CheckOccupiedPosition(_uiSpots, i);
                                }

                                if (_instantiatedRooms[i].Coordinates.y - 1 >= 0)
                                {
                                    _uiSpots = ObjectPool.Instance.RequestObject(roomSize);
                                    _uiSpots.GetComponent<CreateNewRoom>().RightRoom = false;

                                    SetButtonParameter(i, 3);
                                    CheckOccupiedPosition(_uiSpots, i);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    public void SetButtonParameter(int i, int differenceXPos)
    {
        if (_uiSpots.GetComponent<CreateNewRoom>().RightRoom)
        {
            _uiSpots.transform.position = new Vector3(_instantiatedRooms[i].transform.position.x + differenceXPos, _instantiatedRooms[i].transform.position.y, 0);
        }
        else
        {
            _uiSpots.transform.position = new Vector3(_instantiatedRooms[i].transform.position.x - differenceXPos, _instantiatedRooms[i].transform.position.y, 0);
        }

        _uiSpots.GetComponent<CreateNewRoom>().CoordinatesNewRoom = _instantiatedRooms[i].Coordinates;
        _uiSpots.GetComponent<CreateNewRoom>().SizeRoomPast = _instantiatedRooms[i].Size;
    }

    public void AddRoomToList(Room NewRoom)
    {
        _instantiatedRooms.Add(NewRoom);
    }

    public void CheckOccupiedPosition(GameObject _uiSpots, int i)
    {
        for (int l = 0; l < _instantiatedRooms.Count; l++) //recheck si la place est bien libre
        {
            if (_instantiatedRooms[l].Size > 2)
            {
                if (_uiSpots.transform.position == new Vector3(_instantiatedRooms[l].transform.position.x + _instantiatedRooms[l].DifferenceBetweenXPosition / 2, _instantiatedRooms[l].transform.position.y))
                {
                    _uiSpots.SetActive(false);
                }
            }
            if (_instantiatedRooms[l].Size < 2)
            {
                if (_uiSpots.transform.position == new Vector3(_instantiatedRooms[l].transform.position.x - _instantiatedRooms[l].DifferenceBetweenXPosition, _instantiatedRooms[l].transform.position.y))
                {
                    _uiSpots.SetActive(false);
                }
            }
            if (_uiSpots.transform.position == _instantiatedRooms[l].transform.position)
            {
                _uiSpots.SetActive(false);
            }
        }
    }
}