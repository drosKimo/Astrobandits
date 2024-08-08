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
        yield return new WaitForSeconds(0.5f); // ожидание конца перворго кадра

        characterRole = GetComponent<CharacterRole>();

        // добавляет в руку столько карт, сколько хп у игрока
        for (int i = 0; i < characterRole.currentHP; i++)
        {
            characterRole.DrawCard();
        }
        yield return null;
    }
}
