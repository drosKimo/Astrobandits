using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    TurnManager manager;
    HelperData helper;
    PlayerHierarchy hierarchy;
    CharacterRole characterRole, youPlayer;
    PlayCard playCard;
    Cards stolenCard;

    List<int> index;

    bool turnEnd, implantSet;
    [HideInInspector] public bool playedPow = false, // проверка, выстрелил ли
                                  frozen = false; // бросали ли на него Криозаряд

    void Start()
    {
        manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        helper = manager.gameObject.GetComponent<HelperData>();
        hierarchy = manager.gameObject.GetComponent<PlayerHierarchy>();

        turnEnd = false; // ход не закончен
        implantSet = false;

        characterRole = GetComponent<CharacterRole>();
        playCard = GetComponent<PlayCard>();
    }

    // запуск логики противника
    public IEnumerator EnemyTurn()
    {
        if (!frozen)
        {
            StartCoroutine(EnemyPlayCard()); // противник разыгрывает карту
        }
        else
        {
            frozen = false; // сразу выключаем заморозку

            System.Random rand = new System.Random();
            int randed = rand.Next(3);
            Debug.Log($"Попытка разморозки: {randed}");

            switch (randed)
            {
                case 2:
                    StartCoroutine(EnemyPlayCard()); // противник разыгрывает карту
                    break;

                default:
                    turnEnd = true;
                    break;
            }              
        }

        yield return new WaitUntil(() => turnEnd); // ждет пока ход закончится
        yield return new WaitForSeconds(0.3f); // дополнительное ожидание перед окончанием хода

        manager.EndTurn(); // заканчивает ход
    }

    void EnemySearchOther()
    {
        // выбирает случайную цель
        System.Random rand = new System.Random();

        // временно, сейчас он должен искать кого угодно кроме себя
        target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        for (int i = 0; target.name == gameObject.name; i++)
        {
            target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        }

        Animator anim = target.gameObject.GetComponentInChildren<Animator>();
        anim.SetTrigger("TargetSet");
    }

    IEnumerator EnemyPlayCard()
    {
        index = new List<int>();
        playedPow = false;

        restart:
        // чтобы можно было добавлять карты, нужно поместить значения в .List()
        foreach (Cards card in characterRole.hand.ToList())
        {
            yield return new WaitForSeconds(0.5f);

            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull())
            {
                youPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>(); // ищет игрока
            }

            // смотрит каждую карту в своей руке и пытается ее разыграть
            switch (card.itemName)
            {
                case "Cards.Name.Pow":
                    EnemySearchOther();

                    // считает какое расстояние до цели
                    int calc = hierarchy.CalculateCircularDistance(gameObject.transform, target.gameObject.transform);

                    if (!playedPow || calc <= playCard.currentDistance) // проверяет, стрелял ли уже персонаж
                    {
                        playCard.playerDone = false; // ожидание ответа
                        playedPow = true;

                        StartCoroutine(playCard.Pow());
                        index.Add(characterRole.hand.IndexOf(card));

                        yield return new WaitUntil(() => playCard.playerDone); // дождаться ответа игрока
                        // только после этого продолжает ход
                    }
                    else
                        Debug.Log($"{gameObject.name} попытался выстрелить");
                    break;

                case "Cards.Name.Insectoids":             
                    Debug.Log($"{gameObject.name} сыграл {card.name}");
                    playCard.playerDone = false; // ожидание ответа

                    GameObject list = GameObject.Find("Enemies");
                    for (int i = 0; i < list.transform.childCount; i++)
                    {
                        GameObject currentEnemy = list.transform.GetChild(i).gameObject;
                        if (list.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // логика реакции игрока
                                target = list.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();

                                playCard.currentReactionCard = "Cards.Name.Pow"; // задает карту, которая должна использоваться чтобы не потерять хп
                                StartCoroutine(playCard.WaitForPlayer()); // запускает реакцию игрока

                                yield return new WaitUntil(() => playCard.playerDone); // дождаться ответа игрока
                            }
                            else
                            {
                                EnemyCardReaction enemyCardReaction = currentEnemy.GetComponent<EnemyCardReaction>();
                                enemyCardReaction.Insectoids();
                            }
                        }
                    }
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Armageddets":
                    Debug.Log($"{gameObject.name} сыграл {card.name}");
                    playCard.playerDone = false; // ожидание ответа

                    GameObject list2 = GameObject.Find("Enemies");
                    for (int i = 0; i < list2.transform.childCount; i++)
                    {
                        GameObject currentEnemy2 = list2.transform.GetChild(i).gameObject;
                        if (list2.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list2.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // логика реакции игрока
                                target = list2.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();

                                playCard.currentReactionCard = "Cards.Name.Dodge"; // задает карту, которая должна использоваться чтобы не потерять хп
                                StartCoroutine(playCard.WaitForPlayer()); // запускает реакцию игрока

                                yield return new WaitUntil(() => playCard.playerDone); // дождаться ответа игрока
                            }
                            else
                            {
                                EnemyCardReaction enemyCardReaction = currentEnemy2.GetComponent<EnemyCardReaction>();
                                enemyCardReaction.Armageddets();
                            }
                        }
                    }
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Bartender":
                    if (characterRole.currentHP <= characterRole.maxHP - 2)
                    {
                        Debug.Log($"{gameObject.name} сыграл {card.name}");
                        playCard.Bartender();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.SporeBeer":
                    if (characterRole.currentHP <= characterRole.maxHP - 1)
                    {
                        Debug.Log($"{gameObject.name} сыграл {card.name}");
                        playCard.SporeBeer();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.MutantDealer":
                    Debug.Log($"{gameObject.name} сыграл {card.name}");

                    playCard.MutantDealer();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // начинает разыгрывать карты заново

                case "Cards.Name.Jackpot":
                    Debug.Log($"{gameObject.name} сыграл {card.name}");

                    playCard.Jackpot();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // начинает разыгрывать карты заново

                case "Cards.Name.Shredder":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // если у игрока есть карты на руке
                    {
                        int maxCard, ind;
                        System.Random rand = new System.Random();

                        // если цель - игрок, уничтожает карту на экране и пересоздает инвентарь
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxCard = playerHand.transform.childCount - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // уничтожает случайную карту на руке
                            Destroy(playerHand.transform.GetChild(ind).gameObject); // удаляет ту же карту на экране
                        }
                        else
                        {
                            maxCard = target.hand.Count - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // уничтожает случайную карту на руке
                        }

                        Debug.Log($"{gameObject.name} уничтожил карту {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.LootBoxes":
                    Debug.Log($"{gameObject.name} сыграл {card.name}");

                    playCard.EnemyLootBoxes();
                    index.Add(characterRole.hand.IndexOf(card)); 

                    DestroyCards();
                    goto restart; // начинает разыгрывать карты заново

                case "Cards.Name.XenoRunt":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // если у игрока есть карты на руке
                    {
                        int maxcard, indexI;
                        System.Random random = new System.Random();

                        // если цель - игрок, уничтожает карту на экране и пересоздает инвентарь
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxcard = playerHand.transform.childCount - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // уничтожает случайную карту на руке
                            Destroy(playerHand.transform.GetChild(indexI).gameObject); // удаляет ту же карту на экране
                        }
                        else
                        {
                            maxcard = target.hand.Count - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // уничтожает случайную карту на руке
                        }

                        characterRole.hand.Add(stolenCard); // добавляет украденную карту в руку

                        Debug.Log($"{gameObject.name} украл карту {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));

                        DestroyCards();
                        goto restart;
                    }
                    // иначе оставляет эту карту в руке
                    break; // может начать разыгрывать карты заново

                case "Cards.Name.Reassembly":
                    Debug.Log($"{gameObject.name} сыграл {card.name}");
                    playCard.Reassembly();
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Challenge":
                    EnemySearchOther(); // ищет target
                    Debug.Log($"{gameObject.name} кинул вызов {target.name}");

                    helper.challenge_AI = gameObject.GetComponent<Enemy_AI>();
                    helper.challengeDone = false; // ожидание ответа

                    index.Add(characterRole.hand.IndexOf(card));
                    DestroyCards();

                    playCard.Challenge();

                    yield return new WaitUntil(() => helper.challengeDone); // дождаться ответа игрока
                    break;

                case "Cards.Name.Scorpion":
                    if (playCard.currentDistance <= 2)
                    {
                        Debug.Log($"{gameObject.name} взял в руки {card.name}");
                        playCard.Scorpion();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.Turlock":
                    if (playCard.currentDistance <= 3)
                    {
                        Debug.Log($"{gameObject.name} взял в руки {card.name}");
                        playCard.Turlock();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.Isabelle":
                    if (playCard.currentDistance <= 4)
                    {
                        Debug.Log($"{gameObject.name} взял в руки {card.name}");
                        playCard.Isabelle();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.PulseRifle":
                    if (playCard.currentDistance <= 5)
                    {
                        Debug.Log($"{gameObject.name} взял в руки {card.name}");
                        playCard.PulseRifle();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.CyberImplant":
                    if (!implantSet) // чтобы ИИ не мог бесконечно увеличивать свою досягаемость
                    {
                        Debug.Log($"{gameObject.name} сыграл {card.name}");
                        playCard.CyberImplant();
                        implantSet = true;
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                default:
                    Debug.Log("Противник не умеет играть эту карту");
                    break;
            }

            // проверяет, умер ли игрок
            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull() && youPlayer.currentHP <= 0)
            {
                characterRole = youPlayer.GetComponent<CharacterRole>();
                characterRole.DeadPlayer();
            }

            /*switch (card.itemName)
            {
                case "Cards.Name.Bike":
                    break;
                case "Cards.Name.Collapsar":
                    break;
                case "Cards.Name.CryoCharge":
                    break;
                case "Cards.Name.EnergyBlade":
                    break;
                case "Cards.Name.ForceField":
                    break;
            }*/
        }

        DestroyCards();
        LeaveCards();

        turnEnd = true; // ход закончен
    }

    // удаляет разыгранные карты
    void DestroyCards()
    {
        // удаляет использованные карты из руки в обратном порядке
        for (int i = index.Count; i > 0; i--)
        {
            characterRole = gameObject.GetComponent<CharacterRole>();
            characterRole.hand.RemoveAt(index[i - 1]);
        }
        index.Clear(); // очищает отбойник после удаления карт
    }

    // оставляет в руке не больше карт, чем хп у игрока
    void LeaveCards()
    {
        // сейчас убирает просто все карты с конца списка. Нужно будет добавить систему, при которой противник
        // будет оставлять у себя полезные карты
        for (int i = characterRole.hand.Count; i > characterRole.currentHP; i--)
        {
            characterRole.hand.RemoveAt(i - 1);
        }
    }
}
