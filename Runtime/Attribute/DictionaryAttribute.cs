using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame {

/// <summary>
/// Only valid for dictionary's! Used for display in the inspector
/// </summary>
[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class DictionaryAttribute : PropertyAttribute
{
    /// <summary>
    /// Only on SerializableDictionary: Overrides the key-column-header with a custom label
    /// </summary>
    public string keyLabel = null;
    /// <summary>
    /// Only on SerializableDictionary: Overrides the value-column-header with a custom label
    /// </summary>
    public string valueLabel = null;

    public readonly float keySize;
    public const float defaultKeySize = .4f;
    public DictionaryAttribute(float keySize = defaultKeySize)
    {
        if (keySize <= 0)
        {
            Debug.LogWarning($"{nameof(keySize)} has to be greater zero");
            keySize = defaultKeySize;
        }
        else if (keySize >= 1)
        {
            Debug.LogWarning($"{nameof(keySize)} has to be between 0 and 1, because it defines the proportion of space for keys and values");
            keySize = defaultKeySize;
        }
        this.keySize = keySize;
    }
}

}