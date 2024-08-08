using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
public class Characters : ScriptableObject
{
    [Tooltip("Ключ локализации имени")] public string characterName;
    [Tooltip("Ключ локализации описания")] public string characterDescription;
    public Sprite characterImage;
    [Range(0,10)] public int characterHitPoint;
}
