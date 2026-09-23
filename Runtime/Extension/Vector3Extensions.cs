using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityExtensions
{
    public static class Vector3Extensions
    {
        public static float DistanceIndependentY(Vector3 a, Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.z - b.z;
            return (float)Mathf.Sqrt(num * num + num2 * num2);
        }
    }
}
