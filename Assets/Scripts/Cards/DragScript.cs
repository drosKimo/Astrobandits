using Akassets.SmoothGridLayout;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UIElements.VisualElement;

public class DragScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    LerpToPlaceholder LtP;
    ShowCard showCard;
    CardProperty cardProperty;
    GetCardItem getCardItem;
    HelperData helperData;
    GameObject placeholder;
    PlayerHierarchy hierarchy;

    [HideInInspector] public RaycastHit2D hit;

    void Start()
    {
        showCard = gameObject.GetComponent<ShowCard>();
        LtP = GetComponent<LerpToPlaceholder>();
        cardProperty = GetComponent<CardProperty>();
        getCardItem = GetComponent<GetCardItem>();
        helperData = GameObject.Find("WhenGameStarts").GetComponent<HelperData>();
        hierarchy = GameObject.Find("WhenGameStarts").GetComponent<PlayerHierarchy>();
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
                (getCardItem.cardItem.itemBoard == true) || (hit.collider.gameObject.tag == "Untagged") ||
                 getCardItem.cardItem.itemNull == true)
            {
                CantPlay();
            }
            else
            {
                CharacterRole playerChar = GameObject.FindWithTag("Player").GetComponent<CharacterRole>();

                switch (getCardItem.cardItem.itemName)
                {
                    case "Cards.Name.Pow":
                        PlayCard playCard = playerChar.gameObject.GetComponent<PlayCard>();

                        // считает какое расстояние до цели
                        int calc = hierarchy.CalculateCircularDistance(playerChar.gameObject.transform, hit.collider.gameObject.transform);

                        if (helperData.shotDone) // стрелял ли уже игрок
                            CantPlay();
                        else
                        {
                            if (calc <= playCard.currentDistance)
                                CanPlay();
                            else
                                CantPlay();
                        }
                        break;

                    case "Cards.Name.CyberImplant":
                        if (playerChar.implantSet) // установлен ли у игрока имплант
                            CantPlay();
                        else
                            CanPlay();
                        break;

                    case "Cards.Name.Hemotransfusion":
                        helperData.enemyTransHP = hit.collider.gameObject.GetComponent<CharacterRole>();

                        if (helperData.enemyTransHP.currentHP >= 2 && playerChar.currentHP != playerChar.maxHP)
                            CanPlay();
                        else
                            CantPlay();
                        break;

                    default:
                        CanPlay();
                        break;
                }
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
