using System;
using Newtonsoft.Json;
using UnityEngine;

namespace MJGame
{

    public class PlayerPrefsData<T> : IData<T> where T : class
    {
        public string key;
        public PlayerPrefsData(string key)
        {
            this.key = key;
        }
        public T LoadData()
        {
            T result = default;
            string data = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(data))
            {
                result = JsonConvert.DeserializeObject<T>(data);
            }
            if (result == null)
            {
                result = (T)Activator.CreateInstance(typeof(T));
            }

            return result;
        }

        public void Save(string data)
        {
            PlayerPrefs.SetString(key, data);
        }
    }
}