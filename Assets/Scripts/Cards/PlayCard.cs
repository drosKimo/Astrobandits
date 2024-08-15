using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard : MonoBehaviour
{
    [SerializeField] SpawnCard spawnCard;
    [HideInInspector] public Cards playedCard;
    [HideInInspector] public Storage playStorage;

    CharacterRole characterRole;
    Enemy_AI enemy_AI;

    bool dodged = false;
    [HideInInspector] public bool playerDone;
    [HideInInspector] public string currentReactionCard;

    public IEnumerator Pow() // атака противника
    {
        enemy_AI = GetComponent<Enemy_AI>();
        playerDone = false; // ожидание ответа

        if (enemy_AI.target.gameObject.tag == "Player") // отнимает хп, если это игрок
        {
            // как вариант, при создании расстояния между игроками опираться на их положение в иерархии
            // в случае, если индекс выходит за рамки, нужно начать с конца
            // пример: если карты закончились снизу, в счет добавляются карты с верха иерархии

            // не думаю, что нужны ограничения по нижней планке, поскольку, если значения дублируются, противник все равно
            // до них достанет, но лучше удалять дубликаты, чтобы не повышать им шансы на маслину

            currentReactionCard = "Cards.Name.Dodge"; // задает карту, которая должна использоваться чтобы не потерять хп
            StartCoroutine(WaitForPlayer()); // запускает реакцию игрока
        }
        else // позволяет противнику отреагировать на атаку
        {
            EnemyCardReaction reaction = enemy_AI.target.GetComponent<EnemyCardReaction>();
            reaction.Pow();
            playerDone = true;
        }

        Debug.Log($"{gameObject.name} выстрелил в {enemy_AI.target.name}");
        yield return new WaitUntil(() => playerDone); // дождаться ответа игрока
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

            if (charRole.currentHP < charRole.maxHP) // ddd
                charRole.currentHP++;
        }
    }

    public void Bike()
    {
        Debug.Log("Карта разыграна");
    }

    public void Challenge()
    {
        // аналогично с действиями с выстрелом, но нужно сделать так, чтобы все взаимодействия
        // заключались в цикличную корутину для каждого

        // как вариант, можно сделать так, чтобы игрок мог разыгрывать только нужные карты в обоих случаях

        Debug.Log("Карта разыграна");
    }

    public void Collapsar()
    {
        // начать детальную проработку только после добавления окошка для статус-эффектов
        // можно начать чуть раньше, записывая результат в дебаг

        // нужно чтобы она активировалась на втором круге, играется на себя, должна иметь приоритет на удаление
        // у противников

        Debug.Log("Карта разыграна");
    }

    public void CryoCharge()
    {
        // аналогично с Коллапсаром, только при добавлении проверок

        // противники не должны иметь отрицательный приоритет на удаление этого статус-эффекта

        Debug.Log("Карта разыграна");
    }

    public void CyberImplant()
    {
        Debug.Log("Карта разыграна");
    }

    public void EnergyBlade()
    {
        // дальность 1, позвроляет играть Тыщ несколько раз

        Debug.Log("Карта разыграна");
    }

    public void ForceField()
    {
        Debug.Log("Карта разыграна");
    }

    public void Isabelle()
    {
        // дальность 4

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
        
        // сразу раздает всем по карте
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
        // дальность 5
        // скорее всего, придется временно убрать, поскольку при игре с 5 игроками максимальное расстояние = 4
        // (без улучшений - 2)

        Debug.Log("Карта разыграна");
    }

    public void Reassembly()
    {
        System.Random rand = new System.Random();
        List<Cards> allCards = new List<Cards>(); // все карты из рук персонажей
        Dictionary<string, int> players = new Dictionary<string, int>(); // игрок и сколько у него карт

        // проверка для каждого из врагов
        foreach (CharacterRole enemy in GameObject.Find("Enemies").transform.GetComponentsInChildren<CharacterRole>())
        {
            // если текущий персонаж - не разыгравший карту и у него на руках есть карты
            if (enemy.gameObject.name != gameObject.name && enemy.hand.Count > 0)
            {
                // добавляет врага и количество его карт в список
                players.Add(enemy.gameObject.name, enemy.hand.Count);

                // ищет подходящую карту из общего хранилища
                foreach (Cards card in enemy.hand)
                {
                    allCards.Add(card);
                }

                if (enemy.gameObject.tag == "Player")
                {
                    GameObject container = GameObject.Find("Elements Container");
                    
                    for (int i = container.transform.childCount; i > 0; i--) // очистить экран
                    {
                        Destroy(container.transform.GetChild(i - 1).gameObject);
                    }

                    enemy.hand.Clear(); // очистить руку

                }
                else
                    enemy.hand.Clear(); // очистить руку
            }
        }

        // теперь отдаем карты обратно
        foreach (CharacterRole enemy in GameObject.Find("Enemies").transform.GetComponentsInChildren<CharacterRole>())
        {
            // проверяет весь список персонажей
            foreach (KeyValuePair<string, int> pair in players)
            {
                if (enemy.gameObject.name == pair.Key) // проверяет, совпадает ли текущий в очереди персонаж со значением
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        int max = allCards.Count;
                        int card = rand.Next(max);

                        if (enemy.gameObject.tag == "Player") // выводит карты на экран для игрока
                        {
                            // получает общее хранилище
                            GetCardItem cardItem = spawnCard.SCprefab.GetComponent<GetCardItem>();
                            playStorage = cardItem.globalStorage;

                            // спавнит карту по индексу
                            cardItem.cardIndex = playStorage.allCards.IndexOf(allCards[card]);
                            cardItem.calling = false;
                            spawnCard.newItem = GameObject.Instantiate(spawnCard.SCprefab);

                            // получает имя для карты, чтобы не было ошибок с взаимодействием с картами
                            spawnCard.GetItemName();
                        }

                        enemy.hand.Add(allCards[card]); // добавляет выданную карту в руку
                        allCards.RemoveAt(card); // удаляет выданную карту из списка
                    }

                    players.Remove(pair.Key); // удаляет текущего персонажа из списка
                    break;
                }
            }
        }
    }

    public void Scorpion()
    {
        // дальность 2

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
        // дальность 3

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

    public IEnumerator WaitForPlayer()
    {
        enemy_AI = GetComponent<Enemy_AI>();
        playerDone = false; // ожидание ответа

        yield return new WaitForSeconds(0.3f); // небольшая задержка перед ходом

        // проверяет, есть ли у игрока хотя бы 1 нужная в этой ситуации карта
        foreach (Cards card in enemy_AI.target.hand)
        {
            if (card.itemName == currentReactionCard)
                dodged = true;
        }

        // пытается понять, есть ли у игрока возможность отреагировать
        if (dodged)
        {
            // нужно добавить что-то, что будет сигнализировать о том, что игрок может отреагировать
            Debug.Log("Ожидание реакции игрока");

            Player_CanMove();
        }
        else
        {
            enemy_AI.target.currentHP--;
            playerDone = true; // игрок ответил
            Debug.Log("Игрок потерял хп");
        }
    }

    void Player_CanMove()
    {
        // отключает блокировку карт
        TurnManager turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        turnManager.blocker.SetActive(false);

        // блокирует возможность вытянуть карту и включает кнопку
        GameObject container = GameObject.Find("Elements Container");
        for (int i = 0; i < container.transform.childCount; i++)
        {
            DragScript dragCard = container.transform.GetChild(i).GetComponent<DragScript>();
            Button buttonCard = dragCard.gameObject.GetComponent<Button>();
            CardProperty cardProperty = dragCard.gameObject.GetComponent<CardProperty>();
            GetCardItem thisCard = dragCard.gameObject.GetComponent<GetCardItem>();

            dragCard.enabled = false;
            buttonCard.enabled = true;

            // передает карте данные о выстрелившем противнике карте
            cardProperty.enemyObj = gameObject;

            // проверяет, что мы нажали именно ту карту, которая нам нужна
            if (thisCard.nameKey == currentReactionCard)
                cardProperty.cardNeeded = true;
        }
    }
}
