using System;
using System.Diagnostics;
using UnityEngine;
namespace MJGame {

[AttributeUsage(AttributeTargets.Field)]
[Conditional("UNITY_EDITOR")]
public class StaticAttribute : PropertyAttribute
{    
}

}