using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace MJGame
{
    public static class ButtonExtension
    {
        public static void AddResizeEffect(this Button button)
        {
            Vector3 originalScale = button.transform.localScale;

            var buttonRect = button.GetComponent<RectTransform>();
            var buttonEventTrigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => OnPointerDown(buttonRect, originalScale));
            buttonEventTrigger.triggers.Add(entryDown);

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => OnPointerUp(buttonRect, originalScale));
            buttonEventTrigger.triggers.Add(entryUp);
        }
        public static void AddResizeEffect(this EventTrigger eventTrigger)
        {
            RectTransform rectTransform = eventTrigger.gameObject.GetComponent<RectTransform>();
            Vector3 originalScale = rectTransform.localScale;

            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => OnPointerDown(rectTransform, originalScale));
            eventTrigger.triggers.Add(entryDown);

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => OnPointerUp(rectTransform, originalScale));
            eventTrigger.triggers.Add(entryUp);
        }

        private static void OnPointerDown(RectTransform rectTransform, Vector3 originalScale)
        {
            rectTransform.localScale = originalScale * 0.95f;
        }

        private static void OnPointerUp(RectTransform rectTransform, Vector3 originalScale)
        {
            rectTransform.localScale = originalScale;
        }
    }
}
