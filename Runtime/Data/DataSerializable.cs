using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

// Data chung moi game deu co. Data rieng cua game o DataSerializable ben game.

#region Setting
[Serializable]
public class DataSetting
{
    /// <summary>Ban setting dang dung. SaveBase gan lai moi lan load hoac tao game moi.</summary>
    public static DataSetting Current { get; internal set; } = new DataSetting();

    [JsonProperty("Sensivity")] public float Sensitivity;
    public bool Music;
    public bool Sound;
    public bool Vibrate;
    public float Distance;

    public DataSetting()
    {
        Music = true;
        Sound = true;
        Vibrate = true;
        Sensitivity = 0.5f;
        Distance = 6.5f;
    }

    public void ResetData()
    {
        Music = true;
        Sound = true;
        Vibrate = true;
        Sensitivity = 0.5f;
        Distance = 6.5f;
    }
}
#endregion

#region  Avatar
[Serializable]
public class DataAvatar
{
    public int SkinCurrent;

    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public List<bool> ListSkinUnlocked;

    public const int MAX_SKIN = 10;

    public void SetUnlockSkin(int idSkin)
    {
        ListSkinUnlocked[idSkin] = true;
    }

    public void ResetData()
    {
        SkinCurrent = 0;
        ListSkinUnlocked = new List<bool>() { true };
    }

    [JsonConstructor]
    public DataAvatar()
    {
        SkinCurrent = 0;
        ListSkinUnlocked = new List<bool>();
    }

    public DataAvatar(bool init)
    {
        SkinCurrent = 0;
        ListSkinUnlocked = new List<bool>() { true, false, false, false, false, false, false, false, false, false };
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        while (ListSkinUnlocked.Count < MAX_SKIN)
        {
            ListSkinUnlocked.Add(false);
        }
    }
}
#endregion

#region Rate
[Serializable]
public class DataRate
{
    public bool IsRate;
    public int NumberShowRate;
    public float LastShowTime = 0f;

    public DataRate()
    {
        IsRate = false;
        NumberShowRate = 0;
        LastShowTime = 0;
    }

}
#endregion