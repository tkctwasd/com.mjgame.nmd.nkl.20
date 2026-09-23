using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementTransitionAuto : MonoBehaviour
{
    public UITransition showTransition;
    public UITransition hideTransition;

    CanvasGroup canvas;

    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        showTransition?.Initialize(this);
        hideTransition?.Initialize(this);
    }

    void OnEnable()
    {
        showTransition?.Reset();
        showTransition?.Play(true);
    }

    void OnDisable()
    {
        hideTransition?.Play(false, 0f, () => DOTween.Kill(transform));
    }
}
