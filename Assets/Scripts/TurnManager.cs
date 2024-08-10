using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> playerTurn;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject finMove;
    GameObject list;

    private int turnIndex = 0;

    void Start()
    {
        /*list = GameObject.Find("Enemies");

        // каждого противника в список
        for (int i = 0; i < list.transform.childCount; i++)
        {
            playerTurn.Add(list.transform.GetChild(i).gameObject);
        }*/

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
        if (list.transform.childCount > 1)
        {
            // логикка хода противника
            if (playerTurn[turnIndex].tag == "Enemy")
            {
                // дает в руку 2 карты в начале хода
                CharacterRole currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
                currentPlayer.DrawCard();
                currentPlayer.DrawCard();

                Enemy_AI enemy_AI = playerTurn[turnIndex].GetComponent<Enemy_AI>();
                enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
            }
            else if (playerTurn[turnIndex].tag == "Dead")
            {
                if (playerTurn[turnIndex].name == "You")
                    blocker.SetActive(true);

                EndTurn();
            }
            else // разблокирует руку игрока, если сейчас его ход
            {
                // дает в руку 2 карты в начале хода
                CharacterRole currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
                currentPlayer.DrawCard();
                currentPlayer.DrawCard();

                finMove.SetActive(true);
                blocker.SetActive(false);
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log(turnIndex);
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