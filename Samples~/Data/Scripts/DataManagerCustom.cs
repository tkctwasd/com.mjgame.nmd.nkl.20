using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{
    public class DataManagerCustom
    {
        public static PlayerUtility PlayerUtility { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnInit()
        {
            PlayerUtility = new PlayerUtility("DataPlayer");
            Debug.Log("DataManagerCustom has been initialized and added to the scene.");
        }
    }
}