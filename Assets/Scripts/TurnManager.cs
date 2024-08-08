using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> playerTurn;
    [SerializeField] GameObject blocker;
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
            blocker.SetActive(true);
        else
            blocker.SetActive(false);

        StartTurn();
    }

    void StartTurn()
    {
        CharacterRole currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
        
        // логикка хода противника
        if (currentPlayer.gameObject.tag == "Enemy")
        {
            Enemy_AI enemy_AI = currentPlayer.gameObject.GetComponent<Enemy_AI>();

            enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
        }
        else
        {
            blocker.SetActive(false);
        }
    }

    public void EndTurn()
    {
        if (playerTurn[turnIndex].tag == "Player") // Если ход у игрока, при передаче хода блокируется рука
            blocker.SetActive(true);

        if (turnIndex < playerTurn.Count - 1)
            turnIndex++;
        else
            turnIndex = 0;

        StartTurn();
    }
}