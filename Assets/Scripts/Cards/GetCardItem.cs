using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCardItem : MonoBehaviour
{
    public Cards cardItem;

    [HideInInspector] public string nameKey;
    [HideInInspector] public string descriptionKey;

    void Awake()
    {
        nameKey = cardItem.itemName;
        descriptionKey = cardItem.itemDescription;
    }
}
