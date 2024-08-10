using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    TurnManager manager;
    CharacterRole characterRole, youPlayer;
    PlayCard playCard;

    bool turnEnd;

    // ������ ������ ����������
    public IEnumerator EnemyTurn()
    {
        manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        characterRole = GetComponent<CharacterRole>();
        playCard = GetComponent<PlayCard>();

        turnEnd = false; // ��� �� ��������

        // ��������� ����������� �����
        StartCoroutine(EnemyPlayCard());

        yield return new WaitUntil(() => turnEnd); // ���� ���� ��� ����������
        yield return new WaitForSeconds(0.3f); // �������������� �������� ����� ���������� ����

        manager.EndTurn(); // ����������� ���
    }

    IEnumerator EnemyPlayCard()
    {
        List<int> index = new List<int>();

        foreach (Cards card in characterRole.hand)
        {
            yield return new WaitForSeconds(0.5f);

            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull())
            {
                youPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>(); // ���� ������
            }

            // ������� ������ ����� � ����� ���� � �������� �� ���������
            switch (card.itemName)
            {
                case "Cards.Name.Pow":
                    // �������� ��������� ����
                    System.Random rand = new System.Random();

                    // ��������, ������ �� ������ ������ ���� ������ ����� ����
                    target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
                    for (int i = 0; target.name == gameObject.name; i++)
                    {
                        target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
                    }

                    playCard.Pow();
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Insectoids":             
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    GameObject list = GameObject.Find("Enemies");
                    for (int i = 0; i < list.transform.childCount; i++)
                    {
                        GameObject currentEnemy = list.transform.GetChild(i).gameObject;
                        if (list.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // ������ ������� ������
                                CharacterRole charRolePlayer = list.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();
                                charRolePlayer.currentHP--;
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
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    GameObject list2 = GameObject.Find("Enemies");
                    for (int i = 0; i < list2.transform.childCount; i++)
                    {
                        GameObject currentEnemy2 = list2.transform.GetChild(i).gameObject;
                        if (list2.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list2.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // ������ ������� ������
                                CharacterRole charRolePlayer = list2.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();
                                charRolePlayer.currentHP--;
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

                default:
                    Debug.Log("��������� �� ����� ������ ��� �����");
                    break;
            }

            // ���������, ���� �� �����
            if (youPlayer.currentHP <= 0)
            {
                characterRole = youPlayer.GetComponent<CharacterRole>();
                characterRole.DeadPlayer();
            }

            /*switch (card.itemName)
            {
                case "Cards.Name.Bartender":
                    break;
                case "Cards.Name.Bike":
                    break;
                case "Cards.Name.Challenge":
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
                case "Cards.Name.Jackpot":
                    break;
                case "Cards.Name.LootBoxes":
                    break;
                case "Cards.Name.MutantDealer":
                    break;
                case "Cards.Name.PulseRifle":
                    break;
                case "Cards.Name.Reassembly":
                    break;
                case "Cards.Name.Scorpion":
                    break;
                case "Cards.Name.Shredder":
                    break;
                case "Cards.Name.SporeBeer":
                    break;
                case "Cards.Name.Turlock":
                    break;
                case "Cards.Name.XenoRunt":
                    break;
            }*/
        }

        // ������� �������������� ����� �� ����
        for (int i = 0; i < index.Count; i++)
        {
            characterRole = GetComponent<CharacterRole>();
            characterRole.hand.RemoveAt(index[i]);
        }

        turnEnd = true; // ��� ��������
    }
}
