using UnityEngine;

public class HelperData : MonoBehaviour
{
    // для CardProperty
    [HideInInspector] public bool   isChallenge = false,
                                    challengeDone, 
                                    shotDone; // ограничитель для игрока

    [HideInInspector] public Enemy_AI challenge_AI;

    [HideInInspector] public int baseDistance = 1, currentDistance;
}
