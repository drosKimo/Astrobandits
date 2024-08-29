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

    private int turnIndex = 0;

    // для CardProperty
    [HideInInspector] public bool isChallenge = false, challengeDone, shotDone; // shotDone = ограничитель для игрока
    [HideInInspector] public Enemy_AI challenge_AI;

    void Start()
    {
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

        if (list.transform.childCount > 1) // временно. Нужно для того, чтобы ход не продолжался если остается 1 игрок+
        {
            // логика хода противника
            switch (playerTurn[turnIndex].tag) 
            {
                case "Enemy":
                    // дает в руку 2 карты в начале хода
                    currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
                    currentPlayer.DrawCard();
                    currentPlayer.DrawCard();

                    Enemy_AI enemy_AI = playerTurn[turnIndex].GetComponent<Enemy_AI>();
                    enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
                    break;

                case "Dead":
                    if (playerTurn[turnIndex].name == "You")
                        blocker.SetActive(true);
                    EndTurn();
                    break;

                case "Player": // разблокирует руку игрока, если сейчас его ход
                    // дает в руку 2 карты в начале хода
                    currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
                    currentPlayer.DrawCard();
                    currentPlayer.DrawCard();

                    finMove.SetActive(true);
                    blocker.SetActive(false);

                    shotDone = false;
                    break;
            }
        }
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