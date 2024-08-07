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

        // ������� ���������� � ������
        for (int i = 0; i < list.transform.childCount; i++)
        {
            playerTurn.Add(list.transform.GetChild(i).gameObject);
        }

        StartTurn();

        if (playerTurn[0].tag != "Player") // ���� ������� - �� �����, ����� � ���� �������������
            blocker.SetActive(true);
        else
            blocker.SetActive(false);
    }

    void StartTurn()
    {
        CharacterRole currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
    }
}