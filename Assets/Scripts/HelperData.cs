using UnityEngine;

public class HelperData : MonoBehaviour
{
    // ��� CardProperty
    [HideInInspector] public bool   isChallenge = false,
                                    challengeDone, 
                                    shotDone; // ������������ ��� ������

    [HideInInspector] public Enemy_AI challenge_AI;

    [HideInInspector] public int baseDistance = 1, currentDistance;
}
