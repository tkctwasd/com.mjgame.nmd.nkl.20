using System.Collections;
using DG.Tweening;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIDelayedReveal : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    [SerializeField] private float duration = 0.32f;

    [SerializeField] private float spin = -60f;

    private RectTransform rect;
    private Coroutine routine;
    private Vector3 baseScale = Vector3.one;

    public float Delay
    {
        get => delay;
        set => delay = Mathf.Max(0f, value);
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
    }

    private void OnEnable()
    {
        if (delay <= 0f)
        {
            rect.localScale = baseScale;
            return;
        }

        rect.localScale = Vector3.zero;
        routine = StartCoroutine(RevealAfterDelay());
    }

    private void OnDisable()
    {
        routine = null;

        rect.DOKill();
        rect.localScale = baseScale;
    }

    private IEnumerator RevealAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delay);

        Reveal();
        routine = null;
    }

    public void RevealNow()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
        Reveal();
    }

#if UNITY_EDITOR
    public Tween EditorPreviewTween(float extraDelay = 0f)
    {
        var target = GetComponent<RectTransform>();
        if (target == null) return null;

        return UITween.PopIn(target, duration, delay + extraDelay, fromScale: 0f, spin: 0);
    }
#endif

    private void Reveal()
    {
        rect.localScale = baseScale;
        UITween.PopIn(rect, duration, fromScale: 0f, spin: 0);
    }
}
