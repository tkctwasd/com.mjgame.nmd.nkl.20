using System;
using System.Collections.Generic;
namespace MJGame
{

    [Serializable]
    public class PlayerProfile
    {
        public bool Sound;
        public List<int> LstModePlay = new();

        public int Level = 0;

        public PlayerProfile()
        {
            Sound = true;
            LstModePlay = new List<int>();
            Level = 0;
        }
    }

}