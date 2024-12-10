using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public Dictionary<string, Dictionary<string, Room>> Grid;
    public List<Room> InstantiatedRooms;

    public GameData()
    {
        Grid = new Dictionary<string, Dictionary<string, Room>>();
        InstantiatedRooms = new List<Room>();
    }
}
