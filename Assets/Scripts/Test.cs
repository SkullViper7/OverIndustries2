using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Pool 1 : " + ObjectPoolManager.Instance.NewObjectPool());
        Debug.Log("Pool 2 : " + ObjectPoolManager.Instance.NewObjectPool());
    }

}
