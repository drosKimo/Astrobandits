using System.Collections.Generic;
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

    void Awake()
    {
        getCardItem = GetComponent<GetCardItem>();
        CPdragScript = GetComponent<DragScript>();
    }

    // ����������� ��������� ����� ��������
    public void LootBoxes_Button()
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
                Debug.Log("����� �� ������");
                break;
        }

        RecreateInventory();
        Destroy(gameObject);
    }

    // TODO: ��� ����� ������ �����. ������ ����������� ��������� �� ���� �� ������
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
}
