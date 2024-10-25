using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    HelperData helperData;
    PlayCard playCard;

    bool missed = false; // провер€ет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
        helperData = GameObject.Find("WhenGameStarts").GetComponent<HelperData>();
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

            if (helperData.isChallenge) // когда против игрока
            {
                helperData.challengeDone = true;
                helperData.isChallenge = false;
            }
        }

        // показывает хп
        TMPro.TMP_Text text = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
        text.text = $"{characterRole.currentHP}/{characterRole.maxHP}"; 
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
            // мен€ем игрока, которому нужно отвечать
            Enemy_AI attacker_AI = helperData.challenge_AI; // текущий атакующий

            if (attacker_AI.target.gameObject.tag != "Player")
            {
                Enemy_AI defender_AI = helperData.challenge_AI.target.gameObject.GetComponent<Enemy_AI>(); // текущий отвечающий
                CharacterRole attacker = attacker_AI.gameObject.GetComponent<CharacterRole>();

                helperData.challenge_AI = defender_AI; // отвечающий становитс€ атакующим
                helperData.challenge_AI.target = attacker; // атакующий становитс€ отвечающим

                // ломаетс€ где-то здесь
            }

            yield return new WaitForSeconds(0.3f);
            playCard.Challenge();
        }
        else
            helperData.challengeDone = true;

        yield return null;
    }
}
