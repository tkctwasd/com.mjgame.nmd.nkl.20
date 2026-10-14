using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "IndicatorConfig", menuName = "Indicator/IndicatorConfig", order = 1)]
public class IndicatorConfig : ScriptableObject
{
    public bool IsEnabled = true;

    [Tooltip("Giay giu de tu dong bam khi indicator o che do Auto")]
    public float AutoClickTime = 0.35f;

    [Header("Indicators")]
    public List<IndicatorData> Indicators = new();


    private Dictionary<int, IndicatorData> lookup;


    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<int, IndicatorData>();

        foreach (var data in Indicators)
        {
            if (data == null || data.Type == 0)
                continue;

            lookup[data.Type] = data;
        }
    }


    public Indicator GetPrefab(int type)
    {
        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(type, out var data) ? data.Prefab : null;
    }

    public bool CanShow(int type)
    {
        if (!IsEnabled || type == 0)
            return false;

        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(type, out var data) && data.Show;
    }
}


[Serializable]
public class IndicatorData
{
    [IndicatorType] public int Type;
    public Indicator Prefab;
    public bool Show = true;
}