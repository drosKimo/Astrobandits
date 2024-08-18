using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

// ���������� ������� ��������� ���� �� ������ �������, ����� ����� ���
// ���� ������� ��� ������������ � ���������� ������

public class CardProperty : MonoBehaviour
{
    GetCardItem getCardItem;
    [HideInInspector] public DragScript CPdragScript;
    PlayCard playCard;
    EnemyCardReaction enemyCardReaction;
    CharacterRole characterRole;
    TurnManager turnManager;

    [HideInInspector] public GameObject enemyObj;
    [HideInInspector] public bool cardNeeded;

    void Start()
    {
        turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        turnManager.isChallenge = false;
    }

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        CPdragScript = GetComponent<DragScript>();
    }

    // ����������� ��������� ����� ��������
    public void React_Button()
    {
        // ������� ��������� � ����� ���������� ��������� �����
        switch (gameObject.transform.parent.name)
        {
            case "Elements Container":
                if (cardNeeded)
                {
                    GameObject container = GameObject.Find("Elements Container");

                    for (int i = 0; i < container.transform.childCount; i++) // ������������ ��� �����
                    {
                        DragScript dragCard = container.transform.GetChild(i).GetComponent<DragScript>();
                        Button buttonCard = dragCard.gameObject.GetComponent<Button>();

                        dragCard.enabled = true;
                        buttonCard.enabled = false;
                    }

                    if (turnManager.isChallenge)
                    {
                        // �������� ���� ����������� ���� � ����-�� �� �������� ���
                        turnManager.isChallenge = false;
                        Play_Challenge();
                    }
                    else
                    {
                        // �������� ���������� ����
                        TurnManager turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
                        turnManager.blocker.SetActive(true);
                    }

                    // �������� ������ � ���, ��� ����� �������� �������
                    PlayCard playEnemyCard = enemyObj.GetComponent<PlayCard>();
                    playEnemyCard.playerDone = true;

                    RecreateInventory();
                    Destroy(gameObject); // ���������� ��� �����
                }
                break;

            case "LootBoxes Container":
                React_LootBoxes();
                break;
        }
    }

    void Play_Challenge() // ������ ����� ����� �������� �����
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // ����� �������� ������ ���������
        // 1 - ����� �����, 2 - ����� ���������
        //turnManager.playChallenge.challengeAI.target = player.GetComponent<CharacterRole>(); // AI ��������� ���������� �������
        //turnManager.challenge_AI = turnManager.playChallenge.challengeAI; // ? ������� ����� ��������� ����� �����

        turnManager.playChallenge = turnManager.challenge_AI.gameObject.GetComponent<PlayCard>();

        StartCoroutine(turnManager.playChallenge.enemyReact.Challenge());
    }

    // �������� �������� ����� �� �����
    public void GetCardToPlay()
    {
        playCard = CPdragScript.hit.collider.gameObject.GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Bike":
                playCard.Bike();
                break;

            case "Cards.Name.Challenge":
                // ��������� ������ �� ������� ���������
                turnManager.challenge_AI = CPdragScript.hit.collider.gameObject.GetComponent<Enemy_AI>();
                // �������� ����������, �� �������� ������� �����
                playCard.enemyReact = CPdragScript.hit.collider.gameObject.GetComponent<EnemyCardReaction>();
                // �������� AI ���������� �������
                playCard.challengeAI = CPdragScript.hit.collider.gameObject.GetComponent<Enemy_AI>();

                Play_Challenge();
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
                // ���� ������������ ���� ��� ������ ��, ����� ��� ����� ����������
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
                Debug.Log("����� �� ������");
                break;
        }
        
        RecreateInventory();
        Destroy(gameObject);
    }

    // �����, ������� ������������� �� ����
    public void PlayBoardCard()
    {
        playCard = GameObject.FindWithTag("Player").GetComponent<PlayCard>();

        switch (getCardItem.nameKey)
        {
            case "Cards.Name.Armageddets":
                // �������� ��� ������� �� ������
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

            default:
                Debug.Log("����� �� ������");
                break;
        }

        RecreateInventory();
        Destroy(gameObject);
    }

    void RecreateInventory()
    {
        // ���������� ����������� ����� �� ��������� ���������
        GameObject cardDestroy = GameObject.Find("CardToDestroy");
        gameObject.transform.SetParent(cardDestroy.transform);

        // ������� ��������� ������
        CharacterRole playerChar = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>();
        playerChar.hand.Clear();

        GameObject playerHand = GameObject.Find("Elements Container"); // ����� �� ������

        // ���������� ���������
        for (int i = 0; i < playerHand.transform.childCount; i++)
        {
            // �������� ���������� � ������� ����� � ������� � ���� ������
            GetCardItem cardItem = playerHand.transform.GetChild(i).gameObject.GetComponent<GetCardItem>();
            Cards card = cardItem.cardItem;

            playerChar.hand.Add(card); // ��������� ����� � ���� � ���������
        }
    }

    void React_LootBoxes()
    {
        // ������� �� ����� ����� � ����
        GameObject baseGrid = GameObject.Find("Elements Container");

        // ��������� ��������� ������
        Button button = GetComponent<Button>();
        button.enabled = false;

        // �������� ����������� ������ �����
        DragScript dragScript = GetComponent<DragScript>();
        dragScript.enabled = true;

        // �������� ������, ����������� �����
        ShowCard showCard = GetComponent<ShowCard>();
        showCard.enabled = true;

        // ����������� ����������� ����� � ��������� ���������
        getCardItem.transform.SetParent(baseGrid.transform);
        getCardItem.transform.localScale = new Vector3(1, 1, 1);


        System.Random rand = new System.Random();
        GameObject LBgrid = GameObject.Find("LootBoxes Container");
        List<Cards> LBlist = new List<Cards>();

        // ��������� ��� ���������� ����� � ������
        for (int i = 0; i < LBgrid.transform.childCount; i++)
        {
            GetCardItem cardItem = LBgrid.transform.GetChild(i).gameObject.GetComponent<GetCardItem>();
            Cards card = cardItem.cardItem;
            LBlist.Add(card);
            Destroy(LBgrid.transform.GetChild(i).gameObject);
        }

        // ������� ����� � ��������� �������
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            characterRole = enemy.GetComponent<CharacterRole>();
            characterRole.hand.Add(LBlist[rand.Next(0, LBgrid.transform.childCount)]);
        }
    }
}
