using UnityEngine;

[ CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card") ]
public class Cards : ScriptableObject
{
    [Header("Базовые свойства")]
    [Tooltip("Ключ локализации названия")] public string itemName;
    [Tooltip("Ключ локализации описания")] public string itemDescription;
    public Sprite itemImage;
    [Space]
    [Header("Масть карты")]
    [Tooltip("Свойство карты. По умолчанию это карта действия")] public dropDown itemProperty = dropDown.Default; // свойство карты
    [Tooltip("Масть карты")] public suits itemSuit = suits.Default; // масть карты
    [Tooltip("Номер карты. Указывать 2-10, J, Q, K, A")] public string itemNumber; // Может быть 2-10, J, Q, K, A
    [Space]
    [Header("Bool свойства")]
    [Tooltip("Можно сыграть на себя?")] public bool itemSelf = false;
    [Tooltip("Можно сыграть на другого игрока?")] public bool itemOther = false;
    [Tooltip("Это оружие?")] public bool itemIsWeapon = false;

    public enum dropDown 
    { 
        Default, // карта действия
        Blue // карта, которую кладут перед собой
    }

    public enum suits
    {
        Default,
        Spades, // пики
        Hearts, // червы
        Clubs, // трефы
        Diamonds // бубны
    }
}
