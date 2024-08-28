using UnityEngine;
using UnityEngine.UI;

public class ItemContainers : MonoBehaviour
{
    int defaultItems = 2, // ����������� ����� ��������� �� ���������� ����������
        spacingItems; // ���������� ��������

    Vector2 spacingOffset, // ����� �������� �� �������������
            spacingDefault; // ������� ��������

    GridLayoutGroup gridLayoutGroup;

    void Awake()
    {
        // �������� ��������� �����, ����� �������� ����������
        gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

        spacingDefault = gridLayoutGroup.spacing;
    }

    void LateUpdate()
    {
        spacingItems = gameObject.transform.childCount; // ���������� �������� ��������

        // �������� �� 18 ����� ���� ������ �������� ����� ��������, �� �� ������ ���������� ���� �� ������ (������ �����)
        if (spacingItems > defaultItems)
        {
            int restItems = spacingItems - defaultItems;
            spacingOffset.x = spacingDefault.x - (10 * restItems);

            gridLayoutGroup.spacing = new Vector2(spacingOffset.x, 0);
        }
        else
            gridLayoutGroup.spacing = spacingDefault;
    }

    /*private void ItemsCounter()
    {
        spacingItems = elementsContainer.gameObject.transform.childCount;
        ItemsCounter();
        Debug.Log(spacingItems);

        if (spacingItems > 5)
        {
            //Debug.Log(gridLayoutGroup.spacing.x - 5);
            Vector2 aaa = new Vector2((gridLayoutGroup.spacing.x - 5), 0);

            gridLayoutGroup.spacing.Set(aaa.x, 0);
            Debug.Log(aaa.x);
        }
    }*/
}
