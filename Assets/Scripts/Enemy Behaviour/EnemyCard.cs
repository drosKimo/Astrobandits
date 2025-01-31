using System.Collections;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    GetCardItem cardItem;
    CharacterRole characterRole;

    void Awake()
    {
        // ���� ����� � ���� ��� ������ ���������� ������ ����
        StartCoroutine(DrawEnemyCard());
    }

    IEnumerator DrawEnemyCard()
    {
        yield return new WaitForEndOfFrame(); // �������� ����� �������� �����

        characterRole = GetComponent<CharacterRole>();

        // ��������� � ���� ������� ����, ������� �� � ������
        for (int i = 0; i < characterRole.currentHP; i++)
        {
            characterRole.DrawCard();
        }
        yield return null;
    }
}
