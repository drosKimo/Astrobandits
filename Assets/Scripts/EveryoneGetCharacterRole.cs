using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

// ������ ������� ������ ���� � ���������
public class EveryoneGetCharacterRole : MonoBehaviour
{
    [HideInInspector] public List<GameObject> everyPlayer;
    [SerializeField] Storage thisStorage; // ����� ���������
    [SerializeField] Characters character; // �������� �������� �����
    CharacterRole characterRole;

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
            // ��������� ��������� ���������. ��������
            characterRole = player.GetComponent<CharacterRole>();
            characterRole.character = character;
            characterRole.maxHP = character.characterHitPoint;

            System.Random rand = new System.Random();
            int roleIndex = rand.Next(rolePool.Count);

            characterRole.role = rolePool[roleIndex]; // ��������� ���� ������

            if (rolePool[roleIndex].roleName == "Roles.Name.Captain") // ��������� +1 �� ��������
                characterRole.maxHP++;

            rolePool.RemoveAt(roleIndex); // ������� ���� �� ������, ����� �� ���� ��������

            characterRole.currentHP = characterRole.maxHP; // ������������� ������� �������� ��
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
}
