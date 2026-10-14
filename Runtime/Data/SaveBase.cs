using System;
using UnityEngine;

/// <summary>
/// Luu/xoa moi DataModule (JSON vao PlayerPrefs, key "data_&lt;ten_snake&gt;"): Setting, Avatar, Rate cua package
/// va moi data game/mode khai bao ke thua DataModule&lt;T&gt;.
/// Game ke thua 1 class rong (gan len prefab) va them property cho data cua minh de goi DataSave.Instance.X:
/// <code>public class DataSave : SaveBase&lt;DataSave&gt; { public DataMap DataMap => DataMap.Current; }</code>
/// Can xu ly them thi dang ky event OnLoaded/OnSaved/OnNewGame/OnDeleted.
/// </summary>
public abstract class SaveBase<T> : Singleton<T> where T : MonoBehaviour
{
    // Data cua package
    public DataSetting DataSetting => DataSetting.Current;

    public static event Action OnLoaded;
    public static event Action OnSaved;
    public static event Action OnNewGame;
    public static event Action OnDeleted;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    #region Save/Load

    public void SaveGame()
    {
        OnSaved?.Invoke();
        DataModules.SaveAll();

        Debug.Log("<color=orange>[SaveSystem]</color> All data saved.");
    }

    public void LoadGame()
    {
        // DataModule tu load lan dau khi truy cap; nap san Setting vi Audio/Camera doc ngay khi khoi dong
        _ = global::DataSetting.Current;
        OnLoaded?.Invoke();

        Debug.Log("<color=cyan>[SaveSystem]</color> All data loaded.");
    }

    public void NewGame()
    {
        DataModules.DeleteAll();
        _ = global::DataSetting.Current;
        OnNewGame?.Invoke();

        SaveGame();
        Debug.Log("<color=orange>[SaveSystem]</color> New game started.");
    }

    public void DeleteSave()
    {
        OnDeleted?.Invoke();
        DataModules.DeleteAll();

        Debug.Log("<color=red>[SaveSystem]</color> All data deleted.");
    }

    #endregion

    #region Unity Events

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) SaveGame();
    }

    #endregion
}
