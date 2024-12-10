using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MJGame
{
    public class DataManager
    {
        public static DataGameUtility DataGameUtility { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void OnInit()
        {
            DataGameUtility = new DataGameUtility("DataGameDefault");
            Debug.Log("DataManager has been initialized and added to the scene.");
        }
    }
}

