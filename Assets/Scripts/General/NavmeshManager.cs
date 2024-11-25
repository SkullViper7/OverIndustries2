using Unity.AI.Navigation;
using UnityEngine;

public class NavmeshManager : MonoBehaviour
{
    NavMeshSurface _surface;

    [SerializeField] GameObject _room;
    [SerializeField] Transform _roomTarget;
    [SerializeField] GameObject _employee;

    void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        //Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        Instantiate(_room, _roomTarget.position, _roomTarget.rotation);
        _surface.BuildNavMesh();
        _employee.SetActive(true);
    }
}
