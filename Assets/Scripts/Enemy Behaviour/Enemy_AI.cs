using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    TurnManager manager;
    CharacterRole characterRole, youPlayer;
    PlayCard playCard;
    Cards stolenCard;

    List<int> index;

    bool turnEnd, playedPow = false;

    // запуск логики противника
    public IEnumerator EnemyTurn()
    {
        manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        characterRole = GetComponent<CharacterRole>();
        playCard = GetComponent<PlayCard>();

        turnEnd = false; // ход не закончен

        // противник разыгрывает карту
        StartCoroutine(EnemyPlayCard());

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
                // TODO: поставить ограничение по выстрелам через булевый маркер
                case "Cards.Name.Pow":
                    if (!playedPow) // проверяет, стрелял ли уже персонаж
                    {
                        playCard.playerDone = false; // ожидание ответа
                        playedPow = true;
                        EnemySearchOther();
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

                    TurnManager turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
                    turnManager.challenge_AI = gameObject.GetComponent<Enemy_AI>();

                    turnManager.challengeDone = false; // ожидание ответа
                    playCard.Challenge();

                    index.Add(characterRole.hand.IndexOf(card));
                    yield return new WaitUntil(() => turnManager.challengeDone); // дождаться ответа игрока

                    // флажок изменен, но серия все еще прерывается после первой реакции игрока
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
                case "Cards.Name.CyberImplant":
                    break;
                case "Cards.Name.EnergyBlade":
                    break;
                case "Cards.Name.ForceField":
                    break;
                case "Cards.Name.Isabelle":
                    break;
                case "Cards.Name.PulseRifle":
                    break;
                case "Cards.Name.Scorpion":
                    break;
                case "Cards.Name.Turlock":
                    break;
            }*/
        }

        DestroyCards();
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
}
