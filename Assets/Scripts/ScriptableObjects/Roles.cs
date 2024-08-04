using UnityEngine;

[CreateAssetMenu(fileName = "New Role", menuName = "ScriptableObjects/Role")]
public class Roles : ScriptableObject
{
    [Tooltip("Ключ локализации имени")] public string roleName;
    public Sprite roleImage;
    
    [Tooltip(   "Ключ взаимодействия:\n" +
                "0 = нет приоритета\n" +
                "1 = приоритет на капитане\n" +
                "2 = приоритет на всех, кроме капитана")]
    [Range(0, 2)] public int rolePriority;

    [Tooltip("Игра закончится, если этот персонаж умрет?")] public bool roleEnd = false;
}
