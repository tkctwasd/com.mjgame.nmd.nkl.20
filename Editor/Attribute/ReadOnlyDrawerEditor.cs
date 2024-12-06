using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace MJGame
{

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawerEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadOnlyAttribute readOnly = (ReadOnlyAttribute)attribute;

            if (readOnly.DefaultValue != null)
            {
                try
                {
                    switch (property.propertyType)
                    {
                        case SerializedPropertyType.Float:
                            if (property.floatValue == 0f)
                            {
                                if (readOnly.DefaultValue is float defaultFloat)
                                    property.floatValue = defaultFloat;
                            }
                            break;

                        case SerializedPropertyType.Integer:
                            if (property.intValue == 0)
                            {
                                if (readOnly.DefaultValue is int defaultInt)
                                    property.intValue = defaultInt;
                            }
                            break;

                        case SerializedPropertyType.Boolean:
                            if (!property.boolValue)
                            {
                                if (readOnly.DefaultValue is bool defaultBool)
                                    property.boolValue = defaultBool;
                            }
                            break;
                        case SerializedPropertyType.ObjectReference:
                            if (property.objectReferenceValue == null)
                            {
                            }
                            break;

                        default:
                            Debug.LogWarning($"ReadOnlyDrawer: Unsupported property type '{property.propertyType}' for {property.name}.");
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"ReadOnlyDrawer: Failed to assign default value to {property.name}. Error: {ex.Message}");
                }
            }

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
