using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data rieng cua 1 tang/mode, tu luu bang key rieng "data_&lt;TenClass&gt;". Dat file data ngay trong folder tinh nang.
/// Khai bao: <code>[Serializable] public class DataAnimalPen : DataModule&lt;DataAnimalPen&gt; { }</code>
/// Doc/ghi: <c>DataAnimalPen.Current</c> (load lan dau khi truy cap, DataSave tu luu khi SaveGame).
/// </summary>
[Serializable]
public abstract class DataModule<T> : IDataModule where T : DataModule<T>, new()
{
    private static T current;

    public static T Current
    {
        get
        {
            if (current != null) return current;

            current = JsonDataUtility.LoadData<T>(DataModules.KeyOf(typeof(T)));
            if (current == null)
            {
                current = new T();
                current.OnNew();
            }
            current.OnLoaded();
            DataModules.Track(current);
            return current;
        }
    }

    protected virtual void OnNew() { }
    protected virtual void OnLoaded() { }

    void IDataModule.Save() => JsonDataUtility.SaveData(DataModules.KeyOf(typeof(T)), (T)this);

    void IDataModule.Unload() => current = null;
}

public interface IDataModule
{
    void Save();
    void Unload();
}

public static class DataModules
{
    private static readonly List<IDataModule> loaded = new();

    public static string KeyOf(Type type) => "data_" + type.Name;

    internal static void Track(IDataModule module)
    {
        if (!loaded.Contains(module)) loaded.Add(module);
    }

    public static void SaveAll()
    {
        foreach (var module in loaded) module.Save();
    }

    public static void DeleteAll()
    {
        foreach (var module in loaded) module.Unload();
        loaded.Clear();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try { types = assembly.GetTypes(); }
            catch (System.Reflection.ReflectionTypeLoadException e) { types = Array.FindAll(e.Types, t => t != null); }

            foreach (var type in types)
            {
                if (type.IsAbstract || !IsDataModule(type)) continue;
                JsonDataUtility.DeleteData(KeyOf(type));
            }
        }
        Debug.Log("<color=red>[SaveSystem]</color> DataModules deleted.");
    }

    private static bool IsDataModule(Type type)
    {
        for (var t = type.BaseType; t != null; t = t.BaseType)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(DataModule<>)) return true;
        }
        return false;
    }
}
