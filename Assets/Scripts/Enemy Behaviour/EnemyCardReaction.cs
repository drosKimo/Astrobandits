using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    Enemy_AI thisAI;

    bool missed = false; // ���������, ����� �� ��������� ������ �����
    int itemIndex; // ������ ����������� �����

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
    }

    void EnemyDodge() // ������������� �����������, ����� ��� ����� ������� ������
    {
        // ��������� ��� ����� � ���� ����������
        foreach (Cards card in characterRole.hand)
        {
            if (card.itemName == "Cards.Name.Dodge")
            {
                missed = true;
                itemIndex = characterRole.hand.IndexOf(card);
            }
            else
                missed = false;
        }

        Reaction();
    }

    void EnemyPow() // ����������, �� ����� ��� ����� ������������
    {
        foreach (Cards card in characterRole.hand)
        {
            if (card.itemName == "Cards.Name.Pow")
            {
                missed = true;
                itemIndex = characterRole.hand.IndexOf(card);
            }
            else
                missed = false;
        }

        Reaction();
    }

    void Reaction()
    {
        if (missed == true)
        {
            characterRole.hand.RemoveAt(itemIndex);
        }
        else
        {
            characterRole.currentHP--;
            if (characterRole.currentHP <= 0)
                characterRole.DeadPlayer();
        }
    }


    // ������� �� �����
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

    public IEnumerator Challenge()
    {
        EnemyPow();

        if (missed)
        {
            PlayCard playCard = GetComponent<PlayCard>(); // ���������� ����� �������������, ���� ��� ������������� �����
           
            StartCoroutine(playCard.Challenge());
            yield return null;
        }
    }
}
