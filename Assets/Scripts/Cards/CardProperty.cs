using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// этот скрипт висит на картах игрока, чтобы их можно было разыгрывать

// необходимо разбить отыгровки карт на разные скрипты, чтобы позже это
// было удобнее для мультиплеера и управления ботами

public class CardProperty : MonoBehaviour
{
    GetCardItem getCardItem;
    [HideInInspector] public DragScript CPdragScript;
    PlayCard playCard;
    EnemyCardReaction enemyCardReaction;
    HelperData helperData;
    CharacterRole playerYou;

    [HideInInspector] public GameObject enemyObj;
    [HideInInspector] public bool cardNeeded;

    void Start()
    {
        playerYou = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>();
        helperData = GameObject.Find("WhenGameStarts").GetComponent<HelperData>();
        helperData.isChallenge = false;
    }

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        CPdragScript = GetComponent<DragScript>();
    }

    // продолжение розыгрыша карты лутбокса
    public void React_Button()
    {
        // сначала проверяет в каком контейнере находятся карты
        switch (gameObject.transform.parent.name)
        {
            case "Elements Container":
                if (cardNeeded)
                {
                    GameObject container = GameObject.Find("Elements Container");

                    for (int i = 0; i < container.transform.childCount; i++) // разблокирует все карты
                    {
                        DragScript dragCard = container.transform.GetChild(i).GetComponent<DragScript>();
                        Button buttonCard = dragCard.gameObject.GetComponent<Button>();

                        dragCard.enabled = true;
                        buttonCard.enabled = false;
                    }

                    if (helperData.isChallenge)
                    {
                        // начинает цикл перестрелки пока у кого-то не кончатся Тыщ
                        Play_Challenge();
                    }
                    else
                    {
                        // включает блокировку карт
                        TurnManager turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
                        turnManager.blocker.SetActive(true);
                    }

                    // передает данные о том, что игрок закончил реакцию
                    PlayCard playEnemyCard = enemyObj.GetComponent<PlayCard>();
                    playEnemyCard.playerDone = true;

                    RecreateInventory();
                    Destroy(gameObject); // уничтожает эту карту
                }
                break;

            case "LootBoxes Container":
                React_LootBoxes();
                break;
        }
    }

    void Play_Challenge() // только когда игрок разыграл Вызов
    {
        EnemyCardReaction reactEnemy = helperData.challenge_AI.gameObject.GetComponent<EnemyCardReaction>();

        StartCoroutine(reactEnemy.Challenge());
    }

    // включает свойство карты по имени
    public void GetCardToPlay()
    {
        playCard = CPdragScript.hit.collider.gameObject.GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Challenge":
                helperData.isChallenge = true;

                // переносим данные во внешнее хранилище
                helperData.challenge_AI = playCard.gameObject.GetComponent<Enemy_AI>(); // на кого кинули Вызов
                helperData.challenge_AI.target = playerYou.gameObject.GetComponent<CharacterRole>(); // он объявляет игрока целью

                Play_Challenge();
                break;

            case "Cards.Name.Collapsar":
                playCard.Collapsar();
                break;

            case "Cards.Name.CryoCharge":
                CharacterRole enemy = playCard.gameObject.GetComponent<CharacterRole>();
                enemy.frozen = true;
                break;

            case "Cards.Name.CyberImplant":
                if (!playerYou.implantSet)
                {
                    playCard.CyberImplant();
                    playerYou.implantSet = true;
                }
                else
                    Debug.Log("Имплант уже установлен");
                break;

            case "Cards.Name.ForceField":
                playCard.ForceField();
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
                helperData.shotDone = true;
                enemyCardReaction = CPdragScript.hit.collider.gameObject.GetComponent<EnemyCardReaction>();
                enemyCardReaction.Pow();
                break;

            case "Cards.Name.SporeBeer":
                playCard.SporeBeer(); // если использовать пиво при полном хп, карта все равно потеряется
                break;

            case "Cards.Name.Turlock":
                playCard.Turlock();
                break;

            case "Cards.Name.XenoRunt":
                playCard.playStorage = getCardItem.globalStorage;
                playCard.XenoRunt();
                break;

            case "Cards.Name.Glitch":
                playCard.Glitch();
                break;

            case "Cards.Name.Overlap":
                playCard.Overlap();
                break;

            case "Cards.Name.Hemotransfusion":
                helperData.enemyTransHP.currentHP--;
                playerYou.currentHP++;

                // показывает хп противника
                TMPro.TMP_Text text = helperData.enemyTransHP.gameObject.GetComponentInChildren<TMPro.TMP_Text>();
                text.text = $"{helperData.enemyTransHP.currentHP}/{helperData.enemyTransHP.maxHP}";

                // показывает хп себя
                TMPro.TMP_Text textself = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
                textself.text = $"{playerYou.currentHP}/{playerYou.maxHP}";

                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }
        
        Destroy(gameObject);
        RecreateInventory();
    }

    // карты, которые разыгрываются на поле
    public void PlayBoardCard()
    {
        playCard = playerYou.gameObject.GetComponent<PlayCard>();

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

            case "Cards.Name.Instability":
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemyCardReaction = enemy.GetComponent<EnemyCardReaction>();
                    enemyCardReaction.Instability();
                }
                playerYou.currentHP--;
                break;

            default:
                Debug.Log("Карта не задана");
                break;
        }

        RecreateInventory();
        Destroy(gameObject);
    }

    void RecreateInventory()
    {
        // перемещает разыгранную карту во временное хранилище
        GameObject cardDestroy = GameObject.Find("CardToDestroy");
        gameObject.transform.SetParent(cardDestroy.transform);

        // очищает инвентарь игрока
        playerYou.hand.Clear();

        GameObject playerHand = GameObject.Find("Elements Container"); // карты на экране

        // воссоздает инвентарь
        for (int i = 0; i < playerHand.transform.childCount; i++)
        {
            // получает информацию о текущей карте в очереди в руке игрока
            GetCardItem cardItem = playerHand.transform.GetChild(i).gameObject.GetComponent<GetCardItem>();
            Cards card = cardItem.cardItem;

            playerYou.hand.Add(card); // добавляет карту с руки в инвентарь
        }
    }

    void React_LootBoxes()
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
            CharacterRole characterRole = enemy.GetComponent<CharacterRole>();
            characterRole.hand.Add(LBlist[rand.Next(0, LBgrid.transform.childCount)]);
        }
    }
}
