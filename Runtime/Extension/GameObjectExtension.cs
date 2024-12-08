using UnityEngine;

namespace MJGame
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// Tìm hoặc thêm một component vào GameObject.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out var component))
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Kích hoạt hoặc tắt GameObject.
        /// </summary>
        public static void ActiveObj(this GameObject go, bool state)
        {
            if (go != null && go.activeSelf != state)
            {
                go.SetActive(state);
            }
        }
    }

}