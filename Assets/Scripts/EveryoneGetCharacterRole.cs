using System.Collections.Generic;
using UnityEngine;

// выдает каждому игроку роль и персонажа
public class EveryoneGetCharacterRole : MonoBehaviour
{
    [HideInInspector] public List<GameObject> everyPlayer;
    [SerializeField] Storage thisStorage; // общее хранилище
    [SerializeField] Characters character; // персонаж задается здесь
    CharacterRole characterRole;

    void Start()
    {
        GameObject obj = GameObject.Find("Enemies");
        for (int i = 0; i < obj.transform.childCount; i++) // ищет каждого персонажа на поле
        {
            everyPlayer.Add(obj.transform.GetChild(i).gameObject);
        }

        foreach (GameObject player in everyPlayer)
        {
            // назначает выданную роль персонажу
            characterRole = player.GetComponent<CharacterRole>();
            characterRole.character = character;
            characterRole.currentHP = character.characerHitPoint;
        }
    }
}
