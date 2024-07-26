using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Event")]
public class Events : ScriptableObject
{
    [Tooltip("���� ����������� ��������")] public string eventName;
    [Tooltip("���� ����������� ��������")] public string eventDescription;
    public Sprite eventImage;
}