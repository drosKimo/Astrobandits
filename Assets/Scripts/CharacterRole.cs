using UnityEngine;

// хранилище, в котором хранятся данные и персонаже и роли игрока
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP;
    public Roles role;
    [Tooltip("Номер персонажа в очереди")][Range(0, 6)] public int queueNumber;
}
