using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    TurnManager turnManager;
    PlayCard playCard;

    bool missed = false; // провер€ет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
        turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
    }

    void EnemyDodge() // разыгрываетс€ противником, когда ему нужно скинуть ”ворот
    {
        // провер€ет все карты в руке противника
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

    void EnemyPow() // аналогично, но когда ему нужно отстрел€тьс€
    {
        foreach (Cards card in characterRole.hand)
        {
            if (card.itemName == "Cards.Name.Pow")
            {
                missed = true;
                itemIndex = characterRole.hand.IndexOf(card);
                break; // нужно выйти из цикла на случай, если карт “ыщ больше 1 штуки
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
            Debug.Log($"{gameObject.name} потер€л хп");
            characterRole.currentHP--;
            if (characterRole.currentHP <= 0)
                characterRole.DeadPlayer();

            if (turnManager.isChallenge) // когда против игрока
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

        if (turnManager.isChallenge)
        {
            // мен€ем игрока, которому нужно отвечать
            Enemy_AI attacker_AI = turnManager.challenge_AI; // текущий атакующий

            if (attacker_AI.target.gameObject.tag != "Player")
            {
                Enemy_AI defender_AI = turnManager.challenge_AI.target.gameObject.GetComponent<Enemy_AI>(); // текущий отвечающий
                CharacterRole attacker = attacker_AI.gameObject.GetComponent<CharacterRole>();

                turnManager.challenge_AI = defender_AI; // отвечающий становитс€ атакующим
                turnManager.challenge_AI.target = attacker; // атакующий становитс€ отвечающим

                // ломаетс€ где-то здесь
            }

            yield return new WaitForSeconds(0.3f);
            playCard.Challenge();
        }
        else
            turnManager.challengeDone = true;

        yield return null;
    }
}
