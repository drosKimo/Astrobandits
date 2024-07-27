using UnityEngine;
using Unity.VisualScripting;

public class CardProperty : MonoBehaviour
{
    GetCardItem getCardItem;
    DragScript dragScript;
    PlayCard playCard;

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        dragScript = GetComponent<DragScript>();       
    }

    // включает свойство карты по имени
    public void GetCardToPlay()
    {
        playCard = dragScript.hit.collider.gameObject.GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            default:
                playCard.Bang();
                break;
        }

        Destroy(gameObject);
    }

    public void PlayBoardCard()
    {
        Destroy(gameObject);
        //Debug.Log("…ес минус три");
    }
}
