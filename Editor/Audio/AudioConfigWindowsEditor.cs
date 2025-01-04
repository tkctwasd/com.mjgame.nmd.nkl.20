#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace MJGame
{

    public class AudioConfigWindowsEditor : EditorWindow
    {
        private const string DefaultResourcesFolder = "Assets/Resources";
        private const string DefaultAudioConfigPath = "Assets/Resources/AudioConfig.asset";
        private const string AudioConfigType = "t:AudioConfigSO";

        private AudioConfigSO audioConfig;
        private Vector2 scrollPosition;

        public static void ShowWindow()
        {
            var window = GetWindow<AudioConfigWindowsEditor>("Audio Config");
            window.audioConfig = FindOrCreateAudioConfig(); 
            window.Show();
        }

        private static AudioConfigSO FindOrCreateAudioConfig()
        {
            string[] guids = AssetDatabase.FindAssets(AudioConfigType, new[] { "Assets" });
            AudioConfigSO audioConfig = null;

            if (guids.Length > 1)
            {
                Debug.LogError("You have multiple AudioConfigSO files in your project. Please ensure there is only one file and place it in the 'Assets/Resources' folder.");
            }

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                audioConfig = AssetDatabase.LoadAssetAtPath<AudioConfigSO>(path);
                Debug.Log($"AudioConfig found at: {path}");
                Selection.activeObject = audioConfig;
                EditorGUIUtility.PingObject(audioConfig);
                return audioConfig;
            }

            return CreateAudioConfig();
        }

        private static AudioConfigSO CreateAudioConfig()
        {
            if (!Directory.Exists(DefaultResourcesFolder))
            {
                Directory.CreateDirectory(DefaultResourcesFolder);
                AssetDatabase.Refresh();
            }

            AudioConfigSO audioConfig = ScriptableObject.CreateInstance<AudioConfigSO>();
            AssetDatabase.CreateAsset(audioConfig, DefaultAudioConfigPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"AudioConfig created at: {DefaultAudioConfigPath}");
            return audioConfig;
        }

        private void OnGUI()
        {
            if (audioConfig == null)
            {
                EditorGUILayout.HelpBox("No AudioConfigSO found or created. Please restart the editor.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Audio Config Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            SerializedObject serializedConfig = new SerializedObject(audioConfig);
            SerializedProperty bgClips = serializedConfig.FindProperty("bgClips");
            SerializedProperty vfxClips = serializedConfig.FindProperty("vfxClips");
            SerializedProperty sfxClips = serializedConfig.FindProperty("sfxClips");

            EditorGUILayout.PropertyField(bgClips, new GUIContent("Background Clips"), true);
            EditorGUILayout.PropertyField(vfxClips, new GUIContent("VFX Clips"), true);
            EditorGUILayout.PropertyField(sfxClips, new GUIContent("SFX Clips"), true);

            serializedConfig.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
        }
    }

}
#endif