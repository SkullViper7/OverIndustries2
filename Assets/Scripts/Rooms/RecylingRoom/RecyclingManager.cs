using UnityEngine;

public class RecyclingManager : MonoBehaviour
{
    // Singleton
    private static RecyclingManager _instance = null;
    public static RecyclingManager Instance => _instance;

    public int TotalRecyclingComponent { get; private set; }



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


    public void StartRecycling(int speed)
    {

    }
}
