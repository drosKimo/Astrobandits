using Akassets.SmoothGridLayout;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // спавнит столько карт, сколько живых игроков
        for (int i = 0; i < enemies.Length + 1; i++)
        {
            spawnCard.Spawn();
            GameObject card = spawnCard.newItem.gameObject;
            GameObject LBgrid = GameObject.Find("LootBoxes Container");
            GetCardItem item = card.GetComponent<GetCardItem>();

            // включает компонент кнопки
            Button button = card.GetComponent<Button>();
            button.enabled = true;

            // отключает возможность тянуть карту
            DragScript dragScript = card.GetComponent<DragScript>();
            dragScript.enabled = false;

            // отключает скрипт, поднимающий карту
            ShowCard showCard = card.GetComponent<ShowCard>();
            showCard.enabled = false;

            // передвигает появившиеся карты в отдельный контейнер
            item.transform.SetParent(LBgrid.transform);
            item.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
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
        spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>(); // скрипт спавна карт

        // проверка для каждого из врагов
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCard = enemy.GetComponent<EnemyCard>();
            // добавляет врага и количество его карт в список
            players.Add(enemy.name, enemyCard.enemyCards.Count);

            // ищет подходящую карту из общего хранилища
            foreach (Cards card in enemyCard.enemyCards)
            {
                allCards.Add(card);
            }

            enemyCard.enemyCards.Clear(); // очистить список
        }

        // теперь отдаем карты обратно
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCard = enemy.GetComponent<EnemyCard>();
            System.Random rand = new System.Random();

            // проверяет весь список персонажей
            foreach (KeyValuePair<string, int> pair in players)
            {
                if (enemy.name == pair.Key)
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        int max = allCards.Count;
                        int card = rand.Next(0, max);
                        enemyCard.enemyCards.Add(allCards[card]);
                        allCards.RemoveAt(card);
                    }

                    players.Remove(pair.Key); // удаляет текущего персонажа из списка
                    break;
                }
            }
        }
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
