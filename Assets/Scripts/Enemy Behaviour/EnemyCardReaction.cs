using System.Collections;
using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;
    Enemy_AI thisAI;

    bool missed = false; // проверяет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
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

        if (missed)
        {
            PlayCard playCard = GetComponent<PlayCard>(); // отвечающий может отреагировать, зная как разыгрывается карта
           
            StartCoroutine(playCard.Challenge());
            yield return null;
        }
    }
}
