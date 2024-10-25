using System.Collections.Generic;
using UnityEngine;
using TMPro;

// выдает каждому игроку роль и персонажа
public class EveryoneGetCharacterRole : MonoBehaviour
{
    [HideInInspector] public List<GameObject> everyPlayer;
    [SerializeField] Storage thisStorage; // общее хранилище
    Characters character; // персонаж задается здесь
    CharacterRole characterRole;

    System.Random rand;
    List<Roles> rolePool;

    void Start()
    {
        GetRolePool(); // задает пул ролей

        GameObject obj = GameObject.Find("Enemies");
        for (int i = 0; i < obj.transform.childCount; i++) // ищет каждого персонажа на поле
        {
            everyPlayer.Add(obj.transform.GetChild(i).gameObject);
        }

        foreach (GameObject player in everyPlayer)
        {
            TMP_Text text = player.GetComponentInChildren<TMP_Text>(); // временно. Для демонстрации ХП
            characterRole = player.GetComponent<CharacterRole>();
            rand = new System.Random();

            // назначает выданного персонажа. временно
            SetCharacter();
            SetRole();

            characterRole.currentHP = characterRole.maxHP; // устанавливает текущее значение хп
            text.text = $"{characterRole.currentHP}/{characterRole.maxHP}"; // показывает хп
        }

        QueueList queueList = GameObject.Find("WhenGameStarts").GetComponent<QueueList>();
        queueList.SetQueueList();
    }

    void GetRolePool()
    {
        rolePool = new List<Roles>();
        // 1 капитан
        rolePool.Add(thisStorage.allRoles[0]);
        // 1 инженер
        rolePool.Add(thisStorage.allRoles[1]);
        // 1 пришелец
        rolePool.Add(thisStorage.allRoles[2]);
        // 2 пирата
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
        characterRole.role = rolePool[roleIndex]; // назначает роль игроку

        if (rolePool[roleIndex].roleName == "Roles.Name.Captain") // добавляет +1 хп капитану
            characterRole.maxHP++;

        rolePool.RemoveAt(roleIndex); // удаляет роль из списка, чтобы не было повторов
    }
}
