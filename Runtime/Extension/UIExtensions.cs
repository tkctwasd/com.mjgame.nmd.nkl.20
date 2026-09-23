using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityExtensions
{
    public static class UIExtensions
    {
        public static UIButtonPressEffect AddResizeEffect(this Selectable selectable, float pressScale = -1f)
        {
            return selectable == null ? null : AddResizeEffect(selectable.gameObject, pressScale);
        }


        public static UIButtonPressEffect AddResizeEffect(this EventTrigger eventTrigger, float pressScale = -1f)
        {
            return eventTrigger == null ? null : AddResizeEffect(eventTrigger.gameObject, pressScale);
        }

        public static UIButtonPressEffect AddResizeEffect(this GameObject target, float pressScale = -1f)
        {
            if (target == null) return null;

            var effect = target.GetComponent<UIButtonPressEffect>();
            if (effect == null)
                effect = target.AddComponent<UIButtonPressEffect>();

            if (pressScale > 0f)
                effect.pressScale = pressScale;

            return effect;
        }

        public static UIDelayedReveal AddDelayedReveal(this Button button, float delay = -1f)
        {
            return button == null ? null : AddDelayedReveal(button.gameObject, delay);
        }

        public static UIDelayedReveal AddDelayedReveal(this GameObject target, float delay = -1f)
        {
            if (target == null) return null;

            var reveal = target.GetComponent<UIDelayedReveal>();
            if (reveal == null)
                reveal = target.AddComponent<UIDelayedReveal>();

            if (delay >= 0f)
                reveal.Delay = delay;

            return reveal;
        }
    }
}
