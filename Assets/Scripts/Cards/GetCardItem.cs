using UnityEngine;

public class GetCardItem : MonoBehaviour
{
    public Storage globalStorage;
    [HideInInspector] public Cards cardItem;

    [HideInInspector] public int cardIndex;
    [HideInInspector] public string nameKey;
    [HideInInspector] public string descriptionKey;
    [HideInInspector] public bool calling;

    void Awake()
    {
        if (calling == true)
        {
            int minStorage = 0;
            int maxStorage = globalStorage.allCards.Count;

            System.Random rand = new System.Random();

            // выбирает случайную карту
            cardItem = globalStorage.allCards[rand.Next(minStorage, maxStorage)];
            nameKey = cardItem.itemName;
            descriptionKey = cardItem.itemDescription;
        }
        else
        {
            cardItem = globalStorage.allCards[cardIndex];
            nameKey = cardItem.itemName;
            descriptionKey = cardItem.itemDescription;
        }
    }
}
