#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace MJGame {

[CustomPropertyDrawer(typeof(ProgressAttribute))]
public class ProgressPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var Attribute = attribute as ProgressAttribute;

        if (property.propertyType != SerializedPropertyType.Float)
        {
            Debug.LogWarning("Progress attribute must be used with 'float' property type");
            base.OnGUI(position, property, label);
            return;
        }
        float value = property.floatValue;
        if (Attribute.UseSlider)
        {
            var component = property.serializedObject.FindProperty(Attribute.SliderName);
            if (component != null)
            {
                Slider slider = component.boxedValue as Slider;
                Attribute.Min = slider.minValue;
                Attribute.Max = slider.maxValue;
            }
            //Attribute.Min = slider.minValue;
            //Attribute.Max = slider.maxValue;
        }
        property.floatValue = EditorGUI.Slider(position, property.floatValue, Attribute.Min, Attribute.Max);
        property.serializedObject.ApplyModifiedProperties();
        float percent= (value / (Attribute.Max - Attribute.Min));
        EditorGUI.ProgressBar(position, percent, (percent).ToString("0.00%"));
        property.CallMethodOnOwner((attribute as ProgressAttribute).MethodName);
    }
}
}
#endif