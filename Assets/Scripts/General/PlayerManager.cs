using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int PlayerXP;
    public Vector3 PlayerPosition;

    public void AddXP()
    {
        this.PlayerXP++;
    }

    public void SetPlayerPosition()
    {
        this.PlayerPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Debug.Log(this.PlayerPosition);
    }
}
