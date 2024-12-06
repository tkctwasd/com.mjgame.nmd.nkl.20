using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReadOnlyAttribute : PropertyAttribute
{
    public object DefaultValue { get; private set; }

    public ReadOnlyAttribute() { }

    public ReadOnlyAttribute(object defaultValue)
    {
        DefaultValue = defaultValue;
    }
}