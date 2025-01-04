#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

namespace MJGame
{

    public class BaseUIWindowsEditor : EditorWindow
    {
        private int selectedResolution = 0;

        private string[] resolutions = new string[] { "1080 x 1920", "1920 x 1080" };
        private Vector2Int[] vectorResolution = new Vector2Int[] { new Vector2Int(1080, 1920), new Vector2Int(1920, 1080) };

        public static void ShowWindow()
        {
            BaseUIWindowsEditor window = (BaseUIWindowsEditor)EditorWindow.GetWindow(typeof(BaseUIWindowsEditor));
            window.panelConfig = window.FindOrCreatePanelConfig();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Choose Resolution", EditorStyles.boldLabel);

            selectedResolution = EditorGUILayout.Popup("Resolution", selectedResolution, resolutions);

            if (GUILayout.Button("Create PanelUI"))
            {
                CreateUIBase.CreatePanelUI(vectorResolution[selectedResolution]);
            }

            if (GUILayout.Button("Create PanelGamePlay"))
            {
                CreateUIBase.CreatePanelGamePlay(vectorResolution[selectedResolution]);
            }

            if (GUILayout.Button("Create PanelLose"))
            {
                CreateUIBase.CreatePanelLose(vectorResolution[selectedResolution]);
            }


            if (panelConfig == null)
            {
                EditorGUILayout.HelpBox("No PanelConfigSO found or created. Please restart the editor.", MessageType.Warning);
                return;
            }



            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Panel Config Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Scroll view for large lists
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            SerializedObject serializedConfig = new SerializedObject(panelConfig);
            SerializedProperty panel = serializedConfig.FindProperty("Panels");

            // Display list of pool items
            EditorGUILayout.PropertyField(panel, new GUIContent("Panels"), true);

            serializedConfig.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
        }


        private const string DefaultResourcesFolder = "Assets/Resources";
        private const string DefaultPanelConfigPath = "Assets/Resources/PanelConfig.asset";
        private const string PanelConfigType = "t:PanelConfigSO";
        private PanelConfigSO panelConfig;
        private Vector2 scrollPosition;

        private PanelConfigSO FindOrCreatePanelConfig()
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

        private PanelConfigSO CreatePanelConfig()
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


    }

}
#endif