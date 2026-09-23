using UnityEngine;

/// <summary>
/// Phan luu chung moi game deu co: Setting, Avatar, Rate, Monetization (JSON vao PlayerPrefs).
/// Game ke thua va them data cua minh:
/// <code>public class DataSave : SaveBase&lt;DataSave&gt;</code>
/// roi override cac ham OnLoadGame/OnSaveGame/OnNewGame/OnDeleteSave.
/// </summary>
public abstract class SaveBase<T> : Singleton<T> where T : MonoBehaviour
{
    public DataSetting DataSetting { get; private set; }
    public DataAvatar DataAvatar { get; private set; }
    public DataRate DataRate { get; private set; }
    public DataMonetization DataMonetization { get; private set; }

    // Keys
    private const string KEY_AVATAR = "data_avatar_fls";
    private const string KEY_SETTING = "data_setting_fls";
    private const string KEY_RATE = "data_rate_fls";
    private const string KEY_MONETIZATION = "data_monetization_fls";

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    #region Save/Load

    public void SaveGame()
    {
        OnSaveGame();
        JsonDataUtility.SaveData(KEY_AVATAR, DataAvatar);
        JsonDataUtility.SaveData(KEY_SETTING, DataSetting);
        JsonDataUtility.SaveData(KEY_RATE, DataRate);
        JsonDataUtility.SaveData(KEY_MONETIZATION, DataMonetization);

        Debug.Log("<color=orange>[SaveSystem]</color> All data saved.");
    }

    public void LoadGame()
    {
        OnLoadGame();
        DataAvatar = JsonDataUtility.LoadData<DataAvatar>(KEY_AVATAR) ?? new DataAvatar(true);
        DataSetting = JsonDataUtility.LoadData<DataSetting>(KEY_SETTING) ?? new DataSetting();
        DataRate = JsonDataUtility.LoadData<DataRate>(KEY_RATE) ?? new DataRate();
        DataMonetization = JsonDataUtility.LoadData<DataMonetization>(KEY_MONETIZATION) ?? new DataMonetization();

        global::DataSetting.Current = DataSetting;

        Debug.Log("<color=cyan>[SaveSystem]</color> All data loaded.");
    }

    public void NewGame()
    {
        OnNewGame();
        DataAvatar = new DataAvatar(true);
        DataSetting = new DataSetting();
        DataRate = new DataRate();
        DataMonetization = new DataMonetization();

        global::DataSetting.Current = DataSetting;

        SaveGame();
        Debug.Log("<color=orange>[SaveSystem]</color> New game started.");
    }

    public void DeleteSave()
    {
        OnDeleteSave();
        JsonDataUtility.DeleteData(KEY_AVATAR);
        JsonDataUtility.DeleteData(KEY_SETTING);
        JsonDataUtility.DeleteData(KEY_RATE);
        JsonDataUtility.DeleteData(KEY_MONETIZATION);

        Debug.Log("<color=red>[SaveSystem]</color> All data deleted.");
    }

    #endregion

    #region Phan rieng cua game

    protected virtual void OnLoadGame() { }
    protected virtual void OnSaveGame() { }
    protected virtual void OnNewGame() { }
    protected virtual void OnDeleteSave() { }

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
