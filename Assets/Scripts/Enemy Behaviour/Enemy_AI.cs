using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    TurnManager manager;
    CharacterRole characterRole, youPlayer;
    PlayCard playCard;
    Cards stolenCard;

    List<int> index;

    bool turnEnd, playedPow = false;

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

    void EnemySearchOther()
    {
        // �������� ��������� ����
        System.Random rand = new System.Random();

        // ��������, ������ �� ������ ������ ���� ������ ����� ����
        target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        for (int i = 0; target.name == gameObject.name; i++)
        {
            target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        }
    }

    IEnumerator EnemyPlayCard()
    {
        index = new List<int>();

        restart:
        // ����� ����� ���� ��������� �����, ����� ��������� �������� � .List()
        foreach (Cards card in characterRole.hand.ToList())
        {
            yield return new WaitForSeconds(0.5f);

            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull())
            {
                youPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>(); // ���� ������
            }

            // ������� ������ ����� � ����� ���� � �������� �� ���������
            switch (card.itemName)
            {
                // TODO: ��������� ����������� �� ��������� ����� ������� ������
                case "Cards.Name.Pow":
                    if (!playedPow)
                    {
                        playedPow = true;
                        EnemySearchOther();
                        playCard.Pow();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    else
                        Debug.Log($"{gameObject.name} ��������� ����������");
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

                case "Cards.Name.Bartender":
                    if (characterRole.currentHP <= characterRole.maxHP - 2)
                    {
                        Debug.Log($"{gameObject.name} ������ {card.name}");
                        playCard.Bartender();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.SporeBeer":
                    Debug.Log($"{gameObject.name} ������ {card.name}");
                    playCard.SporeBeer();
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.MutantDealer":
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    playCard.MutantDealer();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // �������� ����������� ����� ������

                case "Cards.Name.Jackpot":
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    playCard.Jackpot();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // �������� ����������� ����� ������

                case "Cards.Name.Shredder":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // ���� � ������ ���� ����� �� ����
                    {
                        int maxCard, ind;
                        System.Random rand = new System.Random();

                        // ���� ���� - �����, ���������� ����� �� ������ � ����������� ���������
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxCard = playerHand.transform.childCount - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // ���������� ��������� ����� �� ����
                            Destroy(playerHand.transform.GetChild(ind).gameObject); // ������� �� �� ����� �� ������
                        }
                        else
                        {
                            maxCard = target.hand.Count - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // ���������� ��������� ����� �� ����
                        }

                        Debug.Log($"{gameObject.name} ��������� ����� {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.LootBoxes":
                    Debug.Log($"{gameObject.name} ������ {card.name}");
                    playCard.EnemyLootBoxes();
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.XenoRunt":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // ���� � ������ ���� ����� �� ����
                    {
                        int maxcard, indexI;
                        System.Random random = new System.Random();

                        // ���� ���� - �����, ���������� ����� �� ������ � ����������� ���������
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxcard = playerHand.transform.childCount - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // ���������� ��������� ����� �� ����
                            Destroy(playerHand.transform.GetChild(indexI).gameObject); // ������� �� �� ����� �� ������
                        }
                        else
                        {
                            maxcard = target.hand.Count - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // ���������� ��������� ����� �� ����
                        }

                        characterRole.hand.Add(stolenCard); // ��������� ���������� ����� � ����

                        Debug.Log($"{gameObject.name} ����� ����� {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));

                        DestroyCards();
                        goto restart;
                    }
                    // ����� ��������� ��� ����� � ����
                    break;

                default:
                    Debug.Log("��������� �� ����� ������ ��� �����");
                    break;
            }

            // ���������, ���� �� �����
            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull() && youPlayer.currentHP <= 0)
            {
                characterRole = youPlayer.GetComponent<CharacterRole>();
                characterRole.DeadPlayer();
            }

            /*switch (card.itemName)
            {
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
                case "Cards.Name.PulseRifle":
                    break;
                case "Cards.Name.Reassembly":
                    break;
                case "Cards.Name.Scorpion":
                    break;
                case "Cards.Name.Turlock":
                    break;
            }*/
        }

        DestroyCards();
        turnEnd = true; // ��� ��������
    }

    // ������� ����������� �����
    void DestroyCards()
    {
        // ������� �������������� ����� �� ���� � �������� �������
        for (int i = index.Count; i > 0; i--)
        {
            characterRole = gameObject.GetComponent<CharacterRole>();
            characterRole.hand.RemoveAt(index[i - 1]);
        }
        index.Clear(); // ������� �������� ����� �������� ����
    }
}
