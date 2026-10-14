using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager
{

    public static IndicatorController Controller { get; private set; }
    static List<Indicator> listIndicator { get; set; } = new();
    static IndicatorConfig config;
    public static Dictionary<int, bool> ShowStates { get; private set; } = new();
    public static Dictionary<int, List<IndicatorTarget>> TargetsByType { get; private set; } = new();

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        config = Resources.Load<IndicatorConfig>("IndicatorConfig");
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

    public static void SetShowState(int type, bool state)
    {
        if (type == 0) return;
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

    /// <summary>Loai chua tung SetShowState thi mac dinh duoc hien.</summary>
    public static bool CanShow(int type)
    {
        if (type == 0) return false;
        return !ShowStates.TryGetValue(type, out bool state) || state;
    }
    public static void Register(IndicatorTarget target)
    {
        CheckInfo();
        var type = target.Type;
        if (type == 0) return;
        if (!TargetsByType.TryGetValue(type, out var targets))
            TargetsByType[type] = targets = new List<IndicatorTarget>();

        if (!targets.Contains(target))
            targets.Add(target);
    }

    public static void Unregister(IndicatorTarget target)
    {
        if (target.Indicator != null)
            target.Indicator.Activate(false);

        target.Indicator = null;

        if (target.Type != 0 && TargetsByType.TryGetValue(target.Type, out var targets))
            targets.Remove(target);
    }

    public static Indicator GetIndicator(int type)
    {
        var prefab = config.GetPrefab(type);
        if (prefab == null) return null;
        for (int i = 0; i < listIndicator.Count; i++)
        {
            if (!listIndicator[i].Active && type == listIndicator[i].Type)
            {
                return listIndicator[i];
            }
        }
        var result = GameObject.Instantiate(prefab, Controller.transform);
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
