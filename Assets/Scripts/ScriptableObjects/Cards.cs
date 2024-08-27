using UnityEngine;

[ CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card") ]
public class Cards : ScriptableObject
{
    [Header("������� ��������")]
    [Tooltip("���� ����������� ��������")] public string itemName;
    [Tooltip("���� ����������� ��������")] public string itemDescription;
    public Sprite itemImage;
    [Space]
    [Header("����� �����")]
    [Tooltip("�������� �����. �� ��������� ��� ����� ��������")] public dropDown itemProperty = dropDown.Default; // �������� �����
    [Tooltip("����� �����")] public suits itemSuit = suits.Default; // ����� �����
    [Tooltip("����� �����. ��������� 2-10, J, Q, K, A")] public string itemNumber; // ����� ���� 2-10, J, Q, K, A
    [Space]
    [Header("Bool ��������")]
    [Tooltip("����� ������� ������ �� ������� ������?")] public bool itemOther = false;
    [Tooltip("��� ����� ������� �� ����?")] public bool itemBoard = false;
    [Tooltip("��� ����� ������� ������ ��� �������?")] public bool itemNull = false;

    public enum dropDown 
    { 
        Default, // ����� ��������
        Blue // �����, ������� ������ ����� �����
    }

    public enum suits
    {
        Default,
        Spades, // ����
        Hearts, // �����
        Clubs, // �����
        Diamonds // �����
    }
}
