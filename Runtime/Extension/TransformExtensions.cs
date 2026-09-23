using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityExtensions
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class TransformExtensions
    {

        /// <summary>
        /// lấy tất cả các con của nó
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllChildComponents<T>(this Transform parent) where T : Component
        {
            List<T> result = new List<T>();

            foreach (Transform child in parent)
            {
                T comp = child.GetComponent<T>();
                if (comp != null)
                    result.Add(comp);

                result.AddRange(child.GetAllChildComponents<T>());
            }

            return result;
        }

        /// <summary>
        /// lấy các component của con tầng 1
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public static List<T> GetDirectChildComponents<T>(this Transform parent) where T : Component
        {
            List<T> result = new List<T>();

            foreach (Transform child in parent)
            {
                T comp = child.GetComponent<T>();
                if (comp != null)
                    result.Add(comp);
            }

            return result;
        }
        
        /// <summary>
        /// lấy tất cả component bỏ qua chính nó 
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetComponentsInChildrenExcludeSelf<T>(this Transform parent) where T : Component
        {
            List<T> result = new List<T>();

            foreach (Transform child in parent)
            {
                result.AddRange(child.GetComponentsInChildren<T>());
            }

            return result;
        }
    }
}