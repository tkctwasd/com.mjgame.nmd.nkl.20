#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Hạ tầng chạy thử hiệu ứng UI trong Edit mode. Chỉ lo 3 việc:
/// chụp/trả nguyên trạng, nạp tween cho DOTween preview, và bật/tắt vòng tick.
///
/// Không biết gì về thứ tự runtime - phần đó là việc của <see cref="UIPanelPreviewRunner"/>.
/// </summary>
public static class UIMotionPreview
{
    private class Snapshot
    {
        public GameObject go;
        public bool wasActive;
        public RectTransform rect;
        public Vector3 scale, position, rotation;
        public CanvasGroup canvas;
        public float alpha;
    }

    private static readonly List<Snapshot> snapshots = new List<Snapshot>();
    private static int queued;

    public static bool IsPreviewing { get; private set; }

    #region Batch

    public static void BeginBatch()
    {
        Stop();
        queued = 0;
    }

    /// <summary>Chụp trạng thái hiện tại để <see cref="Stop"/> trả về đúng như cũ.</summary>
    public static void Capture(Component component)
    {
        if (component == null) return;

        var rect = component.GetComponent<RectTransform>();
        if (rect == null) return;

        foreach (var existing in snapshots)
        {
            if (existing.rect == rect) return;
        }

        var canvas = component.GetComponent<CanvasGroup>();
        snapshots.Add(new Snapshot
        {
            go = component.gameObject,
            wasActive = component.gameObject.activeSelf,
            rect = rect,
            scale = rect.localScale,
            position = rect.anchoredPosition3D,
            rotation = rect.localEulerAngles,
            canvas = canvas,
            alpha = canvas != null ? canvas.alpha : 1f
        });
    }

    /// <summary>clearCallbacks mặc định = true nên callback StartAnimation() bị bỏ,
    /// tránh idle loop vô hạn chạy trong Edit mode.</summary>
    public static void Add(Tween tween)
    {
        if (tween == null) return;

        DOTweenEditorPreview.PrepareTweenForPreview(tween);
        queued++;
    }

    public static void PlayBatch()
    {
        if (queued == 0)
        {
            Restore();
            return;
        }

        DOTweenEditorPreview.Start();
        IsPreviewing = true;
    }

    #endregion

    public static void Stop()
    {
        if (IsPreviewing)
            DOTweenEditorPreview.Stop();

        Restore();
        IsPreviewing = false;
        queued = 0;
    }

    private static void Restore()
    {
        foreach (var snap in snapshots)
        {
            if (snap.rect == null) continue;

            snap.rect.DOKill();
            snap.rect.localScale = snap.scale;
            snap.rect.anchoredPosition3D = snap.position;
            snap.rect.localEulerAngles = snap.rotation;

            if (snap.canvas != null) snap.canvas.alpha = snap.alpha;
            if (snap.go != null) snap.go.SetActive(snap.wasActive);
        }
        snapshots.Clear();
    }

    /// <summary>Awake() không chạy ở Edit mode nên rect/canvas/originalPos đều chưa có.</summary>
    public static void Initialize(UITransition transition, MonoBehaviour owner)
    {
        transition?.Initialize(owner);
    }

    /// <summary>Nút Play / Stop dùng chung cho các Inspector.</summary>
    public static void DrawControls(string playLabel, Action onPlay,
                                    string altLabel = null, Action onAlt = null)
    {
        if (Application.isPlaying) return;

        EditorGUILayout.Space(6);
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button(playLabel)) onPlay?.Invoke();
            if (altLabel != null && GUILayout.Button(altLabel)) onAlt?.Invoke();

            using (new EditorGUI.DisabledScope(!IsPreviewing))
            {
                if (GUILayout.Button("■ Stop", GUILayout.Width(70))) Stop();
            }
        }
    }
}
#endif
