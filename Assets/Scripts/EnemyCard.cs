using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    SpawnCard spawnCard;
    GetCardItem cardItem;
    public List<Cards> enemyCards;

    public void GetEnemyCard()
    {
        spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>();
        spawnCard.Spawn(); // создать новую карту, которая окажется в инвентаре врага
        cardItem = spawnCard.newItem.GetComponent<GetCardItem>();
        enemyCards.Add(cardItem.cardItem); // добавляет эту карту в список
        Destroy(cardItem.gameObject); // удаляет сам объект, чтобы его не было в руке у игрока
    }
}
