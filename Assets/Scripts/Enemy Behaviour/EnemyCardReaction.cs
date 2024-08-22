using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    TurnManager turnManager;
    PlayCard playCard;

    bool missed = false; // ���������, ����� �� ��������� ������ �����
    int itemIndex; // ������ ����������� �����

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
        turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
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

            if (turnManager.isChallenge)
            {
                turnManager.challengeDone = true;
                turnManager.isChallenge = false;
            }
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

        playCard = GetComponent<PlayCard>();

        if (missed)
        {
            // ������ ������, �������� ����� ��������
            Enemy_AI enemy_AI = GetComponent<Enemy_AI>(); // ������� ����������

            if (enemy_AI.target.gameObject.tag != "Player")
            {
                enemy_AI.target = turnManager.challenge_AI.gameObject.GetComponent<CharacterRole>(); // ������� ��������� ���������� �����
                turnManager.challenge_AI = enemy_AI; // ���������� ���������� ���������
            }

            playCard.Challenge();
        }
        else
            turnManager.challengeDone = true;

        yield return new WaitForSeconds(0.3f);
    }
}
