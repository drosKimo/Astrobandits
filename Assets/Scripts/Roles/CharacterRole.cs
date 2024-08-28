using System.Collections.Generic;
using UnityEngine;

// ���������, � ������� �������� ������ � ���������, ���� ������ � ��� ������
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP, maxHP;
    public Roles role;

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

    public void DeadPlayer()
    {
        GameObject deads = GameObject.Find("Deads");
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();


        gameObject.transform.SetParent(deads.transform);
        gameObject.tag = "Dead";
        
        sp.enabled = false; // ��������� ������ ���������

        Debug.Log($"{gameObject.name} �����");
    }
}
