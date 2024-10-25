using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    HelperData helperData;
    PlayCard playCard;

    bool missed = false; // ���������, ����� �� ��������� ������ �����
    int itemIndex; // ������ ����������� �����

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
        helperData = GameObject.Find("WhenGameStarts").GetComponent<HelperData>();
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

            if (helperData.isChallenge) // ����� ������ ������
            {
                helperData.challengeDone = true;
                helperData.isChallenge = false;
            }
        }

        // ���������� ��
        TMPro.TMP_Text text = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
        text.text = $"{characterRole.currentHP}/{characterRole.maxHP}"; 
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

    public void Instability()
    {
        Reaction();
        Reaction();
    }

    public IEnumerator Challenge()
    {
        EnemyPow();

        playCard = GetComponent<PlayCard>();

        if (helperData.isChallenge)
        {
            // ������ ������, �������� ����� ��������
            Enemy_AI attacker_AI = helperData.challenge_AI; // ������� ���������

            if (attacker_AI.target.gameObject.tag != "Player")
            {
                Enemy_AI defender_AI = helperData.challenge_AI.target.gameObject.GetComponent<Enemy_AI>(); // ������� ����������
                CharacterRole attacker = attacker_AI.gameObject.GetComponent<CharacterRole>();

                helperData.challenge_AI = defender_AI; // ���������� ���������� ���������
                helperData.challenge_AI.target = attacker; // ��������� ���������� ����������

                // �������� ���-�� �����
            }

            yield return new WaitForSeconds(0.3f);
            playCard.Challenge();
        }
        else
            helperData.challengeDone = true;

        yield return null;
    }
}
