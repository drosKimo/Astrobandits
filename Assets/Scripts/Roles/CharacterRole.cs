using System.Collections.Generic;
using UnityEngine;

// хранилище, в котором хранятся данные о персонаже, роли игрока и его картах
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
