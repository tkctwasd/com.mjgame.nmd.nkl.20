using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProcess : MonoBehaviour
{
    public enum TextMode { Hide, Percent, Fraction, Value, Remain, Custom }

    [Header("Value")]
    [SerializeField] protected ProcessValue process = new ProcessValue();

    [Header("View")]
    [SerializeField] protected Image imgFill;
    [SerializeField] protected Image imgFillBackground;
    [SerializeField] protected TMP_Text text;

    [Header("Color")]
    [SerializeField] protected Color colorFill = Color.green;
    [SerializeField] protected Color colorBackground = Color.gray;
    [SerializeField] protected bool useGradient = false;
    [SerializeField] protected Gradient gradientFill;

    [Header("Text")]
    [SerializeField] protected TextMode textMode = TextMode.Percent;
    [Tooltip("{0} = value, {1} = max, {2} = percent, {3} = remain")]
    [SerializeField] protected string customFormat = "{0}/{1}";

    [Header("Smooth")]
    [SerializeField] protected bool smooth = true;
    [Tooltip("Thời gian chạy hết một lần đổi giá trị (giây), dài ngắn bao nhiêu cũng chạy đúng bằng đây")]
    [SerializeField] protected float duration = 0.3f;
    [Tooltip("Để trống thì dùng SmoothStep (ease in-out)")]
    [SerializeField] protected AnimationCurve ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] protected bool useUnscaledTime = true;

    protected float displayFill;
    protected float targetFill;
    protected float fillFrom;   // fill lúc bắt đầu chặng chạy
    protected float playTime;   // đã chạy được bao lâu

    public double Value
    {
        get => process.Value;
        set => SetValue(value);
    }

    public float Normalized
    {
        get => process.Normalized;
        set => SetNormalized(value);
    }

    public double Min => process.Min;
    public double Max => process.Max;
    public double Remain => process.Remain;
    public bool IsFull => process.IsFull;

    public bool IsPlaying => displayFill != targetFill;

    /// <summary>Thời gian chạy animation, đổi được lúc runtime cho từng chặng.</summary>
    public float Duration
    {
        get => duration;
        set => duration = Mathf.Max(0f, value);
    }

    protected virtual void Awake() => ForceRefresh();

    protected virtual void OnEnable() => ForceRefresh();

    protected virtual void Update()
    {
        if (displayFill == targetFill) return;

        playTime += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        float t = duration <= 0f ? 1f : Mathf.Clamp01(playTime / duration);
        displayFill = t >= 1f ? targetFill : Mathf.LerpUnclamped(fillFrom, targetFill, Ease(t));

        ApplyFill();
    }

    protected float Ease(float t) => ease != null && ease.length > 1 ? ease.Evaluate(t) : Mathf.SmoothStep(0f, 1f, t);

    #region API

    /// <summary>Set một lần cả value lẫn khoảng min/max.</summary>
    public void Setup(double value, double min, double max, bool instant = true)
    {
        process.Setup(value, min, max);
        Refresh(instant);
    }

    public void SetValue(double value, bool instant = false)
    {
        process.SetValue(value);
        Refresh(instant);
    }

    public void AddValue(double amount, bool instant = false)
    {
        process.AddValue(amount);
        Refresh(instant);
    }

    /// <summary>Set thẳng theo tỉ lệ 0..1 (bỏ qua min/max thật).</summary>
    public void SetNormalized(float normalized, bool instant = false)
    {
        process.SetNormalized(normalized);
        Refresh(instant);
    }

    /// <param name="keepNormalized">true: giữ nguyên tỉ lệ hiện tại khi đổi range.</param>
    public void SetMinMax(double min, double max, bool keepNormalized = false, bool instant = false)
    {
        process.SetMinMax(min, max, keepNormalized);
        Refresh(instant);
    }

    public void SetValueMinMax(double min, double max) => SetMinMax(min, max);

    public void SetMin(double min) => SetMinMax(min, process.Max);

    public void SetMax(double max) => SetMinMax(process.Min, max);

    public void SetColor(Color color)
    {
        colorFill = color;
        if (imgFill != null) imgFill.color = color;
    }

    /// <summary>Cập nhật hiển thị theo giá trị hiện tại.</summary>
    public void Refresh(bool instant = false)
    {
        float newTarget = process.Normalized;

        if (instant || !smooth || duration <= 0f)
        {
            targetFill = newTarget;
            displayFill = newTarget;
        }
        else if (newTarget != targetFill)
        {
            fillFrom = displayFill;     // chạy tiếp từ chỗ đang hiển thị
            targetFill = newTarget;
            playTime = 0f;              // mỗi lần đổi giá trị là một chặng mới, luôn tốn đúng duration
        }

        ApplyFill();
        ApplyText();
    }

    /// <summary>Nhảy thẳng tới giá trị hiện tại, không chạy animation.</summary>
    public void ForceRefresh() => Refresh(true);

    #endregion

    protected virtual void ApplyFill()
    {
        if (imgFill != null)
        {
            imgFill.fillAmount = displayFill;
            imgFill.color = useGradient && gradientFill != null ? gradientFill.Evaluate(displayFill) : colorFill;
        }

        if (imgFillBackground != null) imgFillBackground.color = colorBackground;
    }

    protected virtual void ApplyText()
    {
        if (text == null) return;

        text.text = textMode switch
        {
            TextMode.Percent => $"{Mathf.FloorToInt(process.Normalized * 100f)}%",
            TextMode.Fraction => $"{Formatter.FormatNumber(process.Value)}/{Formatter.FormatNumber(process.Max)}",
            TextMode.Value => Formatter.FormatNumber(process.Value),
            TextMode.Remain => Formatter.FormatNumber(process.Remain),
            TextMode.Custom => string.Format(customFormat,
                                             Formatter.FormatNumber(process.Value),
                                             Formatter.FormatNumber(process.Max),
                                             Mathf.FloorToInt(process.Normalized * 100f),
                                             Formatter.FormatNumber(process.Remain)),
            _ => string.Empty
        };
    }

    protected virtual void OnValidate()
    {
        if (duration < 0f) duration = 0f;
        ForceRefresh();
    }
}
