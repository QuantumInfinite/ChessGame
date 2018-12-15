using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerExample : EventTrigger
{
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public override void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag called.");
    }

    public override void OnDrag(PointerEventData data)
    {
        transform.position = new Vector2(data.position.x - rectTransform.rect.width, data.position.y - rectTransform.rect.height);
        print(data.position);
        
    }

    public override void OnEndDrag(PointerEventData data)
    {
        print(transform.localPosition);
        transform.localPosition = new Vector3(
            RoundToMultiple(transform.localPosition.x, 20),
            RoundToMultiple(transform.localPosition.y, 20),
            transform.localPosition.z
        );

        
    }

    public override void OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointerDown called.");
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("OnPointerEnter called.");
    }

    public override void OnPointerExit(PointerEventData data)
    {
        Debug.Log("OnPointerExit called.");
    }

    public override void OnPointerUp(PointerEventData data)
    {
        Debug.Log("OnPointerUp called.");
    }

    float RoundToMultiple(float value, float multiple)
    {
        float rem = value % multiple;
        return (rem >= multiple / 2.0f) ? ((value - rem) + multiple) : (value - rem);
    }
}