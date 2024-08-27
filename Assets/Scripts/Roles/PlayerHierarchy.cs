using UnityEngine;

public class PlayerHierarchy : MonoBehaviour
{
    // необходим для расчета расстояния между игроками
    public int CalculateCircularDistance(Transform player1, Transform player2)
    {
        // проверяем, что оба игрока имеют одного и того же родителя
        if (player1.parent != player2.parent)
        {
            Debug.LogWarning("Убедитесь, что оба игрока имеют один родительский объект");
            return -1;
        }

        // получаем индексы игроков
        int index1 = player1.GetSiblingIndex();
        int index2 = player2.GetSiblingIndex();

        // получаем количество игроков (количество детей родителя)
        int totalPlayers = player1.parent.childCount;

        // рассчитываем прямое и обратное расстояние, чтобы определить минимальное расстояние между ними
        int directDistance = Mathf.Abs(index1 - index2);
        int circularDistance = Mathf.Min(directDistance, totalPlayers - directDistance);

        Debug.Log($"Расстояние между {player1.name} и {player2.name} равно {circularDistance}");
        return circularDistance;
    }
}
