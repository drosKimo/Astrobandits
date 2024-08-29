using UnityEngine;

[ CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card") ]
public class Cards : ScriptableObject
{
    [Header("Базовые свойства")]
    [Tooltip("Ключ локализации названия")] public string itemName;
    [Tooltip("Ключ локализации описания")] public string itemDescription;
    public Sprite itemImage;
    [Space]
    [Header("Свойства карты")]
    [Tooltip("Свойство карты. По умолчанию это карта действия")] public dropDown itemProperty = dropDown.Default; // свойство карты
    [Space]
    [Header("Bool свойства")]
    [Tooltip("Можно сыграть только на другого игрока?")] public bool itemOther = false;
    [Tooltip("Это можно сыграть на поле?")] public bool itemBoard = false;
    [Tooltip("Это можно сыграть только как реакцию?")] public bool itemNull = false;

    public enum dropDown 
    { 
        Default, // карта действия
        Blue // карта, которую кладут перед собой
    }
}
