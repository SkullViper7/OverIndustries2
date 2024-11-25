using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public GameObject prefab;
    public int InitialSize;
    public int Id;
    [HideInInspector] public List<GameObject> PooledObjects;

    public PooledObject(GameObject prefab, int initialSize, int id)
    {
        this.prefab = prefab;
        this.InitialSize = initialSize;
        this.Id = id;
        PooledObjects = new List<GameObject>();
    }
}

/// <summary>
/// The pool of all IPoolable objects
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;

    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPool>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject _poolParent;
    [SerializeField] private List<PooledObject> _pooledObjects;

    private void Start()
    {
        foreach (var pooledObject in _pooledObjects)
        {
            GenerateObjects(pooledObject);
        }
    }

    private void GenerateObjects(PooledObject pooledObject)
    {
        for (int i = 0; i < pooledObject.InitialSize; i++)
        {
            GameObject obj = Instantiate(pooledObject.prefab);
            obj.transform.parent = _poolParent.transform;
            obj.SetActive(false);
            pooledObject.PooledObjects.Add(obj);
        }
    }

    public GameObject RequestObject(int prefab)
    {
        foreach (var pooledObject in _pooledObjects)
        {
            if (pooledObject.Id == prefab) // Match pooled object by Id instead
            {
                foreach (var obj in pooledObject.PooledObjects)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }

                // If no object is available, instantiate a new one
                GameObject newObj = Instantiate(pooledObject.prefab);
                newObj.transform.parent = _poolParent.transform;
                pooledObject.PooledObjects.Add(newObj);
                return newObj;
            }
        }

        Debug.LogWarning("No pool available for this prefab: " + prefab);
        return null;
    }

    public void ReturnAllObjects()
    {
        foreach (var pooledObject in _pooledObjects)
        {
            foreach (var obj in pooledObject.PooledObjects)
            {
                if (obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    public void ReturnThisObject(GameObject _object)
    {
        if (_object.activeInHierarchy)
        {
            _object.SetActive(false);
        }
    }
}
