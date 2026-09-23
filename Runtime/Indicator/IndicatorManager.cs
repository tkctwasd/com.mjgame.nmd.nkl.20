using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager
{

    public static IndicatorController Controller { get; private set; }
    static List<Indicator> listIndicator { get; set; } = new();
    static IndicatorConfig config;
    public static Dictionary<IndicatorType, bool> ShowStates { get; private set; } = new();
    public static Dictionary<IndicatorType, List<IndicatorTarget>> TargetsByType { get; private set; } = new();

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        config = Resources.Load<IndicatorConfig>("IndicatorConfig");
        foreach (IndicatorType t in System.Enum.GetValues(typeof(IndicatorType)))
        {
            if (!ShowStates.ContainsKey(t)) ShowStates[t] = true;
        }
    }

    static void CheckInfo()
    {
        if (config == null || !config.IsEnabled) return;
        if (Controller == null)
        {
            //var canvas = PanelManager.CreateCanvas("Indicator Canvas", 1);
            //new GameObject(nameof(IndicatorController), typeof(RectTransform), typeof(IndicatorController))
            //    .TryGetComponent(out IndicatorController controller);
            //controller.transform.SetParent(canvas.transform, false);
            // Tim controller trong scene, ke ca khi dang tat
            Controller = Object.FindObjectOfType<IndicatorController>(true);
        }
    }

    public static void SetShowState(IndicatorType type, bool state)
    {
        ShowStates[type] = state;
        if (!state && TargetsByType.ContainsKey(type))
        {
            List<IndicatorTarget> targets = TargetsByType[type];
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].Indicator != null && targets[i].Indicator.gameObject.activeSelf)
                {
                    targets[i].Indicator.Activate(false);
                }
            }
        }
    }
    /// <summary>Giay giu de tu dong bam, lay tu IndicatorConfig.</summary>
    public static float AutoClickTime => config != null ? config.AutoClickTime : 0f;

    public static bool CanShow(IndicatorType type)
    {
        return ShowStates.ContainsKey(type) && ShowStates[type];
    }
    public static void Register(IndicatorTarget target)
    {
        CheckInfo();
        if (!TargetsByType.ContainsKey(target.indicatorType))
            TargetsByType[target.indicatorType] = new List<IndicatorTarget>();

        if (!TargetsByType[target.indicatorType].Contains(target))
            TargetsByType[target.indicatorType].Add(target);
    }

    public static void Unregister(IndicatorTarget target)
    {
        if (target.Indicator != null)
            target.Indicator.Activate(false);

        target.Indicator = null;

        if (TargetsByType.ContainsKey(target.indicatorType))
            TargetsByType[target.indicatorType].Remove(target);
    }

    public static Indicator GetIndicator(IndicatorType type)
    {
        for (int i = 0; i < listIndicator.Count; i++)
        {
            if (!listIndicator[i].Active && type == listIndicator[i].IndicatorType)
            {
                return listIndicator[i];
            }
        }
        var result = GameObject.Instantiate(config.GetPrefab(type), Controller.transform);
        listIndicator.Add(result);
        return result;
    }

    internal static void ClearIndicators()
    {
        Controller = null;
        listIndicator.Clear();
        TargetsByType.Clear();
    }

    public static void RefreshAllActive()
    {
        for (int i = 0; i < listIndicator.Count; i++)
        {
            if (listIndicator[i].Active)
            {
                listIndicator[i].Refresh();
            }
        }
    }
}
