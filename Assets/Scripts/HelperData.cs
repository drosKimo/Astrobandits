using UnityEngine;

public class HelperData : MonoBehaviour
{
    // для CardProperty
    [HideInInspector] public bool isChallenge = false,
                                  challengeDone,
                                  shotDone; // ограничитель выстрела для игрока

    [HideInInspector] public Enemy_AI challenge_AI;
    [HideInInspector] public CharacterRole enemyTransHP; // получает противника, на которого сыграна карта Гемотрансфузия

    [HideInInspector] public int baseDistance = 1;
}
