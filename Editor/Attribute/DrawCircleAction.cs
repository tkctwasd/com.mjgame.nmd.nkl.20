#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
namespace MJGame
{


internal class DrawCircleAction : AttributeAction<DrawCircleAttribute>
{
    protected override void OnSceneGUI(SerializedProperty property)
    {
        if (property.isArray)
        {
            int arraySize = property.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                DrawCircle(property.GetArrayElementAtIndex(i));
            }
        }
        else
        {
            DrawCircle(property);
        }
    }

    private void DrawCircle(SerializedProperty property)
    {
        Vector3 position;
        float radius;

        if (property.propertyType == SerializedPropertyType.Float)
        {
            position = transform.position;
            radius = property.floatValue;
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            position = transform.position;
            radius = property.intValue;
        }
        else
        {
            if (!TryGetPosition(property, out position)) return;

            radius = attribute.Radius;

        }

        using (var scope = new DrawingScope(attribute.Color))
        {
            Quaternion q = attribute.Rotation.ToQuaternion();

            Handles.CircleHandleCap(0, position, q, radius, EventType.Repaint);
            if (attribute.IsDisplayName)
            {
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.skin.label.normal.textColor = scope.Color;
                Handles.Label(position, transform.name, GUI.skin.label);
            }
        }
    }
}
}
#endif