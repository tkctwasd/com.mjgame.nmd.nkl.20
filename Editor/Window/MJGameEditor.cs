using UnityEditor;
using UnityEngine;

namespace MJGame
{
    public class MJGameEditor : EditorWindow
    {
        [MenuItem("MJGame/MJGameEditor %m")]
        public static void ShowWindow()
        {
            MJGameEditor window = GetWindow<MJGameEditor>("MJGame Editor");
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
            BaseUIWindowsEditor.Init();
        }

        private void RemoveMissingScripts()
        {
            RemoveMissingScriptsEditor.FindAndRemoveMissingInSelected();
        }
        
    }
}
