using System;
using System.Collections.Generic;
namespace MJGame
{
    [Serializable]
    public partial class PlayerProfile
    {
        public string packageName;

        public float volume = 1.0f;
        public PlayerProfile()
        {
            packageName = "com.mjgame.nmd.nkl.20";
            volume = 1.0f;
        }
    }
}