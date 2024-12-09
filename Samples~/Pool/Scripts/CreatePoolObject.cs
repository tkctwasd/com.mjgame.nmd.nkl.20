using System.Collections;
using System.Collections.Generic;
using MJGame;
using UnityEngine;

namespace MJGame
{

    public class CreatePoolObject : TickBehaviour
    {
        [Button]
        public void Test()
        {
            SingletonComponent<PoolManager>.Instance.GetObject("ObjectPool");
        }
    }

}