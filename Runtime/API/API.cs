using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API
{
    static List<IService> services = new List<IService>();
    static JObject info = null;
    public static JObject Info
    {
        get
        {
            if(info == null)
            {
                var txtInfo = Resources.Load<TextAsset>("Info".ToPlatformName());
                if(txtInfo != null)
                {
                    info = JObject.Parse(txtInfo.text);
                }
            }
            if(info == null)
            {
                Debug.LogError("Info not found");
            }
            return info;
        }
        private set
        {
            if(value != null)
            {
                info = value;
            }
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        services = new List<IService>();
        AddService(new ServiceTracking());
        AddService(new ServiceRemoteConfig());
        AddService(new ServiceLogEvent());
        AddService(new ServiceAds());
        Debug.Log("Init API");
        System.Threading.Thread.Sleep(250);
        foreach(var item in services)
        {
            item.Initialize();
        }
        Debug.Log("Initialize API");
    }
    public static void StartCoroutine(IEnumerator routine)
    {
        TickUpdateManager.Instance.StartCoroutine(routine);
    }
    static void AddService<T>(T service) where T : IService
    {
        if(services.Contains(service) == false)
            services.Add(service);
    }
    public static T Get<T>() where T : IService
    {
        for(int i = 0; i < services.Count; i++)
        {
            if(services[i] is T)
            {
                return (T)services[i];
            }
        }
        return default;
    }
}


public static class Ex
{
    public static string ToPlatformName(this string name)
    {
#if UNITY_ANDROID
        return "Android_" + name;
#elif UNITY_IOS
        return "iOS_" + name;
#else
        return name;
#endif
    }
}
