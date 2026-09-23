using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;
    [SerializeField] private bool autoExpand = true;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private Transform parent;
    private string _poolId;
    public string PoolName => prefab.name;
    public int PoolSize => pool.Count;
    public int ActiveCount => activeObjects.Count;
    public int TotalCount => PoolSize + ActiveCount;
    public IReadOnlyList<GameObject> ActiveObjects => activeObjects;

    public void Initialize(string poolId, Transform parent = null)
    {
        this._poolId = poolId;
        this.parent = parent;

        // Create initial pool objects
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);

        // Ensure the object implements IPoolable
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable == null)
        {
            // Phải gán lại: trước đây AddComponent xong vẫn để poolable = null nên dòng
            // gán PoolId ngay dưới ném NullReferenceException.
            poolable = obj.AddComponent<PoolableObject>();
        }
        poolable.PoolId = _poolId;

        pool.Enqueue(obj);
        return obj;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;

        // Lấy object hợp lệ trong pool
        while (pool.Count > 0)
        {
            obj = pool.Dequeue();

            if (obj != null && !obj.Equals(null))
                break;

            // object trong pool đã bị Destroy => bỏ qua
            obj = null;
        }

        // Nếu không có object hợp lệ -> tạo mới
        if (obj == null)
        {
            if (autoExpand)
            {
                obj = CreateNewObject();

                // dequeue lại object mới tạo
                obj = pool.Dequeue();
            }
            else
            {
                Debug.LogWarning($"Pool {PoolName} is at maximum capacity!");
                return null;
            }
        }

        // Setup transform
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        activeObjects.Add(obj);

        // Callback
        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.PoolId = _poolId;
            poolable.OnSpawnFromPool();
        }
        // obj.GetComponent<IPoolable>()?.OnSpawnFromPool();

        return obj;
    }


    public GameObject Spawn(Vector3 position)
    {
        return Spawn(position, Quaternion.identity);
    }

    public GameObject Spawn()
    {
        return Spawn(Vector3.zero, Quaternion.identity);
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null || obj.Equals(null))
        {
            Debug.LogWarning("Trying to return a destroyed object to pool!");
            activeObjects.Remove(obj);
            return;
        }

        if (!activeObjects.Contains(obj))
        {
            Debug.LogWarning($"Object {obj.name} is not from this pool!");
            return;
        }
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnReturnToPool();

        obj.SetActive(false);
        obj.transform.SetParent(null);
        obj.transform.SetParent(parent, false);
        activeObjects.Remove(obj);
        pool.Enqueue(obj);
    }


    public void ReturnAllToPool()
    {
        var objectsToReturn = new List<GameObject>(activeObjects);
        foreach (var obj in objectsToReturn)
        {
            ReturnToPool(obj);
        }
    }

    public void DestroyPool()
    {
        ReturnAllToPool();

        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (obj != null)
            {
                Object.Destroy(obj);
            }
        }

        pool.Clear();
        activeObjects.Clear();
    }
}
