using System;
using System.Collections.Generic;
using UnityEngine;

// ���������, � ������� �������� ������ � ���������, ���� ������ � ��� ������
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP, maxHP;
    public Roles role;

    public List<Cards> hand;
    public List<StatusEffect> statusEffects;

    [HideInInspector] public GetCardItem getCardItem;
    [HideInInspector] public bool frozen, // ������� �� �� ��������� ���������
                                  implantSet; // ����� �� �� ��������� ������������

    public enum StatusEffect
    {
        Null,
        CryoCharge,
        CyberImplant,
        ForceField,
        Collapsar,
        Glitch
    }

    void Start()
    {
        frozen = false;
        implantSet = false;

        // ��������� ��������
        statusEffects.Add(StatusEffect.Glitch);
        Debug.Log(statusEffects[0]);
    }

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
