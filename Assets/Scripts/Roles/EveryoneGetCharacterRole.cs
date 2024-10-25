using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ������ ������� ������ ���� � ���������
public class EveryoneGetCharacterRole : MonoBehaviour
{
    [HideInInspector] public List<GameObject> everyPlayer;
    [SerializeField] Storage thisStorage; // ����� ���������
    Characters character; // �������� �������� �����
    CharacterRole characterRole;

    System.Random rand;
    List<Roles> rolePool;

    void Start()
    {
        GetRolePool(); // ������ ��� �����

        GameObject obj = GameObject.Find("Enemies");
        for (int i = 0; i < obj.transform.childCount; i++) // ���� ������� ��������� �� ����
        {
            everyPlayer.Add(obj.transform.GetChild(i).gameObject);
        }

        foreach (GameObject player in everyPlayer)
        {
            TMP_Text text = player.GetComponentInChildren<TMP_Text>(); // ��������. ��� ������������ ��
            characterRole = player.GetComponent<CharacterRole>();
            rand = new System.Random();

            // ��������� ��������� ���������. ��������
            SetCharacter();
            SetRole();

            characterRole.currentHP = characterRole.maxHP; // ������������� ������� �������� ��
            text.text = $"{characterRole.currentHP}/{characterRole.maxHP}"; // ���������� ��
        }

        QueueList queueList = GameObject.Find("WhenGameStarts").GetComponent<QueueList>();
        queueList.SetQueueList();
    }

    void GetRolePool()
    {
        rolePool = new List<Roles>();
        // 1 �������
        rolePool.Add(thisStorage.allRoles[0]);
        // 1 �������
        rolePool.Add(thisStorage.allRoles[1]);
        // 1 ��������
        rolePool.Add(thisStorage.allRoles[2]);
        // 2 ������
        rolePool.Add(thisStorage.allRoles[3]);
        rolePool.Add(thisStorage.allRoles[3]);
    }

    void SetCharacter()
    {
        character = thisStorage.allCharacters[0];

        characterRole.character = character;
        characterRole.maxHP = character.characterHitPoint;
    }

    void SetRole()
    {
        int roleIndex = rand.Next(rolePool.Count);
        characterRole.role = rolePool[roleIndex]; // ��������� ���� ������

        if (rolePool[roleIndex].roleName == "Roles.Name.Captain") // ��������� +1 �� ��������
            characterRole.maxHP++;

        rolePool.RemoveAt(roleIndex); // ������� ���� �� ������, ����� �� ���� ��������
    }
}
