using System;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool Settings")]
    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();
    [SerializeField] private bool createPoolParents = true;
    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
    private Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();

    [System.Serializable]
    public class PoolConfig
    {
        public string poolId;
        public GameObject prefab;
        public int initialSize = 10;
        public bool autoExpand = true;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
        DontDestroyOnLoad(this);
    }

    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {
            CreatePool(config);
        }
    }

    public void CreatePool(PoolConfig config)
    {
        if (pools.ContainsKey(config.poolId))
        {
            Debug.LogWarning($"Pool with ID '{config.poolId}' already exists!");
            return;
        }

        Transform parent = null;
        if (createPoolParents)
        {
            GameObject parentObj = new GameObject($"Pool_{config.poolId}");
            parentObj.transform.SetParent(transform);
            parent = parentObj.transform;
            poolParents[config.poolId] = parent;
        }

        ObjectPool pool = new ObjectPool();
        // Set pool properties using reflection or make ObjectPool fields public
        var poolType = typeof(ObjectPool);
        poolType.GetField("prefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pool, config.prefab);
        poolType.GetField("initialSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pool, config.initialSize);
        poolType.GetField("autoExpand", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pool, config.autoExpand);

        pool.Initialize(config.poolId, parent);
        pools[config.poolId] = pool;

        Debug.LogWarning($"<color=yellow>[PoolManager]</color> Created pool: {config.poolId}");
    }

    public GameObject Spawn(string poolId, Vector3 position, Quaternion rotation)
    {
        if (pools.TryGetValue(poolId, out ObjectPool pool))
        {
            return pool.Spawn(position, rotation);
        }

        Debug.LogError($"Pool '{poolId}' not found!");
        return null;
    }

    public GameObject Spawn(string poolId, Vector3 position)
    {
        return Spawn(poolId, position, Quaternion.identity);
    }

    public GameObject Spawn(string poolId)
    {
        return Spawn(poolId, Vector3.zero, Quaternion.identity);
    }

    public GameObject Spawn(string poolId, GameObject prefab)
    {
        if (!pools.ContainsKey(poolId))
        {
            CreatePool(new PoolConfig
            {
                poolId = poolId,
                prefab = prefab,
                initialSize = 3,
                autoExpand = true
            });
        }
        return Spawn(poolId);
    }

    public void ReturnToPool(string poolId, GameObject obj)
    {
        if (pools.TryGetValue(poolId, out ObjectPool pool))
        {
            pool.ReturnToPool(obj);
        }
        else
        {
            Debug.LogError($"Pool '{poolId}' not found!");
        }
    }

    public void ReturnAllToPool(string poolId)
    {
        if (pools.TryGetValue(poolId, out ObjectPool pool))
        {
            pool.ReturnAllToPool();
        }
        else
        {
            Debug.LogError($"Pool '{poolId}' not found!");
        }
    }

    public void ReturnAllToAllPools()
    {
        foreach (var pool in pools.Values)
        {
            pool.ReturnAllToPool();
        }
    }

    public ObjectPool GetPool(string poolId)
    {
        pools.TryGetValue(poolId, out ObjectPool pool);
        return pool;
    }

    /// <summary>Danh sách object đang active của 1 pool. Null nếu pool không tồn tại.</summary>
    public IReadOnlyList<GameObject> GetActiveObjects(string poolId)
    {
        return pools.TryGetValue(poolId, out ObjectPool pool) ? pool.ActiveObjects : null;
    }

    public void DestroyPool(string poolId)
    {
        if (pools.TryGetValue(poolId, out ObjectPool pool))
        {
            pool.DestroyPool();
            pools.Remove(poolId);

            if (poolParents.TryGetValue(poolId, out Transform parent))
            {
                if (parent != null)
                {
                    Destroy(parent.gameObject);
                }
                poolParents.Remove(poolId);
            }
        }
    }

}