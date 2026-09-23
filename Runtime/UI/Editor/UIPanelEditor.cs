#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Nút chạy thử cả panel theo đúng trình tự runtime.
/// editorForChildClasses = true nên mọi PanelXxx đều có, không cần viết editor riêng.
/// </summary>
[CustomEditor(typeof(UIPanel), true)]
public class UIPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var panel = (UIPanel)target;

        int groups = panel.EditorElementGroups.Count;
        int autos = panel.GetComponentsInChildren<UIElementTransitionAuto>(true).Length;
        int reveals = panel.GetComponentsInChildren<UIDelayedReveal>(true).Length;

        if (groups == 0 && autos == 0 && reveals == 0) return;

        UIMotionPreview.DrawControls(
            "▶ Show", () => UIPanelPreviewRunner.PlayShow(panel),
            "▶ Hide", () => UIPanelPreviewRunner.PlayHide(panel));

        EditorGUILayout.LabelField(
            $"{groups} group · {autos} auto · {reveals} delayed",
            EditorStyles.miniLabel);
    }

    private void OnDisable()
    {
        // Đổi selection giữa lúc đang preview: dừng và trả nguyên trạng.
        if (UIMotionPreview.IsPreviewing) UIMotionPreview.Stop();
    }
}
#endif
