using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Range(0.5f, 1f)]
    public float pressScale = 0.92f;

    private RectTransform rect;
    private Vector3 baseScale = Vector3.one;
    private bool captured;

    private void Awake() => Capture();

    private void OnDisable()
    {
        if (!captured || rect == null) return;

        rect.DOKill();
        rect.localScale = baseScale;
    }

    private void Capture()
    {
        if (captured) return;

        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
        captured = true;
    }

    public void SetBaseScale(Vector3 scale) => baseScale = scale;

    public void OnPointerDown(PointerEventData eventData)
    {
        Capture();
        UITween.Press(rect, baseScale, pressScale);
    }

    public void OnPointerUp(PointerEventData eventData) => ReleaseVisual();

    public void OnPointerExit(PointerEventData eventData) => ReleaseVisual();

    private void ReleaseVisual()
    {
        if (!captured) return;
        UITween.Release(rect, baseScale);
    }
}
