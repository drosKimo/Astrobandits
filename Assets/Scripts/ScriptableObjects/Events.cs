using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Event")]
public class Events : ScriptableObject
{
    public string itemName, itemDescription;
    public Sprite itemImage;
}
