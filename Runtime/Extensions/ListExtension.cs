using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MJGame
{

    public static class ListExtension
    {
        /// <summary>
        ///  Extension để xáo trộn List
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Extension để lấy phần tử ngẫu nhiên trong List
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;
            int index = RandomExtension.Range(list.Count);
            return list[index];
        }

        /// <summary>
        /// Extension để kiểm tra xem List có chứa phần tử hay không (trong trường hợp so sánh với null)
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ContainsNull<T>(this List<T> list)
        {
            return list.Contains(default);
        }

        /// <summary>
        /// Extension để thêm phần tử vào List nếu nó không tồn tại
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddIfNotExist<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
    }
}