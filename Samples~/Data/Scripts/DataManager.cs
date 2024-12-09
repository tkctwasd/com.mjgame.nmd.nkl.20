using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{
    public partial class DataManager
    {
        public static PlayerUtility PlayerUtility { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void OnInit()
        {
            PlayerUtility = new PlayerUtility("GamePlay_Data");
        }
    }
}

