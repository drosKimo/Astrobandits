using Akassets.SmoothGridLayout;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    LerpToPlaceholder LtP;
    ShowCard showCard;
    GameObject placeholder;
    EventTrigger trigger;

    void Start()
    {
        showCard = GameObject.Find("ShowCard").GetComponent<ShowCard>();
        LtP = GetComponent<LerpToPlaceholder>();
        trigger = GetComponent<EventTrigger>();
    }

    public void OnDrag(PointerEventData eventData) // вызывается каждый кадр
    {
        transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        LtP.enabled = false;
        placeholder = GameObject.Find(gameObject.name + " placeholder");
        placeholder.SetActive(false);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1); // нормальный размер объекта

        // координаты мыши, иначе не получится
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        
        if (!hit.collider.IsUnityNull()) // есть ли под мышью коллайдер
        {
            trigger.enabled = false; // отключает скрипт с триггером

            //Destroy(gameObject);
        }
        else
        {
            gameObject.transform.SetSiblingIndex(showCard.trans);
            LtP.enabled = true;

            // временно, чтобы можно было вернуть карту в руку
            if (trigger.enabled == false)
                trigger.enabled = true;
        }
    }
}
