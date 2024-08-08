using System.Collections.Generic;
using UnityEngine;

// ���������, � ������� �������� ������ � ���������, ���� ������ � ��� ������
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP;
    public Roles role;
    [HideInInspector][Tooltip("����� ��������� � �������")][Range(0, 6)] public int queueNumber;

    public List<Cards> hand;

    [HideInInspector] public GetCardItem getCardItem;

    public void DrawCard()
    {
        SpawnCard spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>();
        spawnCard.Spawn(); // ������� ����� �����, ������� �������� � ���������
        getCardItem = spawnCard.newItem.GetComponent<GetCardItem>();
        hand.Add(getCardItem.cardItem); // ��������� ��� ����� � ������

        if (gameObject.tag != "Player") // ���� ��� �� �����
            Destroy(getCardItem.gameObject); // ������� ��� ������, ����� ��� �� ���� � ���� � ������
    }
}
