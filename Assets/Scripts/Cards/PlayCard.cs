using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("����� ���������");
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
        Debug.Log("����� ���������");
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
