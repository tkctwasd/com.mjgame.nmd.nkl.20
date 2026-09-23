using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchRaycast : MonoBehaviour
{
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private LayerMask interactMask;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            if (CanTouchPassThroughUI(Input.mousePosition, null))
                TryInteract(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (CanTouchPassThroughUI(t.position, t.fingerId))
                    TryInteract(t.position);
            }
        }
#endif
    }

    private bool CanTouchPassThroughUI(Vector2 pos, int? fingerID)
    {
        if (EventSystem.current == null)
            return true;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        if (fingerID != null)
            eventData.pointerId = fingerID.Value;

        eventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            GameObject ui = r.gameObject;
            if (ui.GetComponent<RaycastContinue>())
                continue;
            return false;
        }

        return true;
    }

    private void TryInteract(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            // 🔥 In ra object bị click
            Debug.LogWarning("TouchRaycast".Message($"Hit object: {hit.collider.gameObject.name}"));

            ItemsInteract interactable = hit.collider.GetComponent<ItemsInteract>();

            if (interactable != null)
            {
                Debug.LogWarning("TouchRaycast".Message("Interact with: {interactable.name}"));
                interactable.Interact();
            }
            else
            {
                Debug.LogWarning("TouchRaycast".Message("Hit object has NO ItemsInteract component."));
            }
        }
        else
        {
            Debug.LogWarning("TouchRaycast".Message(" No object hit"));
        }
    }

}
