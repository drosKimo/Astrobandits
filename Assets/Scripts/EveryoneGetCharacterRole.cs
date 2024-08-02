using System.Collections.Generic;
using UnityEngine;

// ������ ������� ������ ���� � ���������
public class EveryoneGetCharacterRole : MonoBehaviour
{
    [HideInInspector] public List<GameObject> everyPlayer;
    [SerializeField] Storage thisStorage; // ����� ���������
    [SerializeField] Characters character; // �������� �������� �����
    CharacterRole characterRole;

    void Start()
    {
        GameObject obj = GameObject.Find("Enemies");
        for (int i = 0; i < obj.transform.childCount; i++) // ���� ������� ��������� �� ����
        {
            everyPlayer.Add(obj.transform.GetChild(i).gameObject);
        }

        foreach (GameObject player in everyPlayer)
        {
            // ��������� �������� ���� ���������
            characterRole = player.GetComponent<CharacterRole>();
            characterRole.character = character;
            characterRole.currentHP = character.characerHitPoint;
        }
    }
}
