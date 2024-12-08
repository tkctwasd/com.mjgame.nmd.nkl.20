using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{

    /// <summary>
    /// Only valid for ListContainer! Used to fix overriding of other attributes
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ListContainerAttribute : PropertyAttribute { }
}