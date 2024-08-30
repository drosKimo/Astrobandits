using UnityEngine;

public class HelperData : MonoBehaviour
{
    // для CardProperty
    [HideInInspector] public bool isChallenge = false,
                                  challengeDone, 
                                  shotDone, // ограничитель для игрока
                                  playerImplantSet; // проверка, установлен ли имплант

    [HideInInspector] public Enemy_AI challenge_AI;

    [HideInInspector] public int baseDistance = 1;
}
