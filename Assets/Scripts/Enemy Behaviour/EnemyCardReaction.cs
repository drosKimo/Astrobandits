using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    CharacterRole characterRole;

    bool missed; // провер€ет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        characterRole = GetComponent<CharacterRole>();
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
            }
        }

        Reaction();
    }

    void Reaction()
    {
        if (missed == true)
        {
            characterRole.hand.RemoveAt(itemIndex);
            Debug.Log("ћимо!");
        }
        else
        {
            characterRole.currentHP--;
            if (characterRole.currentHP <= 0)
            {
                Destroy(gameObject);
            }
            Debug.Log("ѕопал");
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
