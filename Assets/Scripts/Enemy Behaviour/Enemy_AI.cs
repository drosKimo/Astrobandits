using System.Collections;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public IEnumerator EnemyTurn()
    {
        TurnManager manager = GameObject.Find("WhenGameStarts").GetComponent<TurnManager>();
        yield return new WaitForSeconds(1f);

        manager.EndTurn(); // заканчивает ход
        yield return null;
    }
}
