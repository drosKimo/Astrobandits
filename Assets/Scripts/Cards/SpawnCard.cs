using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    GameObject objParent;
    [HideInInspector] public GameObject newItem;
    GetCardItem cardItem;

    public void Spawn()
    {
        objParent = GameObject.Find("Elements Container");
        cardItem = prefab.GetComponent<GetCardItem>();
        cardItem.calling = true;
        newItem = GameObject.Instantiate(prefab);
        GetItemName();
    }

    // Временно для дебага
    // позволяет заспавнить карту по индексу
    public void SpawnByIndex(int setCardIndex)
    {
        objParent = GameObject.Find("Elements Container");
        GetCardItem cardItem = prefab.GetComponent<GetCardItem>();
        cardItem.cardIndex = setCardIndex;
        cardItem.calling = false;

        newItem = GameObject.Instantiate(prefab);
        GetItemName();
    }

    public void EnemyGetsCard()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            CharacterRole role = enemy.GetComponent<CharacterRole>();
            role.DrawCard();
        }
    }

    void GetItemName()
    {
        // большая проверка, чтобы карты не ломались
        if (objParent.transform.childCount == 0)
        {
            newItem.name = "Item" + $" {objParent.transform.childCount + 1}";
        }
        else
        {
            // начальное имя - имя последней карты в очереди
            string startName = objParent.transform.GetChild(objParent.transform.childCount - 1).name;
            string finalName = startName;

            if (objParent.transform.childCount > 1)
            {
                // последнее имя - имя предпоследней карты
                string lastName = objParent.transform.GetChild(objParent.transform.childCount - 2).name;

                int firstIndex = System.Convert.ToInt32(startName.Substring(5));
                int lastIndex = System.Convert.ToInt32(lastName.Substring(5));

                // если индекс последней карты больше предпоследней, от него идет новый счет карт
                if (firstIndex > lastIndex)
                    finalName = startName;
                else finalName = lastName;
            }

            int cardIndex = System.Convert.ToInt32(finalName.Substring(5)) + 1;
            newItem.name = "Item " + cardIndex;
        }

        newItem.transform.SetParent(objParent.transform, false);
    }
}
