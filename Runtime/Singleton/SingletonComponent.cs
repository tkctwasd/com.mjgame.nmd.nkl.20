using UnityEngine;

namespace MJGame
{
    public class SingletonComponent<T> : TickBehaviour where T : Object
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                if (instance == null)
                {
                    Debug.LogWarningFormat("[SingletonComponent] Returning null instance for component of type {0}.", new object[]
                    {
                        typeof(T)
                    });
                }
                return instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            this.SetInstance();
        }

        public static bool Exists()
        {
            return instance != null;
        }

        public bool SetInstance()
        {
            if (instance != null && instance != base.gameObject.GetComponent<T>())
            {
                Debug.LogWarning("[SingletonComponent] Instance already set for type " + typeof(T));
                return false;
            }
            instance = base.gameObject.GetComponent<T>();
            return true;
        }
    }

}