using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    EnemyCard enemyCard;
    CharacterRole characterRole;

    bool missed; // ���������, ����� �� ��������� ������ �����
    int itemIndex; // ������ ����������� �����

    void Awake()
    {
        enemyCard = GetComponent<EnemyCard>();
        characterRole = GetComponent<CharacterRole>();
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

        Reaction();
    }

    void EnemyPow() // ����������, �� ����� ��� ����� ������������
    {
        foreach (Cards card in enemyCard.enemyCards)
        {
            if (card.itemName == "Cards.Name.Pow")
            {
                missed = true;
                itemIndex = enemyCard.enemyCards.IndexOf(card);
            }
        }

        Reaction();
    }

    void Reaction()
    {
        if (missed == true)
        {
            enemyCard.enemyCards.RemoveAt(itemIndex);
            Debug.Log("����!");
        }
        else
        {
            characterRole.currentHP--;
            if (characterRole.currentHP <= 0)
            {
                Destroy(gameObject);
            }
            Debug.Log("�����");
        }
    }


    public void Armageddets()
    {
        EnemyDodge();
    }

    public void Pow()
    {
        EnemyDodge();
    }

    public void Insectoids()
    {
        EnemyPow();
    }
}
