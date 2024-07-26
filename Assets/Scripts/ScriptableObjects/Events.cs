using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Event")]
public class Events : ScriptableObject
{
    [Tooltip("Ключ локализации названия")] public string eventName;
    [Tooltip("Ключ локализации описания")] public string eventDescription;
    public Sprite eventImage;
}