using Assets.SimpleLocalization.Scripts;
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
    }
}
