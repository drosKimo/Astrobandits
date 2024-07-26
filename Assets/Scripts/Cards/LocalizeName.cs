using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeName : MonoBehaviour
{
    LocalizedText localizedText;
    GetCardItem cardItem;
    void Awake()
    {
        localizedText = GetComponent<LocalizedText>();
        cardItem = GetComponentInParent<GetCardItem>();

        localizedText.LocalizationKey = cardItem.nameKey;
        Debug.Log(cardItem.nameKey);
    }
}
