using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions
{
    public static class GameObjectExtensions
    {
        public static void ActiveObj(this GameObject gameObject, bool state)
        {
            if (gameObject != null && gameObject.activeSelf != state)
                gameObject.SetActive(state);
        }
    }
}
