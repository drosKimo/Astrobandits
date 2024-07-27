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

    // �������� �������� ����� �� �����
    public void getCardToPlay()
    {
        playCard = dragScript.hit.collider.gameObject.GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            default:
                playCard.Bang();
                break;
        }
    }
}
