using UnityEngine;

public class CardProperty : MonoBehaviour
{
    GetCardItem getCardItem;
    DragScript dragScript;
    PlayCard playCard;

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        dragScript = GetComponent<DragScript>();       
    }

    // включает свойство карты по имени
    public void GetCardToPlay()
    {
        playCard = dragScript.hit.collider.gameObject.GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Bike":
                playCard.Bike();
                break;

            case "Cards.Name.Challenge":
                playCard.Challenge();
                break;

            case "Cards.Name.Collapsar":
                playCard.Collapsar();
                break;

            case "Cards.Name.CryoCharge":
                playCard.CryoCharge();
                break;

            case "Cards.Name.CyberImplant":
                playCard.CyberImplant();
                break;

            case "Cards.Name.EnergyBlade":
                playCard.EnergyBlade();
                break;

            case "Cards.Name.ForceField":
                playCard.ForceField();
                break;

            case "Cards.Name.Isabelle":
                playCard.Isabelle();
                break;

            case "Cards.Name.Jackpot":
                playCard.Jackpot();
                break;

            case "Cards.Name.MutantDealer":
                playCard.MutantDealer();
                break;

            case "Cards.Name.PulseRifle":
                playCard.PulseRifle();
                break;

            case "Cards.Name.Scorpion":
                playCard.Scorpion();
                break;

            case "Cards.Name.Shredder":
                playCard.Shredder();
                break;

            case "Cards.Name.Slam":
                playCard.Slam();
                break;

            case "Cards.Name.SporeBeer":
                playCard.SporeBeer();
                break;

            case "Cards.Name.Turlock":
                playCard.Turlock();
                break;

            case "Cards.Name.XenoRunt":
                playCard.XenoRunt();
                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }

        Destroy(gameObject);
    }

    public void PlayBoardCard()
    {
        playCard = GameObject.FindWithTag("Player").GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Armageddets":
                playCard.Armageddets();
                break;

            case "Cards.Name.Bartender":
                playCard.Bartender();
                break;

            case "Cards.Name.Dodge":
                playCard.Dodge();
                break;

            case "Cards.Name.Insectoids":
                playCard.Insectoids();
                break;

            case "Cards.Name.LootBoxes":
                playCard.LootBoxes();
                break;

            case "Cards.Name.Reassembly":
                playCard.Reassembly();
                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }

        Destroy(gameObject);
        //Debug.Log("Йес минус три");
    }
}
