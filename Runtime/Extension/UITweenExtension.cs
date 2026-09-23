using System;
using DG.Tweening;
using UnityEngine;

public static class UITween
{
    public const float DurFast = 0.12f;
    public const float DurPop = 0.32f;
    public const float DurFade = 0.2f;

    public static T Bind<T>(this T tween, Component owner) where T : Tween
    {
        if (tween == null || owner == null) return tween;
        return tween.SetUpdate(true).SetLink(owner.gameObject, LinkBehaviour.KillOnDisable);
    }

    public static Sequence PopIn(Transform target, float duration = DurPop, float delay = 0f,
                                 float fromScale = 0.4f, float spin = 0f, Action onComplete = null)
    {
        if (target == null) return null;

        DOTween.Kill(target, true);

        Vector3 to = target.localScale.sqrMagnitude > 0.0001f ? target.localScale : Vector3.one;
        Vector3 toRotation = target.localEulerAngles;
        var canvas = target.GetComponent<CanvasGroup>();

        target.localScale = to * fromScale;
        if (spin != 0f) target.localEulerAngles = toRotation + new Vector3(0, 0, spin);
        if (canvas != null) canvas.alpha = 0f;

        Sequence seq = DOTween.Sequence().SetId(target);
        seq.AppendInterval(delay);
        seq.Append(target.DOScale(to, duration).SetEase(Ease.OutBack, 2.2f));
        if (canvas != null)
            seq.Join(canvas.DOFade(1f, duration * 0.6f).SetEase(Ease.OutQuad));
        if (spin != 0f)
            seq.Join(target.DOLocalRotate(toRotation, duration).SetEase(Ease.OutBack));

        seq.OnComplete(() => onComplete?.Invoke());
        return seq.Bind(target);
    }

    public static Sequence PopOut(Transform target, float duration = 0.22f, float delay = 0f, Action onComplete = null)
    {
        if (target == null) return null;

        DOTween.Kill(target, true);

        Vector3 from = target.localScale.sqrMagnitude > 0.0001f ? target.localScale : Vector3.one;
        var canvas = target.GetComponent<CanvasGroup>();

        Sequence seq = DOTween.Sequence().SetId(target);
        seq.AppendInterval(delay);
        seq.Append(target.DOScale(from * 1.08f, duration * 0.3f).SetEase(Ease.OutQuad));
        seq.Append(target.DOScale(Vector3.zero, duration * 0.7f).SetEase(Ease.InBack));
        if (canvas != null)
            seq.Join(canvas.DOFade(0f, duration * 0.7f).SetEase(Ease.InQuad));

        seq.OnComplete(() => onComplete?.Invoke());
        return seq.Bind(target);
    }

    public static Tween Bump(Transform target, float strength = 0.25f, float duration = 0.35f)
    {
        if (target == null) return null;
        target.DOKill(true);
        return target.DOPunchScale(Vector3.one * strength, duration, 8, 0.8f).Bind(target);
    }

    public static Tween Deny(Transform target, float strength = 18f, float duration = 0.4f)
    {
        if (target == null) return null;
        target.DOComplete(true);
        return target.DOShakePosition(duration, new Vector3(strength, 0, 0), 16, 90f, false, true).Bind(target);
    }

    public static Tween Press(Transform target, Vector3 baseScale, float pressScale = 0.92f, float duration = DurFast)
    {
        if (target == null) return null;
        target.DOKill();
        return target.DOScale(baseScale * pressScale, duration).SetEase(Ease.OutQuad).Bind(target);
    }

    public static Tween Release(Transform target, Vector3 baseScale, float duration = 0.22f)
    {
        if (target == null) return null;
        target.DOKill();
        return target.DOScale(baseScale, duration).SetEase(Ease.OutBack, 3f).Bind(target);
    }

    public static Tween Fade(CanvasGroup canvas, float to, float duration = DurFade, float delay = 0f)
    {
        if (canvas == null) return null;
        canvas.DOKill();
        return canvas.DOFade(to, duration).SetDelay(delay).SetEase(Ease.OutQuad).Bind(canvas);
    }
}
