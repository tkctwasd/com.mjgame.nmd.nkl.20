using System;
using Newtonsoft.Json;
using UnityEngine;

// Data ma chinh package doc (Audio, Camera, Vibration). Data khac cua game khai bao DataModule ben game.

#region Setting
[Serializable]
public class DataSetting : DataModule<DataSetting>
{
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
