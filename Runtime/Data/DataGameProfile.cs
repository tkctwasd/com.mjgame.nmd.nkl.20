using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{
    [Serializable]
    public partial class DataGameProfile
    {
        public float volume = 1f;
        public int level = 1;
        public int coin = 100;
        public int gem = 10;
        public int numberSpin = 3;
        public string packageName = "com.mjgame.nmd.nkl.20.vs-1.0.12";
        public DataGameProfile()
        {
        }
    }
}
