#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// Dựng lại đúng trình tự runtime của một panel để xem trong Edit mode.
///
/// Trình tự thật khi UIManager.ShowPanel&lt;T&gt;() chạy:
///
///   t=0   gameObject.SetActive(true)
///         └─ Unity bắn OnEnable toàn bộ subtree, CÙNG MỘT FRAME:
///              • UIElementTransitionAuto  -> Reset + Play, mỗi cái theo delay riêng
///              • UIDelayedReveal          -> scale 0, chờ delay rồi bung
///              • UIElementAnimation       -> idle loop (preview bỏ qua, xem ghi chú dưới)
///   t=0   Show() chạy tiếp: HideAllElementsImmediate() + group[0].PlayShow()
///   t=+group[0].TotalDuration   group[1].PlayShow()
///   ...
///
/// Nghĩa là Auto và elementGroups là hai nguồn SONG SONG cùng khởi tại t=0,
/// không phải nối đuôi nhau - đó là chỗ dễ nhìn nhầm nhất khi chỉ xem từng phần.
///
/// Idle loop của UIElementAnimation cố tình không chạy: nó lặp vô hạn,
/// để chạy trong Edit mode thì vòng preview không bao giờ kết thúc.
/// </summary>
public static class UIPanelPreviewRunner
{
    public static void PlayShow(UIPanel panel)
    {
        if (panel == null) return;

        UIMotionPreview.BeginBatch();

        PlayAutoElements(panel, true);
        PlayDelayedReveals(panel);
        PlayGroupsShow(panel);

        UIMotionPreview.PlayBatch();
    }

    public static void PlayHide(UIPanel panel)
    {
        if (panel == null) return;

        UIMotionPreview.BeginBatch();

        float tail = PlayGroupsHide(panel);

        // Auto.OnDisable chỉ bắn khi gameObject thật sự tắt, tức là ở cuối chuỗi hide.
        // Lúc đó object đã inactive nên ở runtime các tween này không kịp hiện lên -
        // preview cho thấy đúng điều đó.
        PlayAutoElements(panel, false, tail);

        UIMotionPreview.PlayBatch();
    }

    #region Auto

    private static void PlayAutoElements(UIPanel panel, bool isShow, float offset = 0f)
    {
        var autos = panel.GetComponentsInChildren<UIElementTransitionAuto>(true);

        foreach (var auto in autos)
        {
            // Runtime: OnEnable chỉ bắn khi mọi cha trung gian đang bật.
            if (!WillActivateWith(auto.transform, panel.transform)) continue;

            var transition = isShow ? auto.showTransition : auto.hideTransition;
            if (transition == null) continue;

            UIMotionPreview.Capture(auto);
            UIMotionPreview.Initialize(transition, auto);

            if (isShow) transition.Reset();
            UIMotionPreview.Add(transition.Play(isShow, offset));
        }
    }

    #endregion

    #region Delayed reveal

    private static void PlayDelayedReveals(UIPanel panel)
    {
        var reveals = panel.GetComponentsInChildren<UIDelayedReveal>(true);

        foreach (var reveal in reveals)
        {
            if (!WillActivateWith(reveal.transform, panel.transform)) continue;

            UIMotionPreview.Capture(reveal);
            UIMotionPreview.Add(reveal.EditorPreviewTween());
        }
    }

    #endregion

    #region Element groups

    private static void PlayGroupsShow(UIPanel panel)
    {
        float offset = 0f;

        foreach (var group in panel.EditorElementGroups)
        {
            if (group == null) continue;

            float stagger = Mathf.Max(0f, group.stagger);
            for (int i = 0; i < group.elements.Count; i++)
            {
                var element = group.elements[i];
                if (element == null) continue;

                // PlayShow() bật element vô điều kiện, không cần kiểm tra cha.
                UIMotionPreview.Capture(element);
                element.gameObject.SetActive(true);
                InitializeBoth(element);

                UIMotionPreview.Add(element.PlayIn(offset + i * stagger));
            }
            offset += group.TotalDuration;
        }
    }

    /// <returns>Thời điểm chuỗi hide kết thúc, để nối Auto vào sau.</returns>
    private static float PlayGroupsHide(UIPanel panel)
    {
        var groups = panel.EditorElementGroups;
        float offset = 0f;

        for (int g = groups.Count - 1; g >= 0; g--)
        {
            var group = groups[g];
            if (group == null || !group.playHide) continue;

            float stagger = Mathf.Max(0f, group.stagger);
            for (int i = 0; i < group.elements.Count; i++)
            {
                var element = group.elements[i];
                if (element == null) continue;

                UIMotionPreview.Capture(element);
                element.gameObject.SetActive(true);
                InitializeBoth(element);

                UIMotionPreview.Add(element.PlayOut(offset + i * stagger));
            }
            offset += group.TotalDuration;
        }

        return offset;
    }

    #endregion

    private static void InitializeBoth(UIElementTransition element)
    {
        UIMotionPreview.Initialize(element.showTransition, element);
        UIMotionPreview.Initialize(element.hideTransition, element);
    }

    /// <summary>Mọi cha từ element lên tới panel có đang bật không.</summary>
    private static bool WillActivateWith(Transform child, Transform root)
    {
        var current = child;
        while (current != null && current != root)
        {
            if (!current.gameObject.activeSelf) return false;
            current = current.parent;
        }
        return true;
    }
}
#endif
