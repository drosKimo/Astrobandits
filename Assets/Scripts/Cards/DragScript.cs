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

    public void OnDrag(PointerEventData eventData) // ���������� ������ ����
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
        gameObject.transform.localScale = new Vector3(1, 1, 1); // ���������� ������ �������

        // ���������� ����, ����� �� ���������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (!hit.collider.IsUnityNull()) // ���� �� ��� ����� ���������
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
                switch (getCardItem.cardItem.itemName)
                {
                    case "Cards.Name.Pow":
                        Transform player = GameObject.FindWithTag("Player").transform;
                        PlayCard playCard = player.gameObject.GetComponent<PlayCard>();

                        // ������� ����� ���������� �� ����
                        int calc = hierarchy.CalculateCircularDistance(player.transform, hit.collider.gameObject.transform);

                        if (helperData.shotDone) // ������� �� ��� �����
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
                        if (helperData.playerImplantSet) // ���������� �� � ������ �������
                            CantPlay();
                        else
                            CanPlay();
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
                showCard.enabled = false; // ��������� ������ � ���������
                cardProperty.PlayBoardCard(); // ��������� ������, ������� ����������� �����, ������� ����� ������� ������ �� ����
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
        showCard.enabled = false; // ��������� ������ � ���������
        cardProperty.GetCardToPlay(); // ��������� ������, ������� ���� �������� ������� �����
    }
}
