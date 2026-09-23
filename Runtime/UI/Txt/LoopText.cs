using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoopText : MonoBehaviour
{
    private Coroutine coroutine;
    public TextMeshProUGUI txt;
    public string baseText = "";
    public float time = 0.3f;
    private void OnEnable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(LoopAdBreakTextIE());

    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator LoopAdBreakTextIE()
    {
        string[] dotStates = { "", ".", "..", "..." };

        int index = 0;
        while (true)
        {
            txt.text = baseText + dotStates[index];
            index = (index + 1) % dotStates.Length;
            yield return new WaitForSeconds(time);
        }
    }
}
