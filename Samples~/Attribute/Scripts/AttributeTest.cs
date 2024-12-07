using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MJGame
{
    public class AttributeTest : TickBehaviour
    {
        [Static]
        public static int staticDemo = 42;
        [ReadOnly] public int readOnlyInt = 42;

        [Button]
        public void ButtonAttribute()
        {
            Debug.Log("HandleButtonAttribute");
        }
        [HorizontalLine()]
        [UseAxisLock(Axis.X)] public Vector3 position;
    }
}