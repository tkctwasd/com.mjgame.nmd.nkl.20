using System;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{

    public class Pool<T> : IEnumerable<T> where T : ObjectPool
    {
        private List<T> pool = new List<T>();
        private GameObject prefab;
        private Transform parent;
        private string key;

        public Pool(PoolItem item, Transform parent)
        {
            this.key = item.name;
            this.prefab = item.prefab;
            this.parent = parent;

            for (int i = 0; i < item.size; i++)
            {
                Spawn(false);
            }
        }

        public T GetObject()
        {
            foreach (var obj in pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    GameObjectExtension.ActiveObj(obj?.gameObject, true);
                    return obj;
                }
            }
            return Spawn(true);
        }

        private T Spawn(bool active = false)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            GameObjectExtension.ActiveObj(obj, active);
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                throw new InvalidOperationException($"Prefab {prefab.name} does not have a component of type {typeof(T).Name}.");
            }
            component.gameObject.name = key;
            pool.Add(component);
            return component;
        }

        public void ReturnObject(T obj)
        {
            GameObjectExtension.ActiveObj(obj?.gameObject, false);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return pool.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }

}
