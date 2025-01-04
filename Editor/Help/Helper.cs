#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace MJGame {

public static class Helper
{

    #region Attribute
    public static void OnSceneGUI(this Attribute attribute, SerializedProperty property,
           FieldInfo fieldInfo, Transform transform, AttributeSettings settings)
    {
        var action = AttributeActionCollector.GetAction(attribute.GetType());
        action?.OnSceneGUI(property, attribute, fieldInfo, transform, settings);
    }
    #endregion

    #region Base Color
    public static UnityEngine.Color ToUnityColor(this BaseColor color, float alpha=1)
    {
        switch (color)
        {
            case BaseColor.Black: return UnityEngine.Color.black.SetAlpha(alpha);
            case BaseColor.Blue: return UnityEngine.Color.blue.SetAlpha(alpha);
            case BaseColor.Green: return UnityEngine.Color.green.SetAlpha(alpha);
            case BaseColor.Red: return UnityEngine.Color.red.SetAlpha(alpha);
            case BaseColor.White: return UnityEngine.Color.white.SetAlpha(alpha);
            case BaseColor.Yellow: return UnityEngine.Color.yellow.SetAlpha(alpha);
        }
        return UnityEngine.Color.white.SetAlpha(alpha);
    }
    public static UnityEngine.Color SetAlpha(this UnityEngine.Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    #endregion

    #region  SerializedProperty
    public static int GetID(this SerializedProperty property)
    {
        return property.serializedObject.targetObject.GetInstanceID() ^ property.propertyPath.GetHashCode();
    }

    public static int GetArrayElementIndex(this SerializedProperty property)
    {
        var path = property.propertyPath;//xxx.Array.data[n]
        int s = path.LastIndexOf('[');
        string n = path.Substring(s + 1, path.Length - s - 2);
        return int.Parse(n);
    }

    public static SerializedProperty GetNeighborProperty(this SerializedProperty property, string neighborName)
    {
        //a
        //a.Array.data[n]
        //a.b...n
        //a.b...n.Array.data[n]
        //a.b...n.Array.data[n].v

        string path = property.propertyPath;

        if (path.Contains("."))
        {
            if (path.EndsWith("]"))
            {
                int cut = 0;
                for (int i = path.Length - 1, dot = 0; i > -1; i--)
                {
                    if (path[i] == '.')
                    {
                        dot++;
                    }

                    if (dot == 2)
                    {
                        cut = i;
                    }

                    if (dot == 3)
                    {
                        cut = i + 1;
                        break;
                    }
                }

                path = path.Substring(0, cut) + neighborName;
                return property.serializedObject.FindProperty(path);
            }
            else
            {
                for (int i = path.Length - 1; i > -1; i--)
                {
                    if (path[i] == '.')
                    {
                        path = path.Substring(0, i + 1) + neighborName;
                        return property.serializedObject.FindProperty(path);
                    }
                }
            }
        }

        return property.serializedObject.FindProperty(neighborName);
    }

    public static void Duplicate(this SerializedProperty property)
    {
        var element = property;
        if (element.TryGetParentProperty(out var parent))
        {
            if (!parent.isArray && parent.IsArrayElement())
            {
                element = parent;
                parent = parent.GetParentProperty();
            }

            if (parent.isArray)
            {
                EditorApplication.delayCall += () =>
                {
                    if (element.propertyType == SerializedPropertyType.ObjectReference && element == property)
                    {
                        var obj = element.objectReferenceValue;
                        if (obj != null)
                        {
                            element.DuplicateCommand();
                            obj = UnityEngine.Object.Instantiate(obj);
                            obj.name = obj.name.Replace("(Clone)", "");
                            element.objectReferenceValue = obj;
                        }
                    }
                    else
                    {
                        element.DuplicateCommand();
                    }

                    parent.serializedObject.ApplyModifiedProperties();
                };

                return;
            }
        }

        Debug.LogWarning($"The '{property.propertyPath}' can't be duplicated.\nIt should be an array element or a property of an array element.");
    }

    public static void Delete(this SerializedProperty property)
    {
        var element = property;
        if (element.TryGetParentProperty(out var parent))
        {
            if (!parent.isArray && parent.IsArrayElement())
            {
                element = parent;
                parent = parent.GetParentProperty();
            }

            if (parent.isArray)
            {
                EditorApplication.delayCall += () =>
                {
                    if (element.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        var obj = element.objectReferenceValue;
                        if (obj != null)
                        {
                            if (obj is Component) obj = (obj as Component).gameObject;
                            UnityEngine.Object.DestroyImmediate(obj);
                        }
                    }

                    element.DeleteCommand();
                    parent.serializedObject.ApplyModifiedProperties();
                };

                return;
            }
        }

        Debug.LogWarning($"The '{property.propertyPath}' can't be deleted.\nIt should be an array element or a property of an array element.");
    }

    public static bool TryGetParentProperty(this SerializedProperty property, out SerializedProperty parent)
    {
        string path = property.propertyPath;

        if (!path.Contains("."))
        {
            parent = null;
            return false;
        }

        if (path[path.Length - 1] == ']')
        {
            //xxx.parent.Array.data[n] => xxx.parent
            path = path.Remove(path.LastIndexOf(".A"));
            parent = property.serializedObject.FindProperty(path);
        }
        else
        {
            //xxx.parent.v => xxx.parent
            path = path.Remove(path.LastIndexOf("."));
            parent = property.serializedObject.FindProperty(path);
        }

        return true;
    }

    public static SerializedProperty GetParentProperty(this SerializedProperty property)
    {
        string path = property.propertyPath;

        if (path[path.Length - 1] == ']')
        {
            //xxx.parent.Array.data[n] => xxx.parent
            path = path.Remove(path.LastIndexOf(".A"));
            return property.serializedObject.FindProperty(path);
        }

        //xxx.parent.v => xxx.parent
        path = path.Remove(path.LastIndexOf("."));
        return property.serializedObject.FindProperty(path);
    }

    public static FieldInfo GetFieldInfo(this SerializedProperty property)
    {
        string path = property.propertyPath;

        if (path.EndsWith("]")) return null;

        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        Type type = property.serializedObject.targetObject.GetType();
        if (!path.Contains(".")) return type.GetField(path, bindingFlags);

        FieldInfo field = null;
        string[] paths = path.Split('.');
        for (int i = 0; i < paths.Length; i++)
        {
            if (type.IsArray)
            {
                type = type.GetElementType();
                field = null;
                i++;//skip "data[n] or size" (xxx.Array.data[n].yyy) (xxx.Array.size)
                continue;
            }

            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
                field = null;
                i++;
                continue;
            }

            field = type.GetField(paths[i], bindingFlags);
            if (field == null) return null;
            type = field.FieldType;
        }

        return field;
    }

    public static bool TryGetNumber(this SerializedProperty property, ref float f)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Float: { f = property.floatValue; return true; }
            case SerializedPropertyType.Integer: { f = property.intValue; return true; }
        }
        return false;
    }

    public static bool TryGetTransform(this SerializedProperty property, out Transform t)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            if (property.objectReferenceValue != null)
            {
                if (property.type == "PPtr<$Transform>")
                {
                    t = (Transform)property.objectReferenceValue;
                    return true;
                }
                else if (property.type == "PPtr<$GameObject>")
                {
                    t = ((GameObject)property.objectReferenceValue).transform;
                    return true;
                }
            }
        }

        t = null;
        return false;
    }
    #endregion

    #region Vector
    public static Vector3Int Vector3Int(this Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }

    public static Vector2Int Vector2Int(this Vector3 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }
    public static Vector3 Vector3(this Vector2Int v)
    {
        return new Vector3(v.x, v.y);
    }
    private static bool TryParseToVector3(this string text, out Vector3 v)
    {
        var axis = text.Split(' ', ',');
        var result = axis.Length == 3;
        if (result)
        {
            result |= float.TryParse(axis[0], out v.x);
            result |= float.TryParse(axis[1], out v.y);
            result |= float.TryParse(axis[2], out v.z);
        }
        else
        {
            v = UnityEngine.Vector3.zero;
        }

        return result;
    }
    #endregion

    #region Quaternion
    public static Quaternion ToQuaternion(this QuationCircle quation)
    {
        switch (quation)
        {
            case QuationCircle.XZ:
                return Quaternion.Euler(90, 0, 0);
            case QuationCircle.XY:
                return Quaternion.Euler(0, 0, 90);
            case QuationCircle.YZ:
                return Quaternion.Euler(0, 90, 0);
            default: return Quaternion.identity;
        }
    }

    internal static void RemoveMissingScriptsInScene()
    {
        var objects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach(var go in objects)
        {
           GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        }
    }
    #endregion
}

}
#endif