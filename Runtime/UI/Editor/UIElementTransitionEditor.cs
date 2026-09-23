#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Chạy thử riêng một element - dùng khi đang canh Slide/Scale cho chính nó,
/// không phải xem cả chuỗi của panel.
/// </summary>
[CustomEditor(typeof(UIElementTransition))]
public class UIElementTransitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var element = (UIElementTransition)target;

        UIMotionPreview.DrawControls(
            "▶ In", () => Play(element, true),
            "▶ Out", () => Play(element, false));
    }

    private static void Play(UIElementTransition element, bool isShow)
    {
        UIMotionPreview.BeginBatch();

        UIMotionPreview.Capture(element);
        element.gameObject.SetActive(true);
        UIMotionPreview.Initialize(element.showTransition, element);
        UIMotionPreview.Initialize(element.hideTransition, element);

        UIMotionPreview.Add(isShow ? element.PlayIn() : element.PlayOut());

        UIMotionPreview.PlayBatch();
    }

    private void OnDisable()
    {
        if (UIMotionPreview.IsPreviewing) UIMotionPreview.Stop();
    }
}

/// <summary>Tương tự cho element tự chạy - không nằm trong elementGroups nào.</summary>
[CustomEditor(typeof(UIElementTransitionAuto))]
public class UIElementTransitionAutoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var auto = (UIElementTransitionAuto)target;

        UIMotionPreview.DrawControls(
            "▶ In", () => Play(auto, true),
            "▶ Out", () => Play(auto, false));
    }

    private static void Play(UIElementTransitionAuto auto, bool isShow)
    {
        var transition = isShow ? auto.showTransition : auto.hideTransition;
        if (transition == null) return;

        UIMotionPreview.BeginBatch();

        UIMotionPreview.Capture(auto);
        auto.gameObject.SetActive(true);
        UIMotionPreview.Initialize(transition, auto);

        if (isShow) transition.Reset();
        UIMotionPreview.Add(transition.Play(isShow));

        UIMotionPreview.PlayBatch();
    }

    private void OnDisable()
    {
        if (UIMotionPreview.IsPreviewing) UIMotionPreview.Stop();
    }
}
#endif
