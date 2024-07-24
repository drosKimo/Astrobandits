using UnityEngine;

[ CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card") ]
public class Cards : ScriptableObject
{
    public string itemName, itemDescription;
    public Sprite itemImage;
    public dropDown itemProperty = dropDown.Default; // свойство карты
    public suits itemSuit = suits.Default; // масть карты

    public enum dropDown 
    { 
        Default, 
        Heal, 
        Damage,
        Defence,
        Weapon
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
