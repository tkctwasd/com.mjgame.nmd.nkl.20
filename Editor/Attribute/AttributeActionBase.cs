#if UNITY_EDITOR


using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;
using System.Reflection;
namespace MJGame
{

    public abstract class AttributeActionBase : IAttributeAction
    {
        protected static int selectedContolID;

        private bool m_Initialized;

        public string name => m_Name;
        private string m_Name;

        public bool visibility
        {
            get
            {
                return m_Visibility;
            }
            set
            {
                if (m_Visibility != value)
                {
                    m_Visibility = value;
                    EditorPrefs.SetBool(visibilityKey, value);
                }
            }
        }

        private string visibilityKey => $"{m_Name}.Visibility";
        private bool m_Visibility;

        private AttributeSettings m_Settings;
        private Transform m_Transform;
        private FieldInfo m_FieldInfo;
        private Type m_FieldType;
        private bool m_CanDuplicate;

        protected Transform transform => m_Transform;
        protected FieldInfo fieldInfo => m_FieldInfo;
        protected Type fieldType => m_FieldType;

        private void Init()
        {

        }

        public void OnSceneGUI(SerializedProperty property, Attribute attribute, FieldInfo fieldInfo, Transform transform, AttributeSettings settings)
        {
#if SUPPORTS_SCENE_VIEW_OVERLAYS
            if (!m_Initialized)
            {
                m_Name = attribute.GetType().Name;
                string key = visibilityKey;
                if (EditorPrefs.HasKey(key))
                {
                    m_Visibility = EditorPrefs.GetBool(key);
                }
                else
                {
                    m_Visibility = true;
                    EditorPrefs.SetBool(key, true);
                }
                m_Initialized = true;
            }

            if (!m_Visibility) return;
#endif

            m_Transform = transform;
            m_FieldInfo = fieldInfo;
            m_FieldType = fieldInfo.FieldType;
            m_Settings = settings;

            if (settings.useLocalSpace)
            {
                var parentName = settings.localSpaceAttr.transformField;
                if (parentName != null)
                {
                    var parentProperty = property.GetNeighborProperty(parentName);
                    if (parentProperty.TryGetTransform(out var parent))
                    {
                        m_Transform = parent;
                    }
                }
            }

            OnSceneGUI(property, attribute);
        }



        protected abstract void OnSceneGUI(SerializedProperty property, Attribute attribute);

        public bool TryGetPosition(SerializedProperty property, out Vector3 position)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Object obj = property.objectReferenceValue;
                if (obj != null)
                {
                    string type = property.type;
                    if (type == "PPtr<$Transform>")
                    {
                        position = (obj as Transform).position;
                        return true;
                    }
                    else if (type == "PPtr<$GameObject>")
                    {
                        position = (obj as GameObject).transform.position;
                        return true;
                    }
                }
            }
            else
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Vector3: position = property.vector3Value; break;
                    case SerializedPropertyType.Vector2: position = property.vector2Value; break;
                    case SerializedPropertyType.Vector3Int: position = property.vector3IntValue; break;
                    case SerializedPropertyType.Vector2Int: position = property.vector2IntValue.Vector3(); break;
                    default: { position = Vector3.zero; return false; };
                }


                return true;
            }

            position = Vector3.zero;
            return false;
        }

        public bool TryGetPositions(SerializedProperty property, out Vector3[] positions, bool loop)
        {
            int length = property.arraySize;

            if (length < 1)
            {
                positions = null;
                return false;
            }

            List<Vector3> list = new List<Vector3>(length);
            Vector3 position;
            for (int i = 0; i < length; i++)
            {
                if (TryGetPosition(property.GetArrayElementAtIndex(i), out position))
                {
                    list.Add(position);
                }
                else
                {
                    positions = null;
                    return false;
                }
            }

            if (loop) list.Add(list[0]);

            positions = list.ToArray();
            return true;
        }

        private Vector3 LockAxis(Vector3 prev, Vector3 current, Axis axis)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(HandleUtility.WorldToGUIPoint(current));
            ray.origin -= ray.direction * 1000;
            Vector3 normal = Vector3.zero;
            if (axis == Axis.X) normal.x = 1;
            if (axis == Axis.Y) normal.y = 1;
            if (axis == Axis.Z) normal.z = 1;
            Plane plane = new Plane(normal, prev);

            if (plane.Raycast(ray, out float enter))
            {
                current = ray.GetPoint(enter);
                if (axis == Axis.X) current.x = prev.x;
                if (axis == Axis.Y) current.y = prev.y;
                if (axis == Axis.Z) current.z = prev.z;
            }
            else
            {
                current = prev;
            }

            return current;
        }
    }
}
#endif