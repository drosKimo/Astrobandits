using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    EnemyCard enemyCard;

    bool missed; // ���������, ����� �� ��������� ������ �����
    int itemIndex; // ������ ����������� �����

    void Awake()
    {
        enemyCard = GetComponent<EnemyCard>();
    }

    void EnemyDodge() // ������������� �����������, ����� ��� ����� ������� ������
    {
        // ��������� ��� ����� � ���� ����������
        foreach (Cards card in enemyCard.enemyCards)
        {
            if (card.itemName == "Cards.Name.Dodge")
            {
                missed = true;
                itemIndex = enemyCard.enemyCards.IndexOf(card);
            }
        }

        if (missed == true)
        {
            enemyCard.enemyCards.RemoveAt(itemIndex);
            Debug.Log("����!");
        }
        else
            Debug.Log("�����");
    }

    void EnemySlam() // ����������, �� ����� ��� ����� ������������
    {
        foreach (Cards card in enemyCard.enemyCards)
        {
            if (card.itemName == "Cards.Name.Slam")
            {
                missed = true;
                itemIndex = enemyCard.enemyCards.IndexOf(card);
            }
        }

        if (missed == true)
        {
            enemyCard.enemyCards.RemoveAt(itemIndex);
            Debug.Log("����!");
        }
        else
            Debug.Log("�����");
    }


    public void Armageddets()
    {
        EnemyDodge();
    }

    public void Slam()
    {
        EnemyDodge();
    }

    public void Insectoids()
    {
        EnemySlam();
    }
}
