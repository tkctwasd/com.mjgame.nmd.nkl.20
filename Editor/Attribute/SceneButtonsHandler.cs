#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEditor;
using UnityEngine;
namespace MJGame {

public class SceneButtonsHandler
{
    [InitializeOnLoadMethod]
    private static void Init()
    {
        new SceneButtonsHandler();
    }

    private TargetScript[] m_Targets;
    private TargetsProvider m_TargetsProvider;

    private SceneButtonsHandler()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        TrackerUtility.OnAllScriptsChanged += OnAllScriptsChanged;
        TrackerUtility.OnMarkedScriptsChanged += OnMarkedScriptsChanged;
    }

    private void OnAllScriptsChanged()
    {
        RefreshTargets(TrackerUtility.allScripts);
    }


    private void OnMarkedScriptsChanged()
    {
        RefreshTargets(TrackerUtility.markedScripts);
    }

    private void RefreshTargets(MonoBehaviour[] scripts)
    {
        if (m_TargetsProvider)
        {
            m_TargetsProvider.Dispose();
        }

        m_TargetsProvider = new TargetsProvider(scripts);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (m_TargetsProvider && m_TargetsProvider.isDone)
        {
            m_Targets = m_TargetsProvider.results;
            m_TargetsProvider.Dispose();
        }

        if (m_Targets == null) return;

        DrawButtons();
    }

    private void DrawButtons()
    {
        Handles.BeginGUI();
        for (int i = 0; i < m_Targets.Length; i++)
        {
            var target = m_Targets[i];
            if (target == null) continue;
            target.DrawButtons(Styles.buttonContent, Styles.buttonStyle);
        }
        Handles.EndGUI();
    }

    private class TargetScript
    {
        private MonoBehaviour m_MonoBehaviour;
        private GameObject m_GameObject;
        private GameObject gameObject
        {
            get
            {
                if (m_GameObject == null) m_GameObject = m_MonoBehaviour.gameObject;
                return m_GameObject;
            }
        }
        private Transform m_Transform;
        private Transform transform
        {
            get
            {
                if (m_Transform == null) m_Transform = m_MonoBehaviour.transform;
                return m_Transform;
            }
        }
        private Button[] m_Buttons;

        public TargetScript(MonoBehaviour monoBehaviour, Button[] buttons)
        {
            m_MonoBehaviour = monoBehaviour;
            m_Buttons = buttons;
        }

        public void DrawButtons(GUIContent content, GUIStyle style)
        {
            if (m_MonoBehaviour == null) return;

            float buttonWidth = CalcWidth(content, style);

            Rect rect = new Rect(HandleUtility.WorldToGUIPoint(transform.position), new Vector2(buttonWidth + 5, 20));
            rect.x -= buttonWidth / 2;

            if (Selection.Contains(gameObject)) rect.y += 23;

            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].Draw(rect, content, style);
                rect.y += 21;
            }
        }

        private float CalcWidth(GUIContent content, GUIStyle style)
        {
            float maxWidth = 0;
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                float width = m_Buttons[i].CalcWidth(content, style);

                if (width > maxWidth)
                {
                    maxWidth = width;
                }
            }
            return maxWidth;
        }
    }
    private class TargetsProvider
    {
        private MonoBehaviour[] m_Scripts;
        private Thread m_Thread;
        public TargetScript[] results;
        public bool isDone;
        public bool m_IsDisposed;

        public TargetsProvider(MonoBehaviour[] scripts)
        {
            m_Scripts = scripts;
            m_Thread = new Thread(FindTargets);
            m_Thread.Start();
        }

        private void FindTargets()
        {
            Thread.Sleep(500);

            if (m_IsDisposed) return;

            List<TargetScript> targetList = new List<TargetScript>();
            List<Button> buttonList = new List<Button>();

            BindingFlags flags =
                BindingFlags.InvokeMethod |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance;

            for (int i = 0; i < m_Scripts.Length; i++)
            {
                if (m_IsDisposed) return;

                var script = m_Scripts[i];
                var type = script.GetType();
                var methods = type.GetMethods(flags);
                buttonList.Clear();

                for (int j = 0; j < methods.Length; j++)
                {
                    if (m_IsDisposed) return;

                    MethodInfo method = methods[j];
                    var attributes = method.GetCustomAttributes(false);

                    for (int w = 0; w < attributes.Length; w++)
                    {
                        if (m_IsDisposed) return;

                        var attribute = attributes[w];
                        var attributeType = attribute.GetType(); ;
                        if (attributeType == typeof(SceneButtonAttribute))
                        {
                            var button = new Button(script, method, (SceneButtonAttribute)attribute);
                            buttonList.Add(button);
                        }
                    }
                }

                if (buttonList.Count > 0)
                {
                    var target = new TargetScript(m_Scripts[i], buttonList.ToArray());
                    targetList.Add(target);
                }
            }

            results = targetList.ToArray();
            isDone = true;
        }

        public void Dispose()
        {
            m_IsDisposed = true;
            m_Thread.Abort();
        }

        public static implicit operator bool(TargetsProvider obj)
        {
            if (obj == null) return false;
            return !obj.m_IsDisposed;
        }
    }
    private class Button
    {
        private MonoBehaviour m_MonoBehaviour;
        private MethodInfo m_MethodInfo;
        private string m_Text;

        public Button(MonoBehaviour monoBehaviour, MethodInfo methodInfo, SceneButtonAttribute attribute)
        {
            m_MonoBehaviour = monoBehaviour;
            m_MethodInfo = methodInfo;
            m_Text = (attribute.text == null) ? methodInfo.Name : attribute.text;
        }

        public void Draw(Rect rect, GUIContent content, GUIStyle style)
        {
            content.text = m_Text;
            if (GUI.Button(rect, content, style))
            {
                m_MethodInfo.Invoke(m_MonoBehaviour, null);
            }
        }

        public float CalcWidth(GUIContent content, GUIStyle style)
        {
            content.text = m_Text;
            return style.CalcSize(content).x;
        }
    }
    private static class Styles
    {
        public static GUIStyle buttonStyle = GUI.skin.button;
        public static GUIContent buttonContent = new GUIContent();
    }
}
}
#endif