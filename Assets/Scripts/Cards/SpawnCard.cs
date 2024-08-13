using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    [SerializeField] public GameObject SCprefab;
    GameObject objParent;
    [HideInInspector] public GameObject newItem;
    GetCardItem cardItem;

    public void Spawn()
    {
        objParent = GameObject.Find("Elements Container");
        cardItem = SCprefab.GetComponent<GetCardItem>();
        cardItem.calling = true;
        newItem = GameObject.Instantiate(SCprefab);
        GetItemName();
    }

    // �������� ��� ������
    // ��������� ���������� ����� �� �������
    public void SpawnByIndex(int setCardIndex)
    {
        objParent = GameObject.Find("Elements Container");
        GetCardItem cardItem = SCprefab.GetComponent<GetCardItem>();
        cardItem.cardIndex = setCardIndex;
        cardItem.calling = false;

        newItem = GameObject.Instantiate(SCprefab);
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

    public void GetItemName()
    {
        // ������� ��������, ����� ����� �� ��������
        if (objParent.transform.childCount == 0)
        {
            newItem.name = "Item" + $" {objParent.transform.childCount + 1}";
        }
        else
        {
            // ��������� ��� - ��� ��������� ����� � �������
            string startName = objParent.transform.GetChild(objParent.transform.childCount - 1).name;
            string finalName = startName;

            if (objParent.transform.childCount > 1)
            {
                // ��������� ��� - ��� ������������� �����
                string lastName = objParent.transform.GetChild(objParent.transform.childCount - 2).name;

                int firstIndex = System.Convert.ToInt32(startName.Substring(5));
                int lastIndex = System.Convert.ToInt32(lastName.Substring(5));

                // ���� ������ ��������� ����� ������ �������������, �� ���� ���� ����� ���� ����
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
