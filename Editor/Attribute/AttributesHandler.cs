#if UNITY_EDITOR

using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
namespace MJGame {

public class AttributesHandler
{
    [InitializeOnLoadMethod]
    private static void Init()
    {
        new AttributesHandler();
    }

    private TargetsProvider m_TargetsProvider;
    private TargetScript[] m_Targets;

    private AttributesHandler()
    {
        m_TargetsProvider = new TargetsProvider();

        SceneView.duringSceneGui += OnSceneGUI;
        TrackerUtility.OnTargetScriptsChanged += OnTargetScriptsChanged;
    }

    private void OnTargetScriptsChanged()
    {
        m_TargetsProvider.FindTargets(ref m_Targets, TrackerUtility.targetScripts);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (m_Targets == null) return;

        AttributeActionCollector.UsedActions.Clear();

        for (int i = 0; i < m_Targets.Length; i++)
        {
            m_Targets[i].OnSceneGUI();
        }
    }

    internal class TargetScript
    {
        private MonoBehaviour m_Script;
        private SerializedObject m_SerializedObject;
        private Transform m_Transform;

        public TargetScript(MonoBehaviour monoBehaviour, SerializedObject serializedObject)
        {
            m_Script = monoBehaviour;
            m_SerializedObject = serializedObject;
            m_Transform = monoBehaviour.transform;
        }

        public void OnSceneGUI()
        {
            if (m_Script == null) return;

            m_SerializedObject.Update();

            var property = m_SerializedObject.GetIterator();
            while (property.NextVisible(true))
            {

                var field = property.GetFieldInfo();
                if (field == null) continue;

                var settings = new AttributeSettings(field.GetCustomAttributes<SettingsAttribute>(false));

                foreach (var attribute in field.GetCustomAttributes<Attribute>(false))
                {
                    attribute.OnSceneGUI(property, field, m_Transform, settings);
                }
            }

            m_SerializedObject.ApplyModifiedProperties();
        }
    }
    private class TargetsProvider
    {
        public void FindTargets(ref TargetScript[] targets, MonoBehaviour[] scripts)
        {
            List<TargetScript> targetList = new List<TargetScript>();

            for (int i = 0; i < scripts.Length; i++)
            {
                var script = scripts[i];
                var serializedObject = new SerializedObject(script);
                var property = serializedObject.GetIterator();

                while (property.NextVisible(true))
                {
                    var field = property.GetFieldInfo();
                    if (field == null)
                    {
                        continue;
                    }

                    if (field.GetCustomAttributes(typeof(Attribute), false).Length > 0)
                    {
                        var target = new TargetScript(script, serializedObject);
                        targetList.Add(target);
                        break;
                    }
                }
            }

            targets = targetList.ToArray();
        }
    }
}
}
#endif