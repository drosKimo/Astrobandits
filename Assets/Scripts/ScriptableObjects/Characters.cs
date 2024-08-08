using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
public class Characters : ScriptableObject
{
    [Tooltip("���� ����������� �����")] public string characterName;
    [Tooltip("���� ����������� ��������")] public string characterDescription;
    public Sprite characterImage;
    [Range(0,10)] public int characterHitPoint;
}
