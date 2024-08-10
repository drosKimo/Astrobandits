using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// необходимо разбить отыгровки карт на разные скрипты, чтобы позже это
// было удобнее для мультиплеера и управления ботами

public class CardProperty : MonoBehaviour
{
    GetCardItem getCardItem;
    [HideInInspector] public DragScript CPdragScript;
    PlayCard playCard;
    EnemyCardReaction enemyCardReaction;
    CharacterRole characterRole;

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        CPdragScript = GetComponent<DragScript>();
    }

    // продолжение розыгрыша карты лутбокса
    public void LootBoxes_Button()
    {
        // сначала мы берем карту в руку
        GameObject baseGrid = GameObject.Find("Elements Container");

        // выключает компонент кнопки
        Button button = GetComponent<Button>();
        button.enabled = false;

        // включает возможность тянуть карту
        DragScript dragScript = GetComponent<DragScript>();
        dragScript.enabled = true;

        // включает скрипт, поднимающий карту
        ShowCard showCard = GetComponent<ShowCard>();
        showCard.enabled = true;

        // передвигает появившиеся карты в отдельный контейнер
        getCardItem.transform.SetParent(baseGrid.transform);
        getCardItem.transform.localScale = new Vector3(1, 1, 1);


        System.Random rand = new System.Random();
        GameObject LBgrid = GameObject.Find("LootBoxes Container");
        List<Cards> LBlist = new List<Cards>();

        // добавляет все оставшиеся карты в список
        for (int i = 0; i < LBgrid.transform.childCount; i++)
        {
            GetCardItem cardItem = LBgrid.transform.GetChild(i).gameObject.GetComponent<GetCardItem>();
            Cards card = cardItem.cardItem;
            LBlist.Add(card);
            Destroy(LBgrid.transform.GetChild(i).gameObject);
        }

        // раздает карты в случайном порядке
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            characterRole = enemy.GetComponent<CharacterRole>();
            characterRole.hand.Add(LBlist[rand.Next(0, LBgrid.transform.childCount)]);
        }
    }

    // включает свойство карты по имени
    public void GetCardToPlay()
    {
        playCard = CPdragScript.hit.collider.gameObject.GetComponent<PlayCard>();

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

            case "Cards.Name.Pow":
                enemyCardReaction = CPdragScript.hit.collider.gameObject.GetComponent<EnemyCardReaction>();
                enemyCardReaction.Pow();
                break;

            case "Cards.Name.SporeBeer":
                // Если использовать пиво при полном хп, карта все равно потеряется
                playCard.SporeBeer();
                break;

            case "Cards.Name.Turlock":
                playCard.Turlock();
                break;

            case "Cards.Name.XenoRunt":
                playCard.playStorage = getCardItem.globalStorage;
                playCard.XenoRunt();
                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }
        
        RecreateInventory();
        Destroy(gameObject);
    }

    // карты, которые разыгрываются на поле
    public void PlayBoardCard()
    {
        playCard = GameObject.FindWithTag("Player").GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Armageddets":
                // проверка для каждого из врагов
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemyCardReaction = enemy.GetComponent<EnemyCardReaction>();
                    enemyCardReaction.Armageddets();
                }
                break;

            case "Cards.Name.Bartender":
                playCard.Bartender();
                break;

            case "Cards.Name.Dodge":
                playCard.Dodge();
                break;

            case "Cards.Name.Insectoids":
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemyCardReaction = enemy.GetComponent<EnemyCardReaction>();
                    enemyCardReaction.Insectoids();
                }
                break;

            case "Cards.Name.LootBoxes":
                playCard.LootBoxes();
                break;

            case "Cards.Name.Reassembly":
                playCard.playStorage = getCardItem.globalStorage;
                playCard.Reassembly();
                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }

        RecreateInventory();
        Destroy(gameObject);
    }

    // TODO: эта штука ультра сырая. Добавь воссоздание инвентаря из руки на экране
    void RecreateInventory()
    {
        // перемещает разыгранную карту во временное хранилище
        GameObject cardDestroy = GameObject.Find("CardToDestroy");
        gameObject.transform.SetParent(cardDestroy.transform);

        // очищает инвентарь игрока
        CharacterRole playerChar = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>();
        playerChar.hand.Clear();

        GameObject playerHand = GameObject.Find("Elements Container"); // карты на экране

        // воссоздает инвентарь
        for (int i = 0; i < playerHand.transform.childCount; i++)
        {
            // получает информацию о текущей карте в очереди в руке игрока
            GetCardItem cardItem = playerHand.transform.GetChild(i).gameObject.GetComponent<GetCardItem>();
            Cards card = cardItem.cardItem;

            playerChar.hand.Add(card); // добавляет карту с руки в инвентарь
        }
    }
}
