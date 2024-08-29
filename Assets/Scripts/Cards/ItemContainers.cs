using UnityEngine;
using UnityEngine.UI;

public class ItemContainers : MonoBehaviour
{
    int defaultItems = 3, // ����������� ����� ��������� �� ���������� ����������
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
}
