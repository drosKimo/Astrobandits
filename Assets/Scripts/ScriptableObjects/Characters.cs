using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
public class Characters : ScriptableObject
{
    public string characerName, characerDescription;
    public Sprite characerImage;
    [Range(0,6)] public int characerHitPoint;
}
