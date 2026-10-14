using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Gan len field int: Inspector hien dropdown theo enum game danh dau [IndicatorTypeEnum].
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class IndicatorTypeAttribute : PropertyAttribute
{
}

/// <summary>
/// Game danh dau enum chua cac loai indicator (gia tri 0 = None, khong doi so da dung).
/// <code>[IndicatorTypeEnum] public enum IndicatorType { None = 0, Sell = 1 }</code>
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public class IndicatorTypeEnumAttribute : Attribute
{
}



#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(IndicatorTypeAttribute))]
public class IndicatorTypeDrawer : PropertyDrawer
{
    private static string[] names;
    private static int[] values;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Integer)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        if (names == null) Load();
        if (names.Length == 0)
        {
            EditorGUI.PropertyField(position, property, new GUIContent($"{label.text} (no [IndicatorTypeEnum])"));
            return;
        }

        EditorGUI.BeginProperty(position, label, property);
        int current = property.intValue;
        if (Array.IndexOf(values, current) < 0)
        {
            // So khong co trong enum: van hien de khong mat du lieu
            property.intValue = EditorGUI.IntField(position, $"{label.text} (?)", current);
        }
        else
        {
            property.intValue = EditorGUI.IntPopup(position, label.text, current, names, values);
        }
        EditorGUI.EndProperty();
    }

    private static void Load()
    {
        var enumType = TypeCache.GetTypesWithAttribute<IndicatorTypeEnumAttribute>().FirstOrDefault(t => t.IsEnum);
        if (enumType == null)
        {
            names = Array.Empty<string>();
            values = Array.Empty<int>();
            return;
        }
        values = Enum.GetValues(enumType).Cast<object>().Select(Convert.ToInt32).ToArray();
        names = Enum.GetNames(enumType);
    }
}

#endif