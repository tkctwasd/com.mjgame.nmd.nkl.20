using System;
using System.Collections.Generic;
using UnityEngine;


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

    private static readonly Dictionary<Type, string> dictKey = new();

    public static string KeyOf(Type type)
    {
        if (dictKey.TryGetValue(type, out var key)) return key;

        string name = type.Name;
        if (name.Length > 4 && name.StartsWith("Data") && char.IsUpper(name[4])) name = name.Substring(4);
        key = "data_" + ToSnakeCase(name);
        dictKey[type] = key;
        return key;
    }

    private static string ToSnakeCase(string name)
    {
        var sb = new System.Text.StringBuilder(name.Length + 8);
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if (char.IsUpper(c))
            {
                bool prevLower = i > 0 && (char.IsLower(name[i - 1]) || char.IsDigit(name[i - 1]));
                bool acronymEnd = i > 0 && char.IsUpper(name[i - 1]) && i + 1 < name.Length && char.IsLower(name[i + 1]);
                if (prevLower || acronymEnd) sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else sb.Append(c);
        }
        return sb.ToString();
    }

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
