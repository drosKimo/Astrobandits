using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
public class Characters : ScriptableObject
{
    [Tooltip("Ключ локализации имени")] public string characerName;
    [Tooltip("Ключ локализации описания")] public string characerDescription;
    public Sprite characerImage;
    [Range(0,10)] public int characerHitPoint;
}
