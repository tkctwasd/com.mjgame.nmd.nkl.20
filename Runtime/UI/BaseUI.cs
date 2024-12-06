using UnityEngine;
using System.Collections;
using MJGame;


[RequireComponent(typeof(CanvasGroup))]
public class BaseUI : TickBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    Coroutine coroutine;
    private void OnValidate()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        Init();
    }

    private void Start()
    {
        OnStart();
    }

    public virtual void Init() { }

    public virtual void OnStart() { }

    public void OnShow()
    {
        if (gameObject.activeSelf) return;
        GameObjectExtension.ActiveObj(gameObject, true);
        ResetCoroutine();
        coroutine = StartCoroutine(FadeInPopup());
    }

    public void OnHide()
    {
        if (!gameObject.activeSelf) return;
        ResetCoroutine();
        coroutine = StartCoroutine(FadeOutPopup());
    }

    private void ResetCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator FadeInPopup()
    {
        float timeElapsed = 0f;

        canvasGroup.alpha = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutPopup()
    {
        float timeElapsed = 0f;

        canvasGroup.alpha = 1f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        GameObjectExtension.ActiveObj(gameObject, false);
    }
}
