using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float interactDistance = 5f;
    private List<GameObject> forwardedTargets = new List<GameObject>();
    public LayerMask interactMask;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected != gameObject &&
                selected.GetComponent<Joystick>() == null &&
                selected.GetComponent<FixedTouchField>() == null)
            {
                return;
            }
        }

        TryInteract(eventData.position);

        forwardedTargets = PassEventToUnderlying(eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        foreach (var target in forwardedTargets)
            ExecuteEvents.Execute(target, eventData, ExecuteEvents.dragHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var target in forwardedTargets)
            ExecuteEvents.Execute(target, eventData, ExecuteEvents.pointerUpHandler);
        forwardedTargets.Clear();
    }

    private void TryInteract(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            var interactable = hit.collider.GetComponent<ItemsInteract>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private List<GameObject> PassEventToUnderlying<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> func)
        where T : IEventSystemHandler
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        var forwarded = new List<GameObject>();

        foreach (var result in raycastResults)
        {
            if (result.gameObject == gameObject) continue;

            if (result.gameObject.GetComponent<Joystick>() != null ||
                result.gameObject.GetComponent<FixedTouchField>() != null)
            {
                ExecuteEvents.Execute(result.gameObject, eventData, func);
                forwarded.Add(result.gameObject);
            }
        }

        return forwarded;
    }
}
