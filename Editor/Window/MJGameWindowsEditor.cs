#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MJGame
{
    public class MJGameWindowsEditor : EditorWindow
    {
        [MenuItem("MJGame/MJGameEditor %m")]
        public static void ShowWindow()
        {
            MJGameWindowsEditor window = GetWindow<MJGameWindowsEditor>("MJGame Editor");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("MJGame Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();


            if (GUILayout.Button("UI"))
            {
                DrawUIPanelTab();
            }
            if (GUILayout.Button("Pool"))
            {
                DrawPoolConfigTab();
            }
            if (GUILayout.Button("Audio"))
            {
                DrawAudioConfigTab();
            }
            if (GUILayout.Button("RemoveMissingScripts"))
            {
                RemoveMissingScripts();
            }
            EditorGUILayout.Space();

        }

        private void DrawPoolConfigTab()
        {
            PoolConfigWindowsEditor.ShowWindow();
        }

        private void DrawUIPanelTab()
        {
            BaseUIWindowsEditor.ShowWindow();
        }
        private void DrawAudioConfigTab()
        {
            AudioConfigWindowsEditor.ShowWindow();
        }

        private void RemoveMissingScripts()
        {
            RemoveMissingScriptsEditor.FindAndRemoveMissingInSelected();
        }

    }
}

#endif