using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    TurnManager manager;
    HelperData helper;
    PlayerHierarchy hierarchy;
    CharacterRole characterRole, youPlayer;
    PlayCard playCard;
    Cards stolenCard;

    List<int> index;

    bool turnEnd;
    [HideInInspector] public bool playedPow = false; // ��������, ��������� ��

    void Start()
    {
        manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        helper = manager.gameObject.GetComponent<HelperData>();
        hierarchy = manager.gameObject.GetComponent<PlayerHierarchy>();

        turnEnd = false; // ��� �� ��������

        characterRole = GetComponent<CharacterRole>();
        playCard = GetComponent<PlayCard>();
    }

    // ������ ������ ����������
    public IEnumerator EnemyTurn()
    {
        StartCoroutine(EnemyPlayCard()); // ��������� ����������� �����

        yield return new WaitUntil(() => turnEnd); // ���� ���� ��� ����������
        yield return new WaitForSeconds(0.3f); // �������������� �������� ����� ���������� ����

        manager.EndTurn(); // ����������� ���
    }

    void EnemySearchOther()
    {
        // �������� ��������� ����
        System.Random rand = new System.Random();

        // ��������, ������ �� ������ ������ ���� ������ ����� ����
        target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        for (int i = 0; target.name == gameObject.name; i++)
        {
            target = GameObject.Find("Enemies").transform.GetChild(rand.Next(GameObject.Find("Enemies").transform.childCount)).GetComponent<CharacterRole>();
        }

        Animator anim = target.gameObject.GetComponentInChildren<Animator>();
        anim.SetTrigger("TargetSet");
    }

    IEnumerator EnemyPlayCard()
    {
        index = new List<int>();
        playedPow = false;

        restart:
        // ����� ����� ���� ��������� �����, ����� ��������� �������� � .List()
        foreach (Cards card in characterRole.hand.ToList())
        {
            yield return new WaitForSeconds(0.5f);

            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull())
            {
                youPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>(); // ���� ������
            }

            // ������� ������ ����� � ����� ���� � �������� �� ���������
            switch (card.itemName)
            {
                case "Cards.Name.Pow":
                    EnemySearchOther();

                    // ������� ����� ���������� �� ����
                    int calc = hierarchy.CalculateCircularDistance(gameObject.transform, target.gameObject.transform);

                    if (!playedPow || calc <= playCard.currentDistance) // ���������, ������� �� ��� ��������
                    {
                        playCard.playerDone = false; // �������� ������
                        playedPow = true;

                        StartCoroutine(playCard.Pow());
                        index.Add(characterRole.hand.IndexOf(card));

                        yield return new WaitUntil(() => playCard.playerDone); // ��������� ������ ������
                        // ������ ����� ����� ���������� ���
                    }
                    else
                        Debug.Log($"{gameObject.name} ��������� ����������");
                    break;

                case "Cards.Name.Insectoids":             
                    Debug.Log($"{gameObject.name} ������ {card.name}");
                    playCard.playerDone = false; // �������� ������

                    GameObject list = GameObject.Find("Enemies");
                    for (int i = 0; i < list.transform.childCount; i++)
                    {
                        GameObject currentEnemy = list.transform.GetChild(i).gameObject;
                        if (list.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // ������ ������� ������
                                target = list.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();

                                playCard.currentReactionCard = "Cards.Name.Pow"; // ������ �����, ������� ������ �������������� ����� �� �������� ��
                                StartCoroutine(playCard.WaitForPlayer()); // ��������� ������� ������

                                yield return new WaitUntil(() => playCard.playerDone); // ��������� ������ ������
                            }
                            else
                            {
                                EnemyCardReaction enemyCardReaction = currentEnemy.GetComponent<EnemyCardReaction>();
                                enemyCardReaction.Insectoids();
                            }
                        }
                    }
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Armageddets":
                    Debug.Log($"{gameObject.name} ������ {card.name}");
                    playCard.playerDone = false; // �������� ������

                    GameObject list2 = GameObject.Find("Enemies");
                    for (int i = 0; i < list2.transform.childCount; i++)
                    {
                        GameObject currentEnemy2 = list2.transform.GetChild(i).gameObject;
                        if (list2.transform.GetChild(i).gameObject.name != gameObject.name)
                        {
                            if (list2.transform.GetChild(i).gameObject.transform.tag == "Player")
                            {
                                // ������ ������� ������
                                target = list2.transform.GetChild(i).gameObject.GetComponent<CharacterRole>();

                                playCard.currentReactionCard = "Cards.Name.Dodge"; // ������ �����, ������� ������ �������������� ����� �� �������� ��
                                StartCoroutine(playCard.WaitForPlayer()); // ��������� ������� ������

                                yield return new WaitUntil(() => playCard.playerDone); // ��������� ������ ������
                            }
                            else
                            {
                                EnemyCardReaction enemyCardReaction = currentEnemy2.GetComponent<EnemyCardReaction>();
                                enemyCardReaction.Armageddets();
                            }
                        }
                    }
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Bartender":
                    if (characterRole.currentHP <= characterRole.maxHP - 2)
                    {
                        Debug.Log($"{gameObject.name} ������ {card.name}");
                        playCard.Bartender();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.SporeBeer":
                    if (characterRole.currentHP <= characterRole.maxHP - 1)
                    {
                        Debug.Log($"{gameObject.name} ������ {card.name}");
                        playCard.SporeBeer();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.MutantDealer":
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    playCard.MutantDealer();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // �������� ����������� ����� ������

                case "Cards.Name.Jackpot":
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    playCard.Jackpot();
                    index.Add(characterRole.hand.IndexOf(card));

                    DestroyCards();
                    goto restart; // �������� ����������� ����� ������

                case "Cards.Name.Shredder":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // ���� � ������ ���� ����� �� ����
                    {
                        int maxCard, ind;
                        System.Random rand = new System.Random();

                        // ���� ���� - �����, ���������� ����� �� ������ � ����������� ���������
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxCard = playerHand.transform.childCount - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // ���������� ��������� ����� �� ����
                            Destroy(playerHand.transform.GetChild(ind).gameObject); // ������� �� �� ����� �� ������
                        }
                        else
                        {
                            maxCard = target.hand.Count - 1;
                            ind = rand.Next(maxCard);
                            target.hand.RemoveAt(ind); // ���������� ��������� ����� �� ����
                        }

                        Debug.Log($"{gameObject.name} ��������� ����� {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.LootBoxes":
                    Debug.Log($"{gameObject.name} ������ {card.name}");

                    playCard.EnemyLootBoxes();
                    index.Add(characterRole.hand.IndexOf(card)); 

                    DestroyCards();
                    goto restart; // �������� ����������� ����� ������

                case "Cards.Name.XenoRunt":
                    EnemySearchOther(); // target = CharacterRole

                    if (target.hand.Count > 0) // ���� � ������ ���� ����� �� ����
                    {
                        int maxcard, indexI;
                        System.Random random = new System.Random();

                        // ���� ���� - �����, ���������� ����� �� ������ � ����������� ���������
                        if (target.gameObject.tag == "Player")
                        {
                            GameObject playerHand = GameObject.Find("Elements Container");
                            maxcard = playerHand.transform.childCount - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // ���������� ��������� ����� �� ����
                            Destroy(playerHand.transform.GetChild(indexI).gameObject); // ������� �� �� ����� �� ������
                        }
                        else
                        {
                            maxcard = target.hand.Count - 1;
                            indexI = random.Next(maxcard);

                            stolenCard = target.hand[indexI];
                            target.hand.RemoveAt(indexI); // ���������� ��������� ����� �� ����
                        }

                        characterRole.hand.Add(stolenCard); // ��������� ���������� ����� � ����

                        Debug.Log($"{gameObject.name} ����� ����� {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));

                        DestroyCards();
                        goto restart;
                    }
                    // ����� ��������� ��� ����� � ����
                    break; // ����� ������ ����������� ����� ������

                case "Cards.Name.Reassembly":
                    Debug.Log($"{gameObject.name} ������ {card.name}");
                    playCard.Reassembly();
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Challenge":
                    EnemySearchOther(); // ���� target
                    Debug.Log($"{gameObject.name} ����� ����� {target.name}");

                    helper.challenge_AI = gameObject.GetComponent<Enemy_AI>();
                    helper.challengeDone = false; // �������� ������

                    index.Add(characterRole.hand.IndexOf(card));
                    DestroyCards();

                    playCard.Challenge();

                    yield return new WaitUntil(() => helper.challengeDone); // ��������� ������ ������
                    break;

                case "Cards.Name.Scorpion":
                    if (playCard.currentDistance <= 2)
                    {
                        Debug.Log($"{gameObject.name} ���� � ���� {card.name}");
                        playCard.Scorpion();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.Turlock":
                    if (playCard.currentDistance <= 3)
                    {
                        Debug.Log($"{gameObject.name} ���� � ���� {card.name}");
                        playCard.Turlock();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.PulseRifle":
                    if (playCard.currentDistance <= 4)
                    {
                        Debug.Log($"{gameObject.name} ���� � ���� {card.name}");
                        playCard.PulseRifle();
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.CyberImplant":
                    if (!characterRole.implantSet) // ����� �� �� ��� ���������� ����������� ���� ������������
                    {
                        Debug.Log($"{gameObject.name} ������ {card.name}");
                        playCard.CyberImplant();
                        characterRole.implantSet = true;
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.CryoCharge":
                    EnemySearchOther();
                    Debug.Log($"{gameObject.name} ��������� {target.name}");
                    target.frozen = true;
                    index.Add(characterRole.hand.IndexOf(card));
                    break;

                case "Cards.Name.Hemotransfusion":
                    EnemySearchOther();
                    if (target.currentHP >= 2 && characterRole.currentHP != characterRole.maxHP)
                    {
                        target.currentHP--; // �������� � ����������
                        characterRole.currentHP++; // ��������� ����

                        // ���������� �� ����������
                        TMPro.TMP_Text text = target.gameObject.GetComponentInChildren<TMPro.TMP_Text>();
                        text.text = $"{target.currentHP}/{target.maxHP}";

                        // ���������� �� ����
                        TMPro.TMP_Text textself = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
                        textself.text = $"{characterRole.currentHP}/{characterRole.maxHP}";

                        Debug.Log($"{gameObject.name} ������� ���� �� {target.name}");
                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                case "Cards.Name.Instability":
                    if (characterRole.currentHP >= 2) // ����� �� ����� ����
                    {
                        Debug.Log($"{gameObject.name} ������ {card.name}");
                        GameObject all = gameObject.transform.parent.gameObject;

                        for(int i = 0; i < all.transform.childCount; i++)
                        {
                            CharacterRole current = all.transform.GetChild(i).GetComponent<CharacterRole>();
                            switch (current.gameObject.tag)
                            {
                                case "Player":
                                    current.currentHP -= 2;
                                    break;

                                case "Enemy":
                                    if (current.gameObject != gameObject)
                                    {
                                        EnemyCardReaction enemyCardReaction = current.gameObject.GetComponent<EnemyCardReaction>();
                                        enemyCardReaction.Instability();
                                    }
                                    else
                                        current.currentHP--;
                                    break;
                            }

                            // ���������� ��
                            TMPro.TMP_Text text = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
                            text.text = $"{current.currentHP}/{current.maxHP}";
                        }

                        index.Add(characterRole.hand.IndexOf(card));
                    }
                    break;

                default:
                    Debug.Log("��������� �� ����� ������ ��� �����");
                    break;
            }

            // ���������, ���� �� �����
            if (!GameObject.FindGameObjectWithTag("Player").IsUnityNull() && youPlayer.currentHP <= 0)
            {
                characterRole = youPlayer.GetComponent<CharacterRole>();
                characterRole.DeadPlayer();
            }

            /*switch (card.itemName)
            {
                case "Cards.Name.Collapsar":
                    break;
                case "Cards.Name.Glitch":
                    break;
                case "Cards.Name.Overlap":
                    break;
                case "Cards.Name.ForceField":
                    break;
            }*/
        }

        if (characterRole.hand.Count > 0) DestroyCards();
        LeaveCards();

        turnEnd = true; // ��� ��������
    }

    // ������� ����������� �����
    void DestroyCards()
    {
        // ������� �������������� ����� �� ���� � �������� �������
        for (int i = index.Count; i > 0; i--)
        {
            characterRole = gameObject.GetComponent<CharacterRole>();
            characterRole.hand.RemoveAt(index[i - 1]);
        }
        index.Clear(); // ������� �������� ����� �������� ����
    }

    // ��������� � ���� �� ������ ����, ��� �� � ������
    void LeaveCards()
    {
        // ������ ������� ������ ��� ����� � ����� ������. ����� ����� �������� �������, ��� ������� ���������
        // ����� ��������� � ���� �������� �����
        for (int i = characterRole.hand.Count; i > characterRole.currentHP; i--)
        {
            characterRole.hand.RemoveAt(i - 1);
        }
    }
}
