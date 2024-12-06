using System;
using System.Collections.Generic;
using MJGame;
using UnityEngine;

namespace MJGame
{
    public class PoolManager :  SingletonComponent<PoolManager>
    {
        private Dictionary<string, Pool<ObjectPool>> pools = new();

        private List<PoolItem> poolItems = new();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            LoadConfig();
            try
            {
                foreach (var item in poolItems)
                {
                    Pool<ObjectPool> o = new(item, this.transform);
                    pools.Add(item.name, o);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"PoolManager: {"Error creating pools"}\nException: {ex.Message}");
            }
        }

        private void LoadConfig()
        {
            PoolConfigSO config = Resources.Load<PoolConfigSO>("PoolConfig");
            if (config == null)
            {
                Debug.Log("PoolConfigSO not found in Resources folder.");
                return;
            }
            poolItems = config.poolItems;
        }

        public ObjectPool GetObject(string prefabName)
        {
            if (pools.ContainsKey(prefabName))
            {
                return pools[prefabName].GetObject();
            }
            Debug.LogWarning($"No pool found for: {prefabName}");
            return null;
        }

        public void ReturnObject(ObjectPool obj, string prefabName)
        {
            if (pools.TryGetValue(prefabName, out var pool))
            {
                pool.ReturnObject(obj);
            }
            else
            {
                Debug.LogWarning($"No pool found for prefab: {prefabName}. Destroying object.");
                Destroy(obj.gameObject);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            GameObject poolManagerObject = new("PoolManager");
            poolManagerObject.AddComponent<PoolManager>(); 
            Debug.Log("PoolManager has been initialized and added to the scene.");
        }
    }
}