using System;
using System.Collections.Generic;
using UnityEngine;

// хранилище, в котором хранятся данные о персонаже, роли игрока и его картах
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP, maxHP;
    public Roles role;

    public List<Cards> hand;
    public List<StatusEffect> statusEffects;

    [HideInInspector] public GetCardItem getCardItem;
    [HideInInspector] public bool frozen, // сыграли ли на персонажа Криозаряд
                                  implantSet; // надет ли на персонажа Киберимплант

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

        // временною проверка
        statusEffects.Add(StatusEffect.Glitch);
        Debug.Log(statusEffects[0]);
    }

    public void DrawCard()
    {
        SpawnCard spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>();
        spawnCard.Spawn(); // создать новую карту, которая окажется в инвентаре
        getCardItem = spawnCard.newItem.GetComponent<GetCardItem>();
        hand.Add(getCardItem.cardItem); // добавляет эту карту в список

        if (gameObject.tag != "Player") // если это не игрок
            Destroy(getCardItem.gameObject); // удаляет сам объект, чтобы его не было в руке у игрока
    }

    public void DeadPlayer()
    {
        GameObject deads = GameObject.Find("Deads");
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();

        gameObject.transform.SetParent(deads.transform);
        gameObject.tag = "Dead";
        
        sp.enabled = false; // выключает спрайт персонажа

        Debug.Log($"{gameObject.name} погиб");
    }
}
