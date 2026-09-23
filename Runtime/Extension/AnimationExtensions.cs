using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UnityExtensions
{
    public static class AnimationExtensions
    {
        public static void PlayAppear(Transform target, int index, Ease ease = Ease.OutBack, float delayStep = 0.1f, float duration = 0.35f)
        {
            target.DOKill();

            target.localScale = Vector3.zero;

            target.DOScale(1f, duration).SetDelay(index * delayStep).SetEase(ease);
        }


        public static void PlayPunchScale(Transform target, Ease ease = Ease.OutBack, float scale = 0.15f, float duration = 0.5f)
        {
            target.DOKill();

            target.transform.localScale = Vector3.one;

            target.transform.DOPunchScale(Vector3.one * scale, duration, 5, 0.5f).OnComplete(() => { target.transform.localScale = Vector3.one; });
        }
    }
}