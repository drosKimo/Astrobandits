using UnityEngine;

public class PlayerHierarchy : MonoBehaviour
{
    // ��������� ��� ������� ���������� ����� ��������
    public int CalculateCircularDistance(Transform player1, Transform player2)
    {
        // ���������, ��� ��� ������ ����� ������ � ���� �� ��������
        if (player1.parent != player2.parent)
        {
            Debug.LogWarning("���������, ��� ��� ������ ����� ���� ������������ ������");
            return -1;
        }

        // �������� ������� �������
        int index1 = player1.GetSiblingIndex();
        int index2 = player2.GetSiblingIndex();

        // �������� ���������� ������� (���������� ����� ��������)
        int totalPlayers = player1.parent.childCount;

        // ������������ ������ � �������� ����������, ����� ���������� ����������� ���������� ����� ����
        int directDistance = Mathf.Abs(index1 - index2);
        int circularDistance = Mathf.Min(directDistance, totalPlayers - directDistance);

        Debug.Log($"���������� ����� {player1.name} � {player2.name} ����� {circularDistance}");
        return circularDistance;
    }
}
