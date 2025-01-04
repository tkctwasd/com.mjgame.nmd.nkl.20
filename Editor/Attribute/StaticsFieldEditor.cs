#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace MJGame {

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)] 
public class StaticFieldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var type = target.GetType();

        // static, public non-public
        var staticFields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        string nameClass=type.Name+".";
        foreach(var field in staticFields)
        {
            // Check StaticField
            if(field.IsDefined(typeof(StaticAttribute), false))
            {
                EditorGUILayout.LabelField("Static Fields", EditorStyles.boldLabel);

                object value = field.GetValue(null); 

                if(field.FieldType == typeof(int))
                {
                    int newValue = EditorGUILayout.IntField(nameClass + field.Name, (int)value);
                    field.SetValue(null, newValue);
                }
                else if(field.FieldType == typeof(float))
                {
                    float newValue = EditorGUILayout.FloatField(nameClass + field.Name, (float)value);
                    field.SetValue(null, newValue);
                }
                else if(field.FieldType == typeof(string))
                {
                    string newValue = EditorGUILayout.TextField(nameClass + field.Name, (string)value);
                    field.SetValue(null, newValue);
                }
                else if(field.FieldType == typeof(bool))
                {
                    bool newValue = EditorGUILayout.Toggle(nameClass + field.Name, (bool)value);
                    field.SetValue(null, newValue);
                }
                else
                {
                    EditorGUILayout.LabelField(nameClass + field.Name, value != null ? value.ToString() : "null");
                }
            }
        }
    }
}
}
#endif