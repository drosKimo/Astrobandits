using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    SpawnCard spawnCard;
    GetCardItem cardItem;
    public List<Cards> enemyCards;

    public void GetEnemyCard()
    {
        spawnCard = GameObject.Find("SpawnCard").GetComponent<SpawnCard>();
        spawnCard.Spawn(); // ������� ����� �����, ������� �������� � ��������� �����
        cardItem = spawnCard.newItem.GetComponent<GetCardItem>();
        enemyCards.Add(cardItem.cardItem); // ��������� ��� ����� � ������
        Destroy(cardItem.gameObject); // ������� ��� ������, ����� ��� �� ���� � ���� � ������
    }
}
