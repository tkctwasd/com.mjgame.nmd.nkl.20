#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;


namespace MJGame
{
    public class PoolConfigWindowsEditor : EditorWindow
    {
        private const string DefaultResourcesFolder = "Assets/Resources";
        private const string DefaultPoolConfigPath = "Assets/Resources/PoolConfig.asset";
        private const string PoolConfigType = "t:PoolConfigSO";
        private PoolConfigSO poolConfig;
        private Vector2 scrollPosition;

        public static void ShowWindow()
        {
            var window = GetWindow<PoolConfigWindowsEditor>("Pool Config");
            window.poolConfig = FindOrCreatePoolConfig();
            window.Show();
        }

        private static PoolConfigSO FindOrCreatePoolConfig()
        {
            string[] guids = AssetDatabase.FindAssets(PoolConfigType, new[] { "Assets" });
            PoolConfigSO poolConfig = null;

            if (guids.Length > 1)
            {
                Debug.LogError($"You have multiple PoolConfigSO files in your project. Please ensure there is only one file and place it in the \n{"Assets/Resources"} folder.");
            }

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                poolConfig = AssetDatabase.LoadAssetAtPath<PoolConfigSO>(path);
                Debug.Log($"PoolConfig found at: {path}");
                Selection.activeObject = poolConfig;
                EditorGUIUtility.PingObject(poolConfig);
                return poolConfig;
            }

            return CreatePoolConfig();
        }

        private static PoolConfigSO CreatePoolConfig()
        {
            if (!Directory.Exists(DefaultResourcesFolder))
            {
                Directory.CreateDirectory(DefaultResourcesFolder);
                AssetDatabase.Refresh();
            }

            PoolConfigSO poolConfig = ScriptableObject.CreateInstance<PoolConfigSO>();
            AssetDatabase.CreateAsset(poolConfig, DefaultPoolConfigPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"PoolConfig created at: {DefaultPoolConfigPath}");
            return poolConfig;
        }

        private void OnGUI()
        {
            if (poolConfig == null)
            {
                EditorGUILayout.HelpBox("No PoolConfigSO found or created. Please restart the editor.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Pool Config Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Scroll view for large lists
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            SerializedObject serializedConfig = new SerializedObject(poolConfig);
            SerializedProperty poolItemsProperty = serializedConfig.FindProperty("poolItems");

            // Display list of pool items
            EditorGUILayout.PropertyField(poolItemsProperty, new GUIContent("Pool Items"), true);

            serializedConfig.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

        }
    }
}
#endif