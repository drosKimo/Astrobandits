using UnityEngine;

public class GetCardItem : MonoBehaviour
{
    public Storage globalStorage;
    [HideInInspector] public Cards cardItem;

    [HideInInspector] public string nameKey;
    [HideInInspector] public string descriptionKey;

    void Awake()
    {
        int minStorage = 0; 
        int maxStorage = globalStorage.allCards.Count;

        Debug.Log(maxStorage);

        System.Random rand = new System.Random();

        cardItem = globalStorage.allCards[rand.Next(minStorage, maxStorage)];
        //cardItem = globalStorage.allCards[0];

        nameKey = cardItem.itemName;
        descriptionKey = cardItem.itemDescription;
    }
}
