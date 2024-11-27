using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static int _idCounter = 0;

    /// <summary>
    /// Instance ID of the pool.
    /// </summary>
    public int PoolId { get; private set; }
    
    /// <summary>
    /// Concurrent bag which stocks objects.
    /// </summary>
    private ConcurrentBag<GameObject> _pool;

    /// <summary>
    /// A pool of object.
    /// </summary>
    public ObjectPool()
    {
        this.PoolId = _idCounter++;
        this._pool = new();
    }
}

/// <summary>
/// The pool of all IPoolable objects
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    // Singleton
    private static ObjectPoolManager _instance = null;
    public static ObjectPoolManager Instance => _instance;

    /// <summary>
    /// GameObject where all objects in pools go.
    /// </summary>
    [SerializeField]
    private GameObject _poolParent;

    /// <summary>
    /// List which stocks all object pools.
    /// </summary>
    private List<ObjectPool> _objectPools = new();

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
    /// <returns></returns>
    public int NewObjectPool()
    {
        ObjectPool newObjectPool = new();
        _objectPools.Add(newObjectPool);
        return newObjectPool.PoolId;
    }

    //private void GenerateObjects(ObjectPool pooledObject)
    //{
    //    for (int i = 0; i < pooledObject.InitialSize; i++)
    //    {
    //        GameObject obj = Instantiate(pooledObject._objectToStock);
    //        obj.transform.parent = _poolParent.transform;
    //        obj.SetActive(false);
    //        pooledObject.PooledObjects.Add(obj);
    //    }
    //}

    //public GameObject RequestObject(int prefab)
    //{
    //    foreach (var pooledObject in _pooledObjects)
    //    {
    //        if (pooledObject.PoolId == prefab) // Match pooled object by Id instead
    //        {
    //            foreach (var obj in pooledObject._pool)
    //            {
    //                if (!obj.activeInHierarchy)
    //                {
    //                    obj.SetActive(true);
    //                    return obj;
    //                }
    //            }

    //            If no object is available, instantiate a new one
    //            GameObject newObj = Instantiate(pooledObject._objectToStock);
    //            newObj.transform.parent = _poolParent.transform;
    //            pooledObject.PooledObjects.Add(newObj);
    //            return newObj;
    //        }
    //    }

    //    Debug.LogWarning("No pool available for this prefab: " + prefab);
    //    return null;
    //}

    //public void ReturnAllObjects()
    //{
    //    foreach (var pooledObject in _pooledObjects)
    //    {
    //        foreach (var obj in pooledObject._pool)
    //        {
    //            if (obj.activeInHierarchy)
    //            {
    //                obj.SetActive(false);
    //            }
    //        }
    //    }
    //}

    //public void ReturnThisObject(GameObject _object)
    //{
    //    if (_object.activeInHierarchy)
    //    {
    //        _object.SetActive(false);
    //    }
    //}
}
