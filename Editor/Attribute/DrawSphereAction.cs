#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MJGame {

internal class DrawSphereAction : AttributeAction<DrawSphereAttribute>
{
    protected override void OnSceneGUI(SerializedProperty property)
    {
        if (property.isArray)
        {
            int arraySize = property.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                DrawSphere(property.GetArrayElementAtIndex(i));
            }
        }
        else
        {
            DrawSphere(property);
        }
    }

    private void DrawSphere(SerializedProperty property)
    {
        if (!TryGetPosition(property, out Vector3 position)) return;

        using (new DrawingScope(attribute.Color, attribute.Alpha))
        {
            if (attribute.IsLocal)
            {
                position = transform.TransformPoint(position);
            }

            float handleSize = HandleUtility.GetHandleSize(position) * attribute.Size;
            Handles.SphereHandleCap(0, position, Quaternion.identity, handleSize, EventType.Repaint);
        }
    }
}
}
#endif