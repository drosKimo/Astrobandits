using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [HideInInspector] public CharacterRole target;

    // ������ ������ ����������
    public IEnumerator EnemyTurn()
    {
        TurnManager manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        CharacterRole characterRole = GetComponent<CharacterRole>();
        PlayCard playCard = GetComponent<PlayCard>();

        foreach (Cards card in characterRole.hand)
        {
            // ��������. ���� � ���������� ���� ����� ��������, �� ���������� �� �� ������
            if (card.itemName == "Cards.Name.Pow")
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRole>();
                playCard.Pow();
            }
        }

        yield return new WaitForSeconds(1f); // �������� ����� ���������� ����

        manager.EndTurn(); // ����������� ���
    }
}
