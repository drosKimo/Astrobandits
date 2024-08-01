using Akassets.SmoothGridLayout;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard : MonoBehaviour
{
    [SerializeField] SpawnCard spawnCard;
    EnemyCard enemyCard;
    [HideInInspector] public Cards playedCard;
    [HideInInspector] public Storage playStorage;

    public void Bartender()
    {
        Debug.Log("����� ���������");
    }

    public void Bike()
    {
        Debug.Log("����� ���������");
    }

    public void Challenge()
    {
        Debug.Log("����� ���������");
    }

    public void Collapsar()
    {
        Debug.Log("����� ���������");
    }

    public void CryoCharge()
    {
        Debug.Log("����� ���������");
    }

    public void CyberImplant()
    {
        Debug.Log("����� ���������");
    }

    public void Dodge()
    {
        Debug.Log("����� ���������");
    }

    public void EnergyBlade()
    {
        Debug.Log("����� ���������");
    }

    public void ForceField()
    {
        Debug.Log("����� ���������");
    }

    public void Isabelle()
    {
        Debug.Log("����� ���������");
    }

    public void Jackpot()
    {
        spawnCard.Spawn();
        spawnCard.Spawn();
        spawnCard.Spawn();
    }

    public void LootBoxes()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ������� ������� ����, ������� ����� �������
        for (int i = 0; i < enemies.Length + 1; i++)
        {
            spawnCard.Spawn();
            GameObject card = spawnCard.newItem.gameObject;
            GameObject LBgrid = GameObject.Find("LootBoxes Container");
            GetCardItem item = card.GetComponent<GetCardItem>();

            // �������� ��������� ������
            Button button = card.GetComponent<Button>();
            button.enabled = true;

            // ��������� ����������� ������ �����
            DragScript dragScript = card.GetComponent<DragScript>();
            dragScript.enabled = false;

            // ��������� ������, ����������� �����
            ShowCard showCard = card.GetComponent<ShowCard>();
            showCard.enabled = false;

            // ����������� ����������� ����� � ��������� ���������
            item.transform.SetParent(LBgrid.transform);
            item.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
    }

    public void MutantDealer()
    {
        spawnCard.Spawn();
        spawnCard.Spawn();
    }

    public void PulseRifle()
    {
        Debug.Log("����� ���������");
    }

    public void Reassembly()
    {
        List<Cards> allCards = new List<Cards>(); // ��� ����� � ����
        Dictionary<string, int> players = new Dictionary<string, int>(); // ����� � ������� � ���� ����
        spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>(); // ������ ������ ����

        // �������� ��� ������� �� ������
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCard = enemy.GetComponent<EnemyCard>();
            // ��������� ����� � ���������� ��� ���� � ������
            players.Add(enemy.name, enemyCard.enemyCards.Count);

            // ���� ���������� ����� �� ������ ���������
            foreach (Cards card in enemyCard.enemyCards)
            {
                allCards.Add(card);
            }

            enemyCard.enemyCards.Clear(); // �������� ������
        }

        // ������ ������ ����� �������
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyCard = enemy.GetComponent<EnemyCard>();
            System.Random rand = new System.Random();

            // ��������� ���� ������ ����������
            foreach (KeyValuePair<string, int> pair in players)
            {
                if (enemy.name == pair.Key)
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        int max = allCards.Count;
                        int card = rand.Next(0, max);
                        enemyCard.enemyCards.Add(allCards[card]);
                        allCards.RemoveAt(card);
                    }

                    players.Remove(pair.Key); // ������� �������� ��������� �� ������
                    break;
                }
            }
        }
    }

    public void Scorpion()
    {
        Debug.Log("����� ���������");
    }

    public void Shredder()
    {
        enemyCard = GetComponent<EnemyCard>();

        int minCard = 0;
        int maxCard = enemyCard.enemyCards.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(minCard, maxCard);
        
        // ���������� ��������� ����� �� ����
        enemyCard.enemyCards.RemoveAt(index);
    }

    public void SporeBeer()
    {
        Debug.Log("����� ���������");
    }

    public void Turlock()
    {
        Debug.Log("����� ���������");
    }

    public void XenoRunt()
    {
        // ��������� ���� ����������
        enemyCard = GetComponent<EnemyCard>();

        int minCard = 0;
        int maxCard = enemyCard.enemyCards.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(minCard, maxCard);

        playedCard = enemyCard.enemyCards[index];
        enemyCard.enemyCards.RemoveAt(index);

        // ���� ���������� ����� �� ������ ���������
        foreach (Cards card in playStorage.allCards)
        {
            if (card.itemName == playedCard.itemName)
            {
                index = playStorage.allCards.IndexOf(card);
            }
        }
        spawnCard.SpawnByIndex(index); // � �������
    }
}
