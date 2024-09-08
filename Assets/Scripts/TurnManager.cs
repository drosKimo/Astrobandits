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

    public int turnIndex = 0;

    void Start()
    {
        helper = gameObject.GetComponent<HelperData>();
        StartCoroutine(WaitForQueue());
    }

    IEnumerator WaitForQueue()
    {
        list = GameObject.Find("Enemies");

        yield return new WaitForSeconds(0.1f);

        // ������� ���������� � ������
        for (int i = 0; i < list.transform.childCount; i++)
        {
            playerTurn.Add(list.transform.GetChild(i).gameObject);
        }

        if (playerTurn[0].tag != "Player") // ���� ������� - �� �����, ����� � ���� �������������
        {
            finMove.SetActive(false);
            blocker.SetActive(true);
            StartTurn();
        }
        else
        {
            blocker.SetActive(false);
            StartCoroutine(WaitBeforeStart()); // �������� ����� ������� ���� �� ������
        }
    }

    void StartTurn()
    {
        if (list.transform.childCount > 1) // ��������. ����� ��� ����, ����� ��� �� ����������� ���� �������� 1 �����
        {
            currentPlayer = playerTurn[turnIndex].GetComponent<CharacterRole>();
            Debug.Log($"{playerTurn[turnIndex].gameObject.name} ����� ���");

            // ������ ���� ����������
            switch (playerTurn[turnIndex].tag) 
            {
                case "Enemy":
                    if (!currentPlayer.frozen)
                    {
                        EnemyTurn();
                    }
                    else
                    {
                        currentPlayer.frozen = false; // ����� ��������� ���������

                        System.Random rand = new System.Random();
                        int randed = rand.Next(3);
                        Debug.Log($"������� ����������: {randed}");

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

                case "Player": // ������������ ���� ������, ���� ������ ��� ���
                    if (!currentPlayer.frozen)
                    {
                        PlayerTurn();
                    }
                    else
                    {
                        currentPlayer.frozen = false; // ��������� ���������

                        System.Random rand = new System.Random();
                        int randed = rand.Next(3);
                        Debug.Log($"������� ����������: {randed}");

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
        // ���� � ���� 2 ����� � ������ ����
        currentPlayer.DrawCard();
        currentPlayer.DrawCard();

        finMove.SetActive(true);
        blocker.SetActive(false);

        helper.shotDone = false;
    }

    void EnemyTurn()
    {
        // ���� � ���� 2 ����� � ������ ����
        currentPlayer.DrawCard();
        currentPlayer.DrawCard();

        Enemy_AI enemy_AI = playerTurn[turnIndex].GetComponent<Enemy_AI>();
        enemy_AI.StartCoroutine(enemy_AI.EnemyTurn());
    }

    public void EndTurn()
    {
        if (playerTurn[turnIndex].tag == "Player") // ���� ��� ��� � ������, ��� �������� ���� ����������� ����
        {
            finMove.SetActive(false);
            blocker.SetActive(true);
        }

        // ��������, ����� ������ ������ �� �������� ������
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