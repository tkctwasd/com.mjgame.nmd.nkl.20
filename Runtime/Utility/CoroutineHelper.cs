using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : Singleton<CoroutineHelper>
{
    public static Coroutine StartCoroutineStatic(System.Collections.IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void StopCoroutineStatic(Coroutine coroutine)
    {
        if (Instance != null && coroutine != null)
        {
            Instance.StopCoroutine(coroutine);
        }
    }

    public static IEnumerator LoopCoroutine(int index, Func<IEnumerator> action, Action onComplete = null)
    {
        for (int i = 0; i < index; i++)
        {
            yield return action();
        }
        onComplete?.Invoke();
    }

}