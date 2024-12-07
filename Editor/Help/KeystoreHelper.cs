//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System;
//using System.Text;
//using System.Reflection;

//#if UNITY_EDITOR

//[InitializeOnLoad]
//public class AndroidKeystoreLoader
//{
//    static AndroidKeystoreLoader()
//    {
//#if UNITY_ANDROID
//        if(EditorPrefs.GetBool("KeystoreHelper"))
//        {
//            string keystorePass, keyaliasName, keyaliasPass;

//            keystorePass = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYSTOREPASS, "wasdmobile.com");
//            keyaliasName = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYALIASNAME, PlayerSettings.applicationIdentifier);
//            keyaliasPass = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYALIASPASS, "wasdmobile.com");

//            PlayerSettings.Android.keystorePass = keystorePass;
//            PlayerSettings.Android.keyaliasName = keyaliasName;
//            PlayerSettings.Android.keyaliasPass = keyaliasPass;
//        }
//#endif
//    }
//}

//#endif
//class KeystoreHelper : EditorWindow
//{
//    public const string KEYSTOREPASS = "KeystorePass_";
//    public const string KEYALIASNAME = "KeyAliasName_";
//    public const string KEYALIASPASS = "KeyAliasPass_";

//    private string keystorePass = "";
//    private string keyAliasName = "";
//    private string keyAliasPass = "";

//    [MenuItem("Wasd/Tools/Keystore Helper")]
//    public static void ShowWindow()
//    {
//        var window = EditorWindow.GetWindow(typeof(KeystoreHelper));
//        window.titleContent = new GUIContent("Keystore");
//        window.minSize = new Vector2(400, 150);
//        window.maxSize = new Vector2(400, 150);
//    }
//    [MenuItem("Wasd/Tools/Keystore Helper", true)]
//    public static bool ValidateShowWindow()
//    {
//#if UNITY_IOS
//        return false;        
//#endif
//        return true;
//    }
//    void OnEnable()
//    {
//        keystorePass = ReadPrefs(KEYSTOREPASS, "wasdmobile.com");
//        keyAliasName = ReadPrefs(KEYALIASNAME, PlayerSettings.applicationIdentifier);
//        keyAliasPass = ReadPrefs(KEYALIASPASS, "wasdmobile.com");
//        PlayerSettings.Android.keystorePass = keystorePass;
//        PlayerSettings.Android.keyaliasName = keyAliasName;
//        PlayerSettings.Android.keyaliasPass = keyAliasPass;
//    }

//    void OnDisable()
//    {
//        WritePrefs(KEYSTOREPASS, keystorePass);
//        WritePrefs(KEYALIASNAME, keyAliasName);
//        WritePrefs(KEYALIASPASS, keyAliasPass);

//        PlayerSettings.Android.keystorePass = keystorePass;
//        PlayerSettings.Android.keyaliasName = keyAliasName;
//        PlayerSettings.Android.keyaliasPass = keyAliasPass;
//    }

//    public static string ReadPrefs(string _key, string _value)
//    {
//        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
//        string key = _key + Md5Sum(codeBase);
//        if(!EditorPrefs.HasKey(key) || string.IsNullOrEmpty(EditorPrefs.GetString(key)))
//        {
//            EditorPrefs.SetString(key, _value);
//        }
//        return EditorPrefs.GetString(key, _value);
//    }

//    public static void WritePrefs(string _key, string value)
//    {
//        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
//        string key = _key + Md5Sum(codeBase);
//        EditorPrefs.SetString(key, value);
//    }

//    void OnGUI()
//    {
//        GUILayout.Label("Android Keystore", EditorStyles.boldLabel);
//        keystorePass = EditorGUILayout.PasswordField("Keystore Password", keystorePass);
//        keyAliasName = EditorGUILayout.TextField("Key Alias Title", keyAliasName);
//        keyAliasPass = EditorGUILayout.PasswordField("Key Alias Password", keyAliasPass);
//        if(GUILayout.Button("Save"))
//        {
//            WritePrefs(KEYSTOREPASS, keystorePass);
//            WritePrefs(KEYALIASNAME, keyAliasName);
//            WritePrefs(KEYALIASPASS, keyAliasPass);
//            EditorPrefs.SetBool("KeystoreHelper", true);
//        }
//    }

//    private static string Md5Sum(string strToEncrypt)
//    {
//        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
//        byte[] bytes = ue.GetBytes(strToEncrypt);

//        // encrypt bytes
//        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
//        byte[] hashBytes = md5.ComputeHash(bytes);

//        // Convert the encrypted bytes back to a string (base 16)
//        string hashString = "";

//        for(int i = 0; i < hashBytes.Length; i++)
//        {
//            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
//        }

//        return hashString.PadLeft(32, '0');
//    }
//}