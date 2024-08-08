using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> playerTurn;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject finMove;
    private int turnIndex = 0;

    void Start()
    {
        GameObject list = GameObject.Find("Enemies");

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
            StartCoroutine(WaitBeforeStart()); // ожижание перед взятием карт на старте
        }
    }

    void StartTurn()
    {
        // дает в руку 2 карты в начале хода
        CharacterRole currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
        currentPlayer.DrawCard();
        currentPlayer.DrawCard();

        // логикка хода противника
        if (playerTurn[turnIndex].tag == "Enemy")
        {
            Enemy_AI enemy_AI = playerTurn[turnIndex].GetComponent<Enemy_AI>();
            enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
        }
        else // разблокирует руку игрока, если сейчас его ход
        {
            finMove.SetActive(true);
            blocker.SetActive(false);
        }
    }

    public void EndTurn()
    {
        if (playerTurn[turnIndex].tag == "Player") // Если ход у игрока, при передаче хода блокируется рука
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