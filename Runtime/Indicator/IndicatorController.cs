using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityExtensions;

public class IndicatorController : MonoBehaviour
{

    private Camera mainCamera;
    private Vector3 screenCenter;

    private void Awake()
    {
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        DrawIndicators();
    }
    private void OnDestroy()
    {
        IndicatorManager.ClearIndicators();
    }
    void DrawIndicators()
    {
        // Player cua game tu dang ky IPlayer vao PlayerBootstrap
        if (!PlayerBootstrap.Has<IPlayer>()) return;
        Transform playerTransform = PlayerBootstrap.Get<IPlayer>().Root;
        if (playerTransform == null) return;
        var playerPos = playerTransform.position;
        var targetsByType = IndicatorManager.TargetsByType;

        foreach (var pair in targetsByType)
        {
            IndicatorType type = pair.Key;
            if (type == IndicatorType.None || !IndicatorManager.CanShow(type)) continue;

            List<IndicatorTarget> targets = pair.Value;
            IndicatorTarget nearest = null;
            float nearestDistSqr = float.MaxValue;
            for (int i = 0; i < targets.Count; i++)
            {
                float maxDistSqr = targets[i].DistanceDisplay * targets[i].DistanceDisplay;
                float distSqr = Vector3.SqrMagnitude(playerPos - targets[i].transform.position);

                if (distSqr <= maxDistSqr && distSqr < nearestDistSqr)
                {
                    nearestDistSqr = distSqr;
                    nearest = targets[i];
                }
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target == nearest)
                {
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(target.transform.position);
                    if (IsTargetVisible(screenPos))
                    {
                        screenPos.z = 0;

                        if (target.Indicator == null || target.Indicator.IndicatorType != type)
                        {
                            target.Indicator = IndicatorManager.GetIndicator(type);
                            // if (target.Indicator != null) target.Indicator.Activate(true);
                            if (target.Indicator != null)
                            {
                                target.Indicator.Configure(target.gameObject);
                            }
                        }

                        if (target.Indicator != null)
                        {
                            var ind = target.Indicator;
                            ind.ObjectIndicator = target.gameObject;
                            ind.Activate(true);

                            Vector3 targetPos = screenPos + Vector3.up * 100 +
                                                Vector3.right * target.Offset.x +
                                                Vector3.down * target.Offset.y;

                            if ((ind.transform.position - targetPos).sqrMagnitude > 0.01f)
                                ind.transform.position = targetPos;

                            float threshold = 200f + ind.Height;
                            bool shouldBeActive = ind.transform.position.y > threshold;
                            if (ind.gameObject.activeSelf != shouldBeActive) ind.Activate(shouldBeActive);
                        }
                    }
                    else
                    {
                        ReleaseIndicator(target);
                    }
                }
                else
                {
                    ReleaseIndicator(target);
                }
            }
        }
    }

    private void ReleaseIndicator(IndicatorTarget target)
    {
        if (target.Indicator != null)
        {
            target.Indicator.Activate(false);
            target.Indicator = null;
        }
    }

    private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
    {
        if (indicator != null && indicator.IndicatorType == type && indicator.gameObject.activeSelf)
        {
            return indicator;
        }

        indicator = IndicatorManager.GetIndicator(type);

        if (indicator != null)
        {
            indicator.Activate(true);
        }

        return indicator;
    }

    public bool IsTargetVisible(Vector3 screenPosition)
    {
        bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 180 && screenPosition.y < Screen.height;
        return isTargetVisible;
    }
}