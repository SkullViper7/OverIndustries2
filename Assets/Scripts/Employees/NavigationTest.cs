using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    [SerializeField] Transform _target;

    NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_agent.SetDestination(_target.position);
    }
}
