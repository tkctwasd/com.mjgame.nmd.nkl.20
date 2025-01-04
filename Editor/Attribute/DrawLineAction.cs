#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
namespace MJGame
{

    internal class DrawLineAction : AttributeAction<DrawLineAttribute>
    {
        protected override void OnSceneGUI(SerializedProperty property)
        {
            DrawLine(property, attribute);
        }

        private void DrawLine(SerializedProperty property, DrawLineAttribute attribute)
        {
            if (!TryGetPosition(property, out Vector3 endPoint)) return;

            Vector3 startPoint = transform.position;
            if (attribute.IsLocal)
            {
                if (property.propertyType == SerializedPropertyType.Vector3 ||
                    property.propertyType == SerializedPropertyType.Vector2 ||
                    property.propertyType == SerializedPropertyType.Vector3Int ||
                    property.propertyType == SerializedPropertyType.Vector2Int)
                {
                    endPoint = transform.TransformPoint(endPoint);
                }
            }
            using (new DrawingScope(attribute.Color))
            {
                Handles.DrawLine(startPoint, endPoint, attribute.Thickness);
            }
        }
    }
}
#endif