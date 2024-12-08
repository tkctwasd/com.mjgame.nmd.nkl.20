using System.Collections;
using System.Collections.Generic;


namespace MJGame
{

    public static class DictionaryExtension
    {

        /// <summary>
        /// Extension để lấy giá trị từ Dictionary nếu có, nếu không thì trả về giá trị mặc định
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return defaultValue;
        }

        /// <summary>
        /// Extension để thêm một cặp key-value vào Dictionary nếu key không tồn tại
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static void AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Extension để kiểm tra nếu Dictionary có chứa tất cả các keys trong một List
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool ContainsKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, List<TKey> keys)
        {
            foreach (var key in keys)
            {
                if (!dictionary.ContainsKey(key))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Extension để lấy tất cả các keys trong dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static List<TKey> GetAllKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new List<TKey>(dictionary.Keys);
        }

        /// <summary>
        /// Extension AddOrUpdate cho Dictionary, nếu key đã tồn tại thì cộng thêm valueToAdd vào value, nếu không thì thêm mới
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="valueToAdd"></param>
        /// <typeparam name="TKey"></typeparam>
        public static void AddOrUpdate<TKey>(this Dictionary<TKey, int> dictionary, TKey key, int valueToAdd = 1)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] += valueToAdd;
            }
            else
            {
                dictionary.Add(key, valueToAdd);
            }
        }

    }
}