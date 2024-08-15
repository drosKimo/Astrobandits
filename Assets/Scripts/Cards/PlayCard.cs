using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard : MonoBehaviour
{
    [SerializeField] SpawnCard spawnCard;
    [HideInInspector] public Cards playedCard;
    [HideInInspector] public Storage playStorage;

    CharacterRole characterRole;
    Enemy_AI enemy_AI;

    bool dodged = false;
    [HideInInspector] public bool playerDone;
    [HideInInspector] public string currentReactionCard;

    public IEnumerator Pow() // ����� ����������
    {
        enemy_AI = GetComponent<Enemy_AI>();
        playerDone = false; // �������� ������

        if (enemy_AI.target.gameObject.tag == "Player") // �������� ��, ���� ��� �����
        {
            // ��� �������, ��� �������� ���������� ����� �������� ��������� �� �� ��������� � ��������
            // � ������, ���� ������ ������� �� �����, ����� ������ � �����
            // ������: ���� ����� ����������� �����, � ���� ����������� ����� � ����� ��������

            // �� �����, ��� ����� ����������� �� ������ ������, ���������, ���� �������� �����������, ��������� ��� �����
            // �� ��� ��������, �� ����� ������� ���������, ����� �� �������� �� ����� �� �������

            currentReactionCard = "Cards.Name.Dodge"; // ������ �����, ������� ������ �������������� ����� �� �������� ��
            StartCoroutine(WaitForPlayer()); // ��������� ������� ������
        }
        else // ��������� ���������� ������������� �� �����
        {
            EnemyCardReaction reaction = enemy_AI.target.GetComponent<EnemyCardReaction>();
            reaction.Pow();
            playerDone = true;
        }

        Debug.Log($"{gameObject.name} ��������� � {enemy_AI.target.name}");
        yield return new WaitUntil(() => playerDone); // ��������� ������ ������
    }

    public void Bartender()
    {
        GameObject enemies = GameObject.Find("Enemies");
        List<GameObject> players = new List<GameObject>();

        SporeBeer(); // ������� ����� ������������ �� 1 ��

        // ���� ���� ����� �������
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            players.Add(enemies.transform.GetChild(i).gameObject);
        }

        // ����� ������� �� 1 ��
        foreach (GameObject enemy in players)
        {
            CharacterRole charRole = enemy.GetComponent<CharacterRole>();

            if (charRole.currentHP < charRole.maxHP) // ddd
                charRole.currentHP++;
        }
    }

    public void Bike()
    {
        Debug.Log("����� ���������");
    }

    public void Challenge()
    {
        // ���������� � ���������� � ���������, �� ����� ������� ���, ����� ��� ��������������
        // ����������� � ��������� �������� ��� �������

        // ��� �������, ����� ������� ���, ����� ����� ��� ����������� ������ ������ ����� � ����� �������

        Debug.Log("����� ���������");
    }

    public void Collapsar()
    {
        // ������ ��������� ���������� ������ ����� ���������� ������ ��� ������-��������
        // ����� ������ ���� ������, ��������� ��������� � �����

        // ����� ����� ��� �������������� �� ������ �����, �������� �� ����, ������ ����� ��������� �� ��������
        // � �����������

        Debug.Log("����� ���������");
    }

    public void CryoCharge()
    {
        // ���������� � �����������, ������ ��� ���������� ��������

        // ���������� �� ������ ����� ������������� ��������� �� �������� ����� ������-�������

        Debug.Log("����� ���������");
    }

    public void CyberImplant()
    {
        Debug.Log("����� ���������");
    }

    public void EnergyBlade()
    {
        // ��������� 1, ���������� ������ ��� ��������� ���

        Debug.Log("����� ���������");
    }

    public void ForceField()
    {
        Debug.Log("����� ���������");
    }

    public void Isabelle()
    {
        // ��������� 4

        Debug.Log("����� ���������");
    }

    public void Jackpot()
    {
        characterRole = GetComponent<CharacterRole>();

        characterRole.DrawCard();
        characterRole.DrawCard();
        characterRole.DrawCard();
    }

    public void LootBoxes()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ������� ������� ����, ������� ����� �������
        for (int i = 0; i < enemies.Length + 1; i++)
        {
            spawnCard.Spawn();
            GameObject card = spawnCard.newItem.gameObject;
            GameObject LBgrid = GameObject.Find("LootBoxes Container");
            GetCardItem item = card.GetComponent<GetCardItem>();

            // �������� ��������� ������
            Button button = card.GetComponent<Button>();
            button.enabled = true;

            // ��������� ����������� ������ �����
            DragScript dragScript = card.GetComponent<DragScript>();
            dragScript.enabled = false;

            // ��������� ������, ����������� �����
            ShowCard showCard = card.GetComponent<ShowCard>();
            showCard.enabled = false;

            // ����������� ����������� ����� � ��������� ���������
            item.transform.SetParent(LBgrid.transform);
            item.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
    }

    public void EnemyLootBoxes()
    {
        GameObject players = GameObject.Find("Enemies");
        
        // ����� ������� ���� �� �����
        for (int i = 0; i < players.transform.childCount; i++)
        {
            characterRole = players.transform.GetChild(i).GetComponent<CharacterRole>();
            characterRole.DrawCard();
        }
    }

    public void MutantDealer()
    {
        characterRole = GetComponent<CharacterRole>();

        characterRole.DrawCard();
        characterRole.DrawCard();
    }

    public void PulseRifle()
    {
        // ��������� 5
        // ������ �����, �������� �������� ������, ��������� ��� ���� � 5 �������� ������������ ���������� = 4
        // (��� ��������� - 2)

        Debug.Log("����� ���������");
    }

    public void Reassembly()
    {
        System.Random rand = new System.Random();
        List<Cards> allCards = new List<Cards>(); // ��� ����� �� ��� ����������
        Dictionary<string, int> players = new Dictionary<string, int>(); // ����� � ������� � ���� ����

        // �������� ��� ������� �� ������
        foreach (CharacterRole enemy in GameObject.Find("Enemies").transform.GetComponentsInChildren<CharacterRole>())
        {
            // ���� ������� �������� - �� ����������� ����� � � ���� �� ����� ���� �����
            if (enemy.gameObject.name != gameObject.name && enemy.hand.Count > 0)
            {
                // ��������� ����� � ���������� ��� ���� � ������
                players.Add(enemy.gameObject.name, enemy.hand.Count);

                // ���� ���������� ����� �� ������ ���������
                foreach (Cards card in enemy.hand)
                {
                    allCards.Add(card);
                }

                if (enemy.gameObject.tag == "Player")
                {
                    GameObject container = GameObject.Find("Elements Container");
                    
                    for (int i = container.transform.childCount; i > 0; i--) // �������� �����
                    {
                        Destroy(container.transform.GetChild(i - 1).gameObject);
                    }

                    enemy.hand.Clear(); // �������� ����

                }
                else
                    enemy.hand.Clear(); // �������� ����
            }
        }

        // ������ ������ ����� �������
        foreach (CharacterRole enemy in GameObject.Find("Enemies").transform.GetComponentsInChildren<CharacterRole>())
        {
            // ��������� ���� ������ ����������
            foreach (KeyValuePair<string, int> pair in players)
            {
                if (enemy.gameObject.name == pair.Key) // ���������, ��������� �� ������� � ������� �������� �� ���������
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        int max = allCards.Count;
                        int card = rand.Next(max);

                        if (enemy.gameObject.tag == "Player") // ������� ����� �� ����� ��� ������
                        {
                            // �������� ����� ���������
                            GetCardItem cardItem = spawnCard.SCprefab.GetComponent<GetCardItem>();
                            playStorage = cardItem.globalStorage;

                            // ������� ����� �� �������
                            cardItem.cardIndex = playStorage.allCards.IndexOf(allCards[card]);
                            cardItem.calling = false;
                            spawnCard.newItem = GameObject.Instantiate(spawnCard.SCprefab);

                            // �������� ��� ��� �����, ����� �� ���� ������ � ��������������� � �������
                            spawnCard.GetItemName();
                        }

                        enemy.hand.Add(allCards[card]); // ��������� �������� ����� � ����
                        allCards.RemoveAt(card); // ������� �������� ����� �� ������
                    }

                    players.Remove(pair.Key); // ������� �������� ��������� �� ������
                    break;
                }
            }
        }
    }

    public void Scorpion()
    {
        // ��������� 2

        Debug.Log("����� ���������");
    }

    public void Shredder() // ������ ����� ����������� �����
    {
        characterRole = GetComponent<CharacterRole>();

        int maxCard = characterRole.hand.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(maxCard);

        // ���������� ��������� ����� �� ����
        characterRole.hand.RemoveAt(index);
    }

    public void SporeBeer()
    {
        CharacterRole charRole = gameObject.GetComponent<CharacterRole>();

        if (charRole.currentHP < charRole.maxHP)
            charRole.currentHP++;
    }

    public void Turlock()
    {
        // ��������� 3

        Debug.Log("����� ���������");
    }

    public void XenoRunt()
    {
        // ��������� ���� ����������
        characterRole = GetComponent<CharacterRole>();

        int maxCard = characterRole.hand.Count - 1;

        System.Random rand = new System.Random();
        int index = rand.Next(maxCard);

        playedCard = characterRole.hand[index];
        characterRole.hand.RemoveAt(index);

        // ���� ���������� ����� �� ������ ���������
        foreach (Cards card in playStorage.allCards)
        {
            if (card.itemName == playedCard.itemName)
            {
                index = playStorage.allCards.IndexOf(card);
            }
        }
        spawnCard.SpawnByIndex(index); // � �������
    }

    public IEnumerator WaitForPlayer()
    {
        enemy_AI = GetComponent<Enemy_AI>();
        playerDone = false; // �������� ������

        yield return new WaitForSeconds(0.3f); // ��������� �������� ����� �����

        // ���������, ���� �� � ������ ���� �� 1 ������ � ���� �������� �����
        foreach (Cards card in enemy_AI.target.hand)
        {
            if (card.itemName == currentReactionCard)
                dodged = true;
        }

        // �������� ������, ���� �� � ������ ����������� �������������
        if (dodged)
        {
            // ����� �������� ���-��, ��� ����� ��������������� � ���, ��� ����� ����� �������������
            Debug.Log("�������� ������� ������");

            Player_CanMove();
        }
        else
        {
            enemy_AI.target.currentHP--;
            playerDone = true; // ����� �������
            Debug.Log("����� ������� ��");
        }
    }

    void Player_CanMove()
    {
        // ��������� ���������� ����
        TurnManager turnManager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        turnManager.blocker.SetActive(false);

        // ��������� ����������� �������� ����� � �������� ������
        GameObject container = GameObject.Find("Elements Container");
        for (int i = 0; i < container.transform.childCount; i++)
        {
            DragScript dragCard = container.transform.GetChild(i).GetComponent<DragScript>();
            Button buttonCard = dragCard.gameObject.GetComponent<Button>();
            CardProperty cardProperty = dragCard.gameObject.GetComponent<CardProperty>();
            GetCardItem thisCard = dragCard.gameObject.GetComponent<GetCardItem>();

            dragCard.enabled = false;
            buttonCard.enabled = true;

            // �������� ����� ������ � ������������ ���������� �����
            cardProperty.enemyObj = gameObject;

            // ���������, ��� �� ������ ������ �� �����, ������� ��� �����
            if (thisCard.nameKey == currentReactionCard)
                cardProperty.cardNeeded = true;
        }
    }
}
