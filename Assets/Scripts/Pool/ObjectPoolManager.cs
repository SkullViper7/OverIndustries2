using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // Singleton
    private static ObjectPoolManager _instance = null;
    public static ObjectPoolManager Instance => _instance;

    private static int _idCounter = 0;

    /// <summary>
    /// Dictionnary which stocks object pools with an ID.
    /// </summary>
    private Dictionary<int, ObjectPool> _objectPools = new();

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

    /// <summary>
    /// Called to create a new object pool and get its ID.
    /// </summary>
    /// <param name="objectPrefab"> Prefab of the object stocked in the pool. </param>
    /// <returns></returns>
    public int NewObjectPool(GameObject objectPrefab)
    {
        _idCounter++;
        _objectPools.Add(_idCounter, new(new(), objectPrefab));
        return _idCounter;
    }

    /// <summary>
    /// Called to get an object in a pool.
    /// </summary>
    /// <param name="poolId"> ID of the pool where get the object. </param>
    /// <returns></returns>
    public GameObject GetObjectInPool(int poolId)
    {
        if (_objectPools.ContainsKey(poolId))
        {
            if (_objectPools[poolId].Pool.TryTake(out GameObject objectPicked))
            {
                return objectPicked;
            }
            else
            {
                GameObject newObject = Instantiate(_objectPools[poolId].ObjectPrefab);
                return newObject;
            }
        }
        else
        {
            Debug.LogError(poolId + " is not an existing ID !");
            return null;
        }
    }

    /// <summary>
    /// Called to return an object to its pool.
    /// </summary>
    /// <param name="poolId"> ID of the pool where return the object. </param>
    /// <param name="objectToReturn"> The object to return. </param>
    public void ReturnObjectToThePool(int poolId, GameObject objectToReturn)
    {
        if (_objectPools.ContainsKey(poolId))
        {
            _objectPools[poolId].Pool.Add(objectToReturn);
        }
        else
        {
            Debug.LogError(poolId + " is not an existing ID !");
        }
    }
}
