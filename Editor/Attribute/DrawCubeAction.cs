#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
namespace MJGame
{

    internal class DrawCubeAction : AttributeAction<DrawCubeAttribute>
    {
        protected override void OnSceneGUI(SerializedProperty property)
        {
            if (property.isArray)
            {
                int arraySize = property.arraySize;
                for (int i = 0; i < arraySize; i++)
                {
                    DrawCube(property.GetArrayElementAtIndex(i));
                }
            }
            else
            {
                DrawCube(property);
            }
        }

        private void DrawCube(SerializedProperty property)
        {
            if (TryGetPosition(property, out Vector3 position))
            {
                Quaternion rotation = Quaternion.identity;
                if (attribute.IsLocal)
                {
                    position = transform.TransformPoint(position);
                    rotation = transform.rotation;
                }
                using (var scope = new DrawingScope(attribute.Color, attribute.Alpha))
                {
                    float handleSize = HandleUtility.GetHandleSize(position) * attribute.Size;
                    Handles.CubeHandleCap(0, position, rotation, handleSize, EventType.Repaint);
                    if (attribute.IsDisplayName)
                    {
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        GUI.skin.label.normal.textColor = scope.Color;
                        Handles.Label(position, property.displayName);
                    }
                }
            }
        }
    }
}
#endif