using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;


[Serializable]
public class UITransition
{
    public enum TransitionType
    {
        Fade,
        Scale,
        Slide,
        PopBounce, 
        ZoomOut  
    }
    public TransitionType transition = TransitionType.Fade;
    public float duration = 0.4f;
    public float delay = 0f;
    public Ease ease = Ease.OutBack;

    [ShowIf(nameof(transition), (int)TransitionType.Slide)]
    public Vector3 slideTarget = new Vector3(0, -100, 0);

    [ShowIf(nameof(transition), (int)TransitionType.Scale, (int)TransitionType.PopBounce, (int)TransitionType.ZoomOut)]
    public Vector3 scaleFrom = Vector3.zero;

    [ShowIf(nameof(transition), (int)TransitionType.Fade)]
    public float fadeTo = 1f;

    [Tooltip("Fade kèm theo chuyển động chính - làm mọi thứ mềm hơn hẳn.")]
    [ShowIf(nameof(transition), (int)TransitionType.Scale, (int)TransitionType.Slide, (int)TransitionType.PopBounce, (int)TransitionType.ZoomOut)]
    public bool withFade = true;

    [Tooltip("Độ vượt đà cho các ease dạng Back/Elastic. 1.7 = mặc định của DOTween.")]
    [Range(0.5f, 5f)]
    public float overshoot = 1.7f;

    [Tooltip("Kích thước ban đầu so với kích thước đích khi dùng ZoomOut.")]
    [ShowIf(nameof(transition), (int)TransitionType.ZoomOut)]
    public float zoomFrom = 1.6f;

    private RectTransform rect;
    private CanvasGroup canvas;
    private Tween current;

    private Vector3 originalPos;

    public void Initialize(MonoBehaviour owner)
    {
        if (owner == null) return;
        rect = owner.GetComponent<RectTransform>();
        canvas = owner.GetComponent<CanvasGroup>();
        originalPos = rect.anchoredPosition3D;
    }

    /// <summary>Kích thước đích của chuyển động (scaleFrom được dùng như giá trị đích).</summary>
    private Vector3 TargetScale => scaleFrom == Vector3.zero && transition != TransitionType.Scale
                                   ? Vector3.one
                                   : scaleFrom;

    public Tween Play(Action onComplete = null) => Play(true, 0f, onComplete);

    /// <param name="isShow">true = chuyển động vào, false = chuyển động ra (fade sẽ chạy ngược).</param>
    /// <param name="extraDelay">Delay cộng thêm, dùng cho hiệu ứng so le (stagger).</param>
    public Tween Play(bool isShow, float extraDelay = 0f, Action onComplete = null)
    {
        if (rect == null)
        {
            onComplete?.Invoke();
            return null;
        }

        current?.Kill();

        float dur = Mathf.Max(0.01f, duration);
        float wait = Mathf.Max(0f, delay + extraDelay);
        Vector3 target = TargetScale;

        Sequence seq = DOTween.Sequence();

        switch (transition)
        {
            case TransitionType.Fade:
                if (canvas != null)
                    seq.Insert(wait, canvas.DOFade(fadeTo, dur).SetEase(ease));
                break;

            case TransitionType.Scale:
                seq.Insert(wait, rect.DOScale(target, dur).SetEase(ease, overshoot));
                InsertFade(seq, wait, dur, isShow);
                break;

            case TransitionType.Slide:
                seq.Insert(wait, rect.DOAnchorPos3D(slideTarget, dur).SetEase(ease, overshoot));
                InsertFade(seq, wait, dur, isShow);
                break;

            case TransitionType.PopBounce:
                seq.Insert(wait, rect.DOScale(target * 1.12f, dur * 0.55f).SetEase(Ease.OutQuad));
                seq.Insert(wait + dur * 0.55f, rect.DOScale(target, dur * 0.45f).SetEase(Ease.OutBack, overshoot));
                InsertFade(seq, wait, dur * 0.55f, isShow);
                break;

            case TransitionType.ZoomOut:
                seq.Insert(wait, rect.DOScale(target, dur).SetEase(Ease.OutCubic));
                InsertFade(seq, wait, dur * 0.6f, isShow);
                break;
        }

        seq.OnComplete(() => onComplete?.Invoke());
        seq.SetUpdate(true).SetLink(rect.gameObject, LinkBehaviour.KillOnDestroy);

        current = seq;
        return seq;
    }

    private void InsertFade(Sequence seq, float at, float dur, bool isShow)
    {
        if (!withFade || canvas == null) return;
        seq.Insert(at, canvas.DOFade(isShow ? 1f : 0f, dur).SetEase(Ease.OutQuad));
    }

    /// <summary>Đưa element về trạng thái bắt đầu của chuyển động vào.</summary>
    public void Reset()
    {
        if (rect == null) return;

        current?.Kill();
        current = null;

        float startAlpha = withFade ? 0f : 1f;
        rect.anchoredPosition3D = originalPos;

        switch (transition)
        {
            case TransitionType.Fade:
                rect.localScale = Vector3.one;
                if (canvas != null) canvas.alpha = 0f;
                break;

            case TransitionType.Scale:
            case TransitionType.PopBounce:
                rect.localScale = Vector3.zero;
                if (canvas != null) canvas.alpha = startAlpha;
                break;

            case TransitionType.Slide:
                rect.localScale = Vector3.one;
                if (canvas != null) canvas.alpha = startAlpha;
                break;

            case TransitionType.ZoomOut:
                rect.localScale = TargetScale * Mathf.Max(0.01f, zoomFrom);
                if (canvas != null) canvas.alpha = startAlpha;
                break;
        }
    }
}



[Serializable]
public class UIElementGroup
{
    public List<UIElementTransition> elements = new List<UIElementTransition>();

    [Tooltip("Thời gian chờ trước khi chạy group kế tiếp.")]
    public float duration = 0.3f;

    [Tooltip("Độ trễ giữa từng element trong group - tạo hiệu ứng nối đuôi.")]
    public float stagger = 0f;

    public bool playHide = false;

    /// <summary>Tổng thời gian group chiếm, đã tính cả phần so le.</summary>
    public float TotalDuration => duration + Mathf.Max(0, elements.Count - 1) * Mathf.Max(0f, stagger);

    public void PlayShow()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var item = elements[i];
            if (item == null) continue;

            item.gameObject.SetActive(true);
            item.PlayIn(i * Mathf.Max(0f, stagger));
        }
    }

    public void PlayHide()
    {
        if (!playHide) return;
        for (int i = 0; i < elements.Count; i++)
        {
            var item = elements[i];
            if (item == null) continue;

            item.PlayOut(i * Mathf.Max(0f, stagger));
        }
    }

    public void HideImmediate()
    {
        foreach (var item in elements)
        {
            if (item != null)
                item.ResetShow();
        }
    }
}


[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class UIElementTransition : MonoBehaviour
{
    public UITransition showTransition = new UITransition();
    public UITransition hideTransition = new UITransition();

    private void Awake()
    {
        showTransition?.Initialize(this);
        hideTransition?.Initialize(this);
    }


    public Tween PlayIn(float extraDelay = 0f, Action onComplete = null)
    {
        showTransition.Reset();
        return showTransition.Play(true, extraDelay, () =>
        {
            var anim = GetComponent<UIElementAnimation>();
            anim?.StartAnimation();
            onComplete?.Invoke();
        });
    }

    public Tween PlayOut(float extraDelay = 0f, Action onComplete = null)
    {
        var anim = GetComponent<UIElementAnimation>();
        anim?.StopAnimation();
        return hideTransition.Play(false, extraDelay, onComplete);
    }

    public void ResetShow()
    {
        var anim = GetComponent<UIElementAnimation>();
        anim?.StopAnimation();
        showTransition?.Reset();
    }
}
