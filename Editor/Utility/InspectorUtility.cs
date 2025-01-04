#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace MJGame {

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class InspectorUtility : Editor
{
    Dictionary<string, object[]> methodParameters = new Dictionary<string, object[]>();

    private delegate void PropertyFieldFunction(Rect rect, SerializedProperty property, GUIContent label, bool includeChildren);

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawButtons();
    }

    protected void DrawButtons(bool drawHeader = false)
    {
        var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach(var method in methods)
        {
            var buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

            if(buttonAttribute != null)
            {
                string buttonLabel = string.IsNullOrEmpty(buttonAttribute.Text) ? method.Name : buttonAttribute.Text;
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                var parameters = method.GetParameters();
              
                if(GUILayout.Button(buttonLabel))
                {
                    if(parameters.Length > 0)
                    {
                        method.Invoke(target, methodParameters[method.Name]);
                    }
                    else
                    {
                        method.Invoke(target, null);
                    }
                }
                if(parameters.Length > 0)
                {
                    EditorGUIUtility.labelWidth = 100;
                    if(!methodParameters.ContainsKey(method.Name))
                    {
                        methodParameters[method.Name] = new object[parameters.Length];
                    }
                    for(int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        if(param.ParameterType == typeof(int))
                        {
                            methodParameters[method.Name][i] = EditorGUILayout.IntField(param.Name, (int)(methodParameters[method.Name][i] ?? 0));
                        }
                        else if(param.ParameterType == typeof(float))
                        {
                            methodParameters[method.Name][i] = EditorGUILayout.FloatField(param.Name, (float)(methodParameters[method.Name][i] ?? 0f));
                        }
                        else if(param.ParameterType == typeof(string))
                        {
                            methodParameters[method.Name][i] = EditorGUILayout.TextField(param.Name, (string)(methodParameters[method.Name][i] ?? ""));
                        }
                        else if(param.ParameterType == typeof(bool))
                        {
                            methodParameters[method.Name][i] = EditorGUILayout.Toggle(param.Name, (bool)(methodParameters[method.Name][i] ?? false));
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Parameter type {param.ParameterType} not supported");
                        }
                    }
                    EditorGUILayout.Space(15);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
}
#endif