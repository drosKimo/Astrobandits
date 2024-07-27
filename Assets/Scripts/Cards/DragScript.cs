using Akassets.SmoothGridLayout;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    LerpToPlaceholder LtP;
    ShowCard showCard;
    CardProperty cardProperty;
    GetCardItem getCardItem;
    GameObject placeholder;

    [HideInInspector] public RaycastHit2D hit;

    void Start()
    {
        showCard = gameObject.GetComponent<ShowCard>();
        LtP = GetComponent<LerpToPlaceholder>();
        cardProperty = GetComponent<CardProperty>();
        getCardItem = GetComponent<GetCardItem>();
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
        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (!hit.collider.IsUnityNull()) // есть ли под мышью коллайдер
        {
            if ((getCardItem.cardItem.itemOther == false && hit.collider.gameObject.tag == "Enemy") ||
                (getCardItem.cardItem.itemOther == true && hit.collider.gameObject.tag == "Player") ||
                (getCardItem.cardItem.itemBoard == true) || (hit.collider.gameObject.tag == "Untagged"))
            {
                CantPlay();
            }
            else
            {
                CanPlay();
            }
        }
        else
        {
            if (getCardItem.cardItem.itemBoard == true)
            {
                showCard.enabled = false; // отключает скрипт с триггером
                cardProperty.PlayBoardCard(); // запускает скрипт, который разыгравает карту, которую можно сыграть только на поле
            }
            else
                CantPlay();
        }
    }

    void CantPlay()
    {
        //Debug.Log("Нельзя применить");
        placeholder.SetActive(true);
        gameObject.transform.SetSiblingIndex(showCard.trans);
        LtP.enabled = true;
    }

    void CanPlay()
    {
        showCard.enabled = false; // отключает скрипт с триггером
        cardProperty.GetCardToPlay(); // запускает скрипт, который ищет свойство текущей карты
    }
}
