using UnityEngine;
using System.Collections.Generic;


namespace MJGame
{

    public static class SaveExtension
    {
        #region Bool 
        /// <summary>
        ///  GetBool
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool(string key)
        {
            return (PlayerPrefs.GetInt(key, 0) == 1);
        }

        /// <summary>
        /// GetBool
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            return (PlayerPrefs.GetInt(key, (defaultValue ? 1 : 0)) == 1);
        }

        /// <summary>
        /// SetBool
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, (value ? 1 : 0));
        }

        #endregion

        #region Vector 2 

        /// <summary>
        /// GetVector2
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Vector2 GetVector2(string key)
        {
            return Get<Vector2>(key, Vector2.zero);
        }

        /// <summary>
        /// GetVector2
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Vector2 GetVector2(string key, Vector2 defaultValue)
        {
            return Get<Vector2>(key, defaultValue);
        }

        /// <summary>
        ///  SetVector2
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public static void SetVector2(string key, Vector2 value)
        {
            Set(key, value);
        }

        #endregion

        #region Vector 3 

        /// <summary>
        /// GetVector3
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Vector3 GetVector3(string key)
        {
            return Get<Vector3>(key, Vector3.zero);
        }

        /// <summary>
        /// GetVector3
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return Get<Vector3>(key, defaultValue);
        }

        /// <summary>
        /// SetVector3
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetVector3(string key, Vector3 value)
        {
            Set(key, value);
        }

        #endregion

        #region Vector 4 

        /// <summary>
        ///  GetVector4
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Vector4 GetVector4(string key)
        {
            return Get<Vector4>(key, Vector4.zero);
        }

        /// <summary>
        /// GetVector4
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Vector4 GetVector4(string key, Vector4 defaultValue)
        {
            return Get<Vector4>(key, defaultValue);
        }

        /// <summary>
        /// SetVector4
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetVector4(string key, Vector4 value)
        {
            Set(key, value);
        }

        #endregion

        #region Color 
        /// <summary>
        /// GetColor
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Color GetColor(string key)
        {
            return Get<Color>(key, new Color(0f, 0f, 0f, 0f));
        }

        /// <summary>
        /// GetColor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Color GetColor(string key, Color defaultValue)
        {
            return Get<Color>(key, defaultValue);
        }


        /// <summary>
        ///  SetColor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public static void SetColor(string key, Color value)
        {
            Set(key, value);
        }

        #endregion

        #region Quaternion 

        /// <summary>
        /// GetQuaternion
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternion(string key)
        {
            return Get<Quaternion>(key, Quaternion.identity);
        }

        /// <summary>
        /// GetQuaternion
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
        {
            return Get<Quaternion>(key, defaultValue);
        }

        /// <summary>
        ///  SetQuaternion
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public static void SetQuaternion(string key, Quaternion value)
        {
            Set(key, value);
        }

        #endregion

        #region List <T> 

        public class ListWrapper<T>
        {
            public List<T> list = new();
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetList<T>(string key)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T>()).list;
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetList<T>(string key, List<T> defaultValue)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T> { list = defaultValue }).list;
        }

        /// <summary>
        /// SetList
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetList<T>(string key, List<T> value)
        {
            Set(key, new ListWrapper<T> { list = value });
        }

        #endregion


        #region Object 

        /// <summary>
        /// GetObject
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(string key)
        {
            return Get<T>(key, default);
        }

        /// <summary>
        /// GetObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue);
        }

        /// <summary>
        /// SetObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetObject<T>(string key, T value)
        {
            Set(key, value);
        }

        #endregion

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static T Get<T>(string key, T defaultValue)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key, JsonUtility.ToJson(defaultValue)));
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        static void Set<T>(string key, T value)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }

    }
}