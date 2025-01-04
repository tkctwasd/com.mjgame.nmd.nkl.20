#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace MJGame {

[CustomPropertyDrawer(typeof(SceneAttribute))]
public class SceneAttributeDrawer : PropertyDrawer
{
    bool isErrorMessage = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        isErrorMessage = false;
        string[] sceneNames = GetSceneNames((SceneAttribute)attribute);
        if (sceneNames.Length == 0)
        {
            DrawProperties.DrawPropertyWithMessage(position, label, property, "No scenes found in Build Settings", MessageType.Error, false);
            isErrorMessage = true;
            return;
        }
        GUIContent[] guiContents = sceneNames.Select((n, i) => new GUIContent($"{n} ({i})")).ToArray();

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.Popup(position, label, property.intValue, guiContents);
        }
        else if (property.propertyType == SerializedPropertyType.String)
        {
            int index = Array.IndexOf(sceneNames, property.stringValue);
            if (index == -1)
            {
                index = 0;
                property.stringValue = sceneNames[0];
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(position, label, index, guiContents);
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = sceneNames[index];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        else
        {
            DrawProperties.DrawPropertyWithMessage(position, label, property,
                                                nameof(SceneAttribute) + " is only available on Integers and Strings", MessageType.Error);
            isErrorMessage = true;
        }

        static string[] GetSceneNames(SceneAttribute attribute)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] sceneNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                if (attribute.useFullPath)
                    sceneNames[i] = scenes[i].path;
                else
                    sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            }
            return sceneNames;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * (isErrorMessage ? 4 : 1);
    }
}
}
#endif