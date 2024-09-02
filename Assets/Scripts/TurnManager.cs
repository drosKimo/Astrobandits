using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnManager : MonoBehaviour
{
    public List<GameObject> playerTurn;
    [SerializeField] public GameObject blocker;
    [SerializeField] public GameObject finMove;
    GameObject list;

    CharacterRole currentPlayer;
    HelperData helper;

    private int turnIndex = 0, randed;

    System.Random rand;

    void Start()
    {
        helper = gameObject.GetComponent<HelperData>();
        System.Random rand = new System.Random();

        StartCoroutine(WaitForQueue());
    }

    IEnumerator WaitForQueue()
    {
        list = GameObject.Find("Enemies");

        yield return new WaitForSeconds(0.1f);

        // каждого противника в список
        for (int i = 0; i < list.transform.childCount; i++)
        {
            playerTurn.Add(list.transform.GetChild(i).gameObject);
        }

        if (playerTurn[0].tag != "Player") // если капитан - не игрок, карты в руке заблокироавны
        {
            finMove.SetActive(false);
            blocker.SetActive(true);
            StartTurn();
        }
        else
        {
            blocker.SetActive(false);
            StartCoroutine(WaitBeforeStart()); // ожидание перед взятием карт на старте
        }
    }

    void StartTurn()
    {
        Debug.Log($"{playerTurn[turnIndex].gameObject.name} начал ход");

        if (list.transform.childCount > 1) // временно. Нужно для того, чтобы ход не продолжался если остается 1 игрок
        {
            // логика хода противника
            switch (playerTurn[turnIndex].tag) 
            {
                case "Enemy":
                    if (!currentPlayer.frozen)
                    {
                        EnemyTurn();
                    }
                    else
                    {
                        currentPlayer.frozen = false; // сразу выключаем заморозку

                        randed = rand.Next(3);
                        Debug.Log($"Попытка разморозки: {randed}");

                        switch (randed)
                        {
                            case 2:
                                EnemyTurn();
                                break;

                            default:
                                EndTurn();
                                break;
                        }
                    }
                    break;

                case "Dead":
                    if (playerTurn[turnIndex].name == "You")
                        blocker.SetActive(true);
                    EndTurn();
                    break;

                case "Player": // разблокирует руку игрока, если сейчас его ход
                    if (!currentPlayer.frozen)
                    {
                        PlayerTurn();
                    }
                    else
                    {
                        currentPlayer.frozen = false; // отключает заморозку

                        randed = rand.Next(3);
                        Debug.Log($"Попытка разморозки: {randed}");

                        switch (randed)
                        {
                            case 2:
                                PlayerTurn();
                                break;

                            default:
                                EndTurn();
                                break;
                        }
                    }
                    break;
            }
        }
    }

    void PlayerTurn()
    {
        // дает в руку 2 карты в начале хода
        currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
        currentPlayer.DrawCard();
        currentPlayer.DrawCard();

        finMove.SetActive(true);
        blocker.SetActive(false);

        helper.shotDone = false;
    }

    void EnemyTurn()
    {
        // дает в руку 2 карты в начале хода
        currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
        currentPlayer.DrawCard();
        currentPlayer.DrawCard();

        Enemy_AI enemy_AI = playerTurn[turnIndex].GetComponent<Enemy_AI>();
        enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
    }

    public void EndTurn()
    {
        if (playerTurn[turnIndex].tag == "Player") // Если ход был у игрока, при передаче хода блокируется рука
        {
            finMove.SetActive(false);
            blocker.SetActive(true);
        }

        // проверка, чтобы индекс игрока не превышал лимиты
        if (turnIndex < playerTurn.Count - 1)
            turnIndex++;
        else
            turnIndex = 0;

        StartTurn();
    }

    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(1f);
        StartTurn();
        yield return null;
    }
}