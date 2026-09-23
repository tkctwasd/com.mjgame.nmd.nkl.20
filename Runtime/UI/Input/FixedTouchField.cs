using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtensions;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Huong dan")]
    [SerializeField] private GameObject tutHint;

    public Vector2 TouchDist;
    private Vector2 pointerOld;
    private int pointerId;
    [SerializeField] private bool pressed;
    private void OnDisable()
    {
        StopTouch();
    }
    private void Update()
    {
        if (pressed)
        {
            if (Time.timeScale == 0)
            {
                TouchDist = new Vector2();
            }
            else
            {
                if (pointerId >= 0 && pointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[pointerId].position - pointerOld;
                    pointerOld = Input.touches[pointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld;
                    pointerOld = Input.mousePosition;
                }
            }
        }
        else
        {
            TouchDist = new Vector2();
        }
    }

    /// <summary>Bat/tat huong dan keo de xoay camera.</summary>
    public void ActiveHint(bool isActive)
    {
        GameObjectExtensions.ActiveObj(tutHint, isActive);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ActiveHint(false);
        pressed = true;
        pointerId = eventData.pointerId;
        pointerOld = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    public void StopTouch()
    {
        pressed = false;
        TouchDist = new Vector2();
    }
}