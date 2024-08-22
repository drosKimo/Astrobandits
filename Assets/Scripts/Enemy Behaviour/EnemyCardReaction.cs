using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    TurnManager turnManager;
    PlayCard playCard;

    bool missed = false; // проверяет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
        turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
    }

    void EnemyDodge() // разыгрывается противником, когда ему нужно скинуть Уворот
    {
        // проверяет все карты в руке противника
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

    void EnemyPow() // аналогично, но когда ему нужно отстреляться
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


    // реакции на карты
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
            // меняем игрока, которому нужно отвечать
            Enemy_AI enemy_AI = GetComponent<Enemy_AI>(); // текущий отвечающий

            if (enemy_AI.target.gameObject.tag != "Player")
            {
                enemy_AI.target = turnManager.challenge_AI.gameObject.GetComponent<CharacterRole>(); // текущий атакующий становится целью
                turnManager.challenge_AI = enemy_AI; // отвечающий становится атакующим
            }

            playCard.Challenge();
        }
        else
            turnManager.challengeDone = true;

        yield return new WaitForSeconds(0.3f);
    }
}
