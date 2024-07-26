using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeDescription : MonoBehaviour
{
    LocalizedText localizedText;
    GetCardItem cardItem;
    void Awake()
    {
        localizedText = GetComponent<LocalizedText>();
        cardItem = GetComponentInParent<GetCardItem>();

        localizedText.LocalizationKey = cardItem.descriptionKey;
        Debug.Log(cardItem.descriptionKey);
    }
}
