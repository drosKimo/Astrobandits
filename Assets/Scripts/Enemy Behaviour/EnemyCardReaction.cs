using UnityEngine;

public class EnemyCardReaction : MonoBehaviour
{
    EnemyCard enemyCard;

    bool missed; // проверяет, может ли противник отбить атаку
    int itemIndex; // индекс разыгранной карты

    void Awake()
    {
        enemyCard = GetComponent<EnemyCard>();
    }

    void EnemyDodge() // разыгрывается противником, когда ему нужно скинуть Уворот
    {
        // проверяет все карты в руке противника
        foreach (Cards card in enemyCard.enemyCards)
        {
            if (card.itemName == "Cards.Name.Dodge")
            {
                missed = true;
                itemIndex = enemyCard.enemyCards.IndexOf(card);
            }
        }

        if (missed == true)
        {
            enemyCard.enemyCards.RemoveAt(itemIndex);
            Debug.Log("Мимо!");
        }
        else
            Debug.Log("Попал");
    }

    void EnemySlam() // аналогично, но когда ему нужно отстреляться
    {
        foreach (Cards card in enemyCard.enemyCards)
        {
            if (card.itemName == "Cards.Name.Slam")
            {
                missed = true;
                itemIndex = enemyCard.enemyCards.IndexOf(card);
            }
        }

        if (missed == true)
        {
            enemyCard.enemyCards.RemoveAt(itemIndex);
            Debug.Log("Мимо!");
        }
        else
            Debug.Log("Попал");
    }


    public void Armageddets()
    {
        EnemyDodge();
    }

    public void Slam()
    {
        EnemyDodge();
    }

    public void Insectoids()
    {
        EnemySlam();
    }
}
