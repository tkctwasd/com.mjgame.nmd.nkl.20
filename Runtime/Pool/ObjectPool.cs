using System.Collections;
using System.Collections.Generic;
using MJGame;
using UnityEngine;

namespace MJGame
{
    public class ObjectPool : TickBehaviour
    {
        [Button("Return")]
        public virtual void Return()
        {
            SingletonComponent<PoolManager>.Instance.ReturnObject(this, this.name);
        }
    }
}
