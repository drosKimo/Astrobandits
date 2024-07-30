using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard : MonoBehaviour
{
    [SerializeField] SpawnCard spawnCard;
    EnemyCard enemyCard;
    [HideInInspector] public Cards playedCard;
    [HideInInspector] public Storage playStorage;

    public void Bartender()
    {
        Debug.Log("Карта разыграна");
    }

    public void Bike()
    {
        Debug.Log("Карта разыграна");
    }

    public void Challenge()
    {
        Debug.Log("Карта разыграна");
    }

    public void Collapsar()
    {
        Debug.Log("Карта разыграна");
    }

    public void CryoCharge()
    {
        Debug.Log("Карта разыграна");
    }

    public void CyberImplant()
    {
        Debug.Log("Карта разыграна");
    }

    public void Dodge()
    {
        Debug.Log("Карта разыграна");
    }

    public void EnergyBlade()
    {
        Debug.Log("Карта разыграна");
    }

    public void ForceField()
    {
        Debug.Log("Карта разыграна");
    }

    public void Isabelle()
    {
        Debug.Log("Карта разыграна");
    }

    public void Jackpot()
    {
        spawnCard.Spawn();
        spawnCard.Spawn();
        spawnCard.Spawn();
    }

    public void LootBoxes()
    {
        Debug.Log("Карта разыграна");
    }

    public void MutantDealer()
    {
        spawnCard.Spawn();
        spawnCard.Spawn();
    }

    public void PulseRifle()
    {
        Debug.Log("Карта разыграна");
    }

    public void Reassembly()
    {
        List<Cards> allCards = new List<Cards>(); // все карты в руке
        Dictionary<string, int> players = new Dictionary<string, int>(); // игрок и сколько у него карт
        
        // проверка для каждого из врагов
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCard = enemy.GetComponent<EnemyCard>();
            // добавляет врага и количество его карт в список
            players.Add(enemy.name, enemyCard.enemyCards.Count);

            // ищет подходящую карту из общего хранилища
            foreach (Cards card in enemyCard.enemyCards)
            {
                //Debug.Log($"{card.itemName}");
                allCards.Add(card);
            }

            enemyCard.enemyCards.Clear(); // очистить список
        }

        // теперь то же самое для игрока
        GameObject playerHand = GameObject.Find("Elements Container");
        players.Add(gameObject.name, playerHand.transform.childCount); // добавляет игрока в список игроков

        for (int i = 0; i < playerHand.transform.childCount; i++)
        {
            // ищет каждую карту в руке
            GetCardItem getCard = playerHand.transform.GetChild(i).GetComponent<GetCardItem>();
            allCards.Add(getCard.cardItem); // добавляет в список
            Destroy(playerHand.transform.GetChild(i).gameObject); // удаляет карту
        }

        // проверять через точку остановки
        // TODO: обратная выдача карт
        Debug.Log(allCards);
        Debug.Log(players);

        //Debug.Log("Карта разыграна");
    }

    public void Scorpion()
    {
        Debug.Log("Карта разыграна");
    }

    public void Shredder()
    {
        enemyCard = GetComponent<EnemyCard>();

        int minCard = 0;
        int maxCard = enemyCard.enemyCards.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(minCard, maxCard);
        
        // уничтожает случайную карту на руке
        enemyCard.enemyCards.RemoveAt(index);
    }

    public void SporeBeer()
    {
        Debug.Log("Карта разыграна");
    }

    public void Turlock()
    {
        Debug.Log("Карта разыграна");
    }

    public void XenoRunt()
    {
        // хранилище карт противника
        enemyCard = GetComponent<EnemyCard>();

        int minCard = 0;
        int maxCard = enemyCard.enemyCards.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(minCard, maxCard);

        playedCard = enemyCard.enemyCards[index];
        enemyCard.enemyCards.RemoveAt(index);

        // ищет подходящую карту из общего хранилища
        foreach (Cards card in playStorage.allCards)
        {
            if (card.itemName == playedCard.itemName)
            {
                index = playStorage.allCards.IndexOf(card);
            }
        }

        spawnCard.SpawnByIndex(index); // и спавнит
    }
}
