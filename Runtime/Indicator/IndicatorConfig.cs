using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IndicatorConfig", menuName = "Indicator/IndicatorConfig", order = 1)]
public class IndicatorConfig : ScriptableObject
{
    public bool IsEnabled = true;

    [Tooltip("Giay giu de tu dong bam khi indicator o che do Auto")]
    public float AutoClickTime = 0.35f;

    [Header("Indicators")]
    public List<IndicatorData> Indicators = new();


    private Dictionary<IndicatorType, IndicatorData> lookup;


    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<IndicatorType, IndicatorData>();

        foreach (var data in Indicators)
        {
            if (data == null)
                continue;

            lookup[data.Type] = data;
        }
    }


    public Indicator GetPrefab(IndicatorType type)
    {
        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(type, out var data) ? data.Prefab : null;
    }

    public bool CanShow(IndicatorType type)
    {
        if (!IsEnabled)
            return false;

        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(type, out var data) && data.Show;
    }
}


[Serializable]
public class IndicatorData
{
    public IndicatorType Type;
    public Indicator Prefab;
    public bool Show = true;
}