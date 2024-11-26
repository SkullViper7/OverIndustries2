using UnityEngine;

public class SpotUIManager : MonoBehaviour
{
    GridManager _spotChecker;

    void Start()
    {
        _spotChecker = GetComponent<GridManager>();
    }

    public void ShowAvailableSmallSpots()
    {
        CheckSpots();
        // _spotChecker.ShowSpotsUI(0);
    }

    public void ShowAvailableLargeSpots()
    {
        CheckSpots();
        // _spotChecker.ShowSpotsUI(1);
    }

    public void ShowAvailableElevatorSpots()
    {
        CheckSpots();
        // _spotChecker.ShowSpotsUI(2);
    }

    public void CheckSpots()
    {
        // _spotChecker.ResetSpots();
        // _spotChecker.CheckOccupiedSpots();
    }
}
