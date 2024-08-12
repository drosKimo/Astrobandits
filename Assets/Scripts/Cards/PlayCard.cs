using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard : MonoBehaviour
{
    [SerializeField] SpawnCard spawnCard;
    CharacterRole characterRole;
    [HideInInspector] public Cards playedCard;
    [HideInInspector] public Storage playStorage;

    public void Pow() // атака противника
    {
        Enemy_AI enemy_AI = GetComponent<Enemy_AI>();

        if (enemy_AI.target.gameObject.tag == "Player") // отнимает хп, если это игрок
        {
            enemy_AI.target.currentHP--;
        }
        else // позволяет противнику отреагировать на атаку
        {
            EnemyCardReaction reaction = enemy_AI.target.GetComponent<EnemyCardReaction>();
            reaction.Pow();
        }

        Debug.Log($"{gameObject.name} выстрелил в {enemy_AI.target.name}");
    }

    public void Bartender()
    {
        GameObject enemies = GameObject.Find("Enemies");
        List<GameObject> players = new List<GameObject>();

        SporeBeer(); // сначала хилит разыгравшего на 1 хп

        // ищет всех живых игроков
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            players.Add(enemies.transform.GetChild(i).gameObject);
        }

        // хилит каждого на 1 хп
        foreach (GameObject enemy in players)
        {
            CharacterRole charRole = enemy.GetComponent<CharacterRole>();
            Characters character = charRole.character;

            if (charRole.currentHP < character.characterHitPoint)
                charRole.currentHP++;
        }
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
        characterRole = GetComponent<CharacterRole>();

        characterRole.DrawCard();
        characterRole.DrawCard();
        characterRole.DrawCard();
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

    public void EnemyLootBoxes()
    {
        GameObject players = GameObject.Find("Enemies");
        
        // сразу раздает ем по карте
        for (int i = 0; i < players.transform.childCount; i++)
        {
            characterRole = players.transform.GetChild(i).GetComponent<CharacterRole>();
            characterRole.DrawCard();
        }
    }

    public void MutantDealer()
    {
        characterRole = GetComponent<CharacterRole>();

        characterRole.DrawCard();
        characterRole.DrawCard();
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
            characterRole = enemy.GetComponent<CharacterRole>();
            // добавляет врага и количество его карт в список
            players.Add(enemy.name, characterRole.hand.Count);

            // ищет подходящую карту из общего хранилища
            foreach (Cards card in characterRole.hand)
            {
                allCards.Add(card);
            }

            characterRole.hand.Clear(); // очистить список
        }

        // теперь отдаем карты обратно
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            characterRole = enemy.GetComponent<CharacterRole>();
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
                        characterRole.hand.Add(allCards[card]);
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

    public void Shredder() // только когда разыгрывает игрок
    {
        characterRole = GetComponent<CharacterRole>();

        int maxCard = characterRole.hand.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(maxCard);

        // уничтожает случайную карту на руке
        characterRole.hand.RemoveAt(index);
    }

    public void SporeBeer()
    {
        CharacterRole charRole = gameObject.GetComponent<CharacterRole>();

        if (charRole.currentHP < charRole.maxHP)
            charRole.currentHP++;
    }

    public void Turlock()
    {
        Debug.Log("Карта разыграна");
    }

    public void XenoRunt()
    {
        // хранилище карт противника
        characterRole = GetComponent<CharacterRole>();

        int maxCard = characterRole.hand.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(maxCard);

        playedCard = characterRole.hand[index];
        characterRole.hand.RemoveAt(index);

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
