using System.Collections;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    CharacterRole characterRole;

    void Start()
    {
        StartCoroutine(DrawPlayerCard());
    }

    IEnumerator DrawPlayerCard()
    {
        yield return new WaitForSeconds(0.5f); // �������� ����� �������� �����

        characterRole = GetComponent<CharacterRole>();

        // ��������� � ���� ������� ����, ������� �� � ������
        for (int i = 0; i < characterRole.currentHP; i++)
        {
            characterRole.DrawCard();
        }
        yield return null;
    }
}
