#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
namespace MJGame
{

    public class PanelConfigWindowsEditor : EditorWindow
    {
        private const string DefaultResourcesFolder = "Assets/Resources";
        private const string DefaultPanelConfigPath = "Assets/Resources/PanelConfig.asset";
        private const string PanelConfigType = "t:PanelConfigSO";

        private PanelConfigSO panelConfig;
        private Vector2 scrollPosition;

        public static void ShowWindow()
        {
            var window = GetWindow<PanelConfigWindowsEditor>("Panel Config");
            window.panelConfig = FindOrCreatePanelConfig();
            window.Show();
        }

        private static PanelConfigSO FindOrCreatePanelConfig()
        {
            string[] guids = AssetDatabase.FindAssets(PanelConfigType, new[] { "Assets" });
            PanelConfigSO panelConfig = null;

            if (guids.Length > 1)
            {
                Debug.LogError("You have multiple PanelConfigSO files in your project. Please ensure there is only one file and place it in the 'Assets/Resources' folder.");
            }

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                panelConfig = AssetDatabase.LoadAssetAtPath<PanelConfigSO>(path);
                Debug.Log($"PanelConfig found at: {path}");
                Selection.activeObject = panelConfig;
                EditorGUIUtility.PingObject(panelConfig);
                return panelConfig;
            }

            return CreatePanelConfig();
        }

        private static PanelConfigSO CreatePanelConfig()
        {
            if (!Directory.Exists(DefaultResourcesFolder))
            {
                Directory.CreateDirectory(DefaultResourcesFolder);
                AssetDatabase.Refresh();
            }

            PanelConfigSO panelConfig = ScriptableObject.CreateInstance<PanelConfigSO>();
            AssetDatabase.CreateAsset(panelConfig, DefaultPanelConfigPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"PanelConfig created at: {DefaultPanelConfigPath}");
            return panelConfig;
        }

        private void OnGUI()
        {
            if (panelConfig == null)
            {
                EditorGUILayout.HelpBox("No PanelConfigSO found or created. Please restart the editor.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Panel Config Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Scroll view for large lists
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            SerializedObject serializedConfig = new SerializedObject(panelConfig);
            SerializedProperty panel = serializedConfig.FindProperty("panel");

            // Display list of pool items
            EditorGUILayout.PropertyField(panel, new GUIContent("Panel"), true);

            serializedConfig.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
        }
    }
}

#endif