using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{


    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Data/AudioConfigSO", order = 0)]
    public class AudioConfigSO : ScriptableObject
    {
        public List<ItemAudio> bgClips = new List<ItemAudio>();
        public List<ItemAudio> vfxClips = new List<ItemAudio>();
        public List<ItemAudio> sfxClips = new List<ItemAudio>();
    }

}