using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[,] _occupiedSpots;

    public GameData()
    {
        this._occupiedSpots = new bool[16, 16];
    }
}
