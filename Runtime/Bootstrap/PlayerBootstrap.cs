using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerBootstrap
{

    // Dictionary dùng để lưu trữ mọi kiểu dữ liệu đã đăng ký
    private static readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

    /// <summary>
    /// Đăng ký một instance theo kiểu T
    /// </summary>
    public static void Register<T>(T instance) where T : class
    {
        var type = typeof(T);
        if (registry.ContainsKey(type))
        {
            Debug.LogWarning($"<color=yellow>[PlayerBootstrap]</color> <color=white> Đã có sẵn một instance của {type.Name}</color>!");
            return;
        }

        Debug.Log($"<color=yellow>[PlayerBootstrap]</color> <color=white> Đã đăng kí instance của {type.Name} thành công!</color>");
        registry[type] = instance;
    }

    /// <summary>
    /// Huỷ đăng ký một kiểu T
    /// </summary>
    public static void Unregister<T>() where T : class
    {
        var type = typeof(T);
        if (registry.ContainsKey(type))
        {
            Debug.Log($"<color=yellow>[PlayerBootstrap]</color> <color=white>Huỷ đăng ký {type.Name}</color>.");
            registry.Remove(type);
        }
    }

    /// <summary>
    /// Lấy ra instance đã đăng ký theo kiểu T
    /// </summary>
    /// 
    public static T Get<T>() where T : class
    {
        var type = typeof(T);
        if (registry.TryGetValue(type, out var instance))
        {
            return instance as T;
        }

        Debug.LogWarning($"<color=yellow>[PlayerBootstrap]</color><color=white> Không có {type.Name} trong registry. </color> ");
        return null;
    }

    /// <summary>
    /// Unregister tat ca cac Component
    /// </summary>
    public static void UnregisterAll()
    {
        registry.Clear();
        Debug.Log("<color=yellow>Clear All Register</color>");
    }

    /// <summary>
    /// Kiểm tra xem kiểu T đã đăng ký chưa
    /// </summary>
    public static bool Has<T>() where T : class
    {
        return registry.ContainsKey(typeof(T));
    }


}
