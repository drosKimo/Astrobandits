using Akassets.SmoothGridLayout;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag_n_Drop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    LerpToPlaceholder LtP;
    ShowCard showCard;
    GameObject placeholder;

    void Start()
    {
        showCard = GameObject.Find("ShowCard").GetComponent<ShowCard>();
        LtP = gameObject.GetComponent<LerpToPlaceholder>();
    }

    public void OnDrag(PointerEventData eventData)
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
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.SetSiblingIndex(showCard.trans);
        LtP.enabled = true;
    }
}
