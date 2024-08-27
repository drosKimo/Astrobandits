using System.Collections;
using Unity.VisualScripting;
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
                break; // ����� ����� �� ����� �� ������, ���� ���� ��� ������ 1 �����
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

            missed = false;
        }
        else
        {
            Debug.Log($"{gameObject.name} ������� ��");
            characterRole.currentHP--;
            if (characterRole.currentHP <= 0)
                characterRole.DeadPlayer();

            if (turnManager.isChallenge) // ����� ������ ������
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

        if (turnManager.isChallenge)
        {
            // ������ ������, �������� ����� ��������
            Enemy_AI attacker_AI = turnManager.challenge_AI; // ������� ���������

            if (attacker_AI.target.gameObject.tag != "Player")
            {
                Enemy_AI defender_AI = turnManager.challenge_AI.target.gameObject.GetComponent<Enemy_AI>(); // ������� ����������
                CharacterRole attacker = attacker_AI.gameObject.GetComponent<CharacterRole>();

                turnManager.challenge_AI = defender_AI; // ���������� ���������� ���������
                turnManager.challenge_AI.target = attacker; // ��������� ���������� ����������

                // �������� ���-�� �����
            }

            yield return new WaitForSeconds(0.3f);
            playCard.Challenge();
        }
        else
            turnManager.challengeDone = true;

        yield return null;
    }
}
