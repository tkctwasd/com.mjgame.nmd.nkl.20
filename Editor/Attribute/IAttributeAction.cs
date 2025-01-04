#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MJGame
{

    internal interface IAttributeAction
    {
        string name { get; }
        bool visibility { get; set; }

        void OnSceneGUI(SerializedProperty property,
            Attribute attribute, FieldInfo fieldInfo,
            Transform transform, AttributeSettings settings);
    }
}
#endif