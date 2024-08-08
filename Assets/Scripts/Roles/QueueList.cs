using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QueueList : MonoBehaviour
{
    bool capFound = false;

    public void SetQueueList()
    {
        GameObject list = GameObject.Find("Enemies");
        List<GameObject> GOlist = new List<GameObject>();

        // каждого противника в список
        for (int i = 0; i < list.transform.childCount; i++)
        {
            GOlist.Add(list.transform.GetChild(i).gameObject);
        }

        // ищет капитана
        foreach (GameObject cap in GOlist)
        {
            CharacterRole characterRole = cap.GetComponent<CharacterRole>();

            if (characterRole.role.roleName == "Roles.Name.Captain")
            {
                capFound = true; // теперь капитан найден
                cap.GetComponent<SpriteRenderer>().color = Color.red; // меняет цвет кэпа
            }
            else if (!capFound)
                cap.transform.SetAsLastSibling(); // пока кэп не найден, ставит текущего игрока крайним в очереди
        }
    }
}
