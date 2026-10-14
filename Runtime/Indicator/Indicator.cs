using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityExtensions;

public enum ButtonType
{
    Click,
    Hold,
    Auto
}

public class Indicator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField, IndicatorType, FormerlySerializedAs("IndicatorType")] int type;
    public int Type => type;
    public ButtonType clickType = ButtonType.Click;
    [SerializeField] private Image circleImage;

    [Header("UI")]
    public GameObject hand;
    public TextMeshProUGUI txt;
    public TextMeshProUGUI txtTitle;

    public GameObject ObjectIndicator;

    private EventTrigger eventTrigger;
    private Tween tween;

    private bool isHolding;
    private bool isAutoRunning;

    public float Height { get; private set; }
    public bool Active => gameObject.activeInHierarchy;

    #region Unity

    protected virtual void Awake()
    {
        Height = GetComponent<RectTransform>().rect.height * 0.5f;

        SetupEventTrigger();
        SetCircle(0f, false);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        StopAllStates();
    }

    #endregion

    #region Setup

    private void SetupEventTrigger()
    {
        eventTrigger = GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Clear();

        RegisterEvent(EventTriggerType.PointerDown, OnPointerDown);
        RegisterEvent(EventTriggerType.PointerUp, OnPointerUp);
    }

    private void RegisterEvent(EventTriggerType type, Action<BaseEventData> callback)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(callback.Invoke);
        eventTrigger.triggers.Add(entry);
    }

    #endregion

    #region Public API

    /// <summary>Cap nhat lai noi dung hien tren indicator. Lop con cua game ghi de.</summary>
    public virtual void Refresh()
    {
    }

    public void Activate(bool value) => GameObjectExtensions.ActiveObj(gameObject, value);

    public void ActiveHand(bool value) => GameObjectExtensions.ActiveObj(hand, value);

    public virtual void SetText(string msg) => txt.text = msg;

    public virtual void SetTitle(string msg)
    {
        if (txtTitle == null) return;
        txtTitle.text = msg;
    }

    public virtual void Configure(GameObject targetObj)
    {
        ObjectIndicator = targetObj;
        Refresh();
    }

    #endregion

    #region Input

    private void OnPointerDown(BaseEventData _)
    {
        switch (clickType)
        {
            case ButtonType.Click:
                Click();
                break;

            case ButtonType.Hold:
                StartHold();
                break;

            case ButtonType.Auto:
                Click();
                StopAuto();
                break;
        }
    }

    private void OnPointerUp(BaseEventData _)
    {
        if (clickType == ButtonType.Hold)
            StopHold();
    }

    #endregion

    #region Core Actions

    protected virtual void Click()
    {
        AudioManager.Instance.AudioClickBtn();
        ActiveHand(false);
        IndicatorManager.RefreshAllActive();
        Debug.Log($"<color=green>[Indicator]</color> Action: {type}");
    }

    #endregion

    #region Hold

    private void StartHold()
    {
        if (!HasCircle())
        {
            Click();
            return;
        }

        isHolding = true;

        PlayFillTween(0.2f, Ease.Linear, () =>
        {
            if (!isHolding) return;

            isHolding = false;
            ResetCircle();
            Click();
        });
    }

    private void StopHold()
    {
        isHolding = false;
        ResetCircle();
    }

    #endregion

    #region Auto

    public void StartAuto()
    {
        if (!HasCircle())
        {
            Click();
            return;
        }

        isAutoRunning = true;
        float time = IndicatorManager.AutoClickTime;

        PlayFillTween(time, Ease.OutCubic, () =>
        {
            if (!isAutoRunning) return;

            Click();
            StopAuto();
        });
    }

    public void StopAuto()
    {
        if (!isAutoRunning) return;

        isAutoRunning = false;
        ResetCircle();
    }

    #endregion

    #region Tween Helpers

    private void PlayFillTween(float duration, Ease ease, Action onComplete)
    {
        KillTween();

        SetCircle(0f, true);

        tween = DOVirtual.Float(0f, 1f, duration, value =>
        {
            if (circleImage != null)
                circleImage.fillAmount = value;
        })
        .SetEase(ease)
        .OnComplete(() => onComplete?.Invoke());
    }

    private void KillTween()
    {
        tween?.Kill();
        tween = null;
    }

    #endregion

    #region Circle

    private bool HasCircle() => circleImage != null;

    private void SetCircle(float value, bool active)
    {
        if (circleImage == null) return;

        circleImage.fillAmount = value;
        GameObjectExtensions.ActiveObj(circleImage.gameObject, active);
    }

    private void ResetCircle()
    {
        KillTween();
        SetCircle(0f, false);
    }

    #endregion

    #region State

    private void StopAllStates()
    {
        isHolding = false;
        isAutoRunning = false;
        ResetCircle();
    }

    #endregion
}