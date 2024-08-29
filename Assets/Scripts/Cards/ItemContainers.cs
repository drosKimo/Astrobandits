using UnityEngine;
using UnityEngine.UI;

public class ItemContainers : MonoBehaviour
{
    int defaultItems = 3, // минимальное число предметов до уменьшения расстояния
        spacingItems; // количество объектов

    Vector2 spacingOffset, // какое значение мы устанавливаем
            spacingDefault; // базовое значение

    GridLayoutGroup gridLayoutGroup;

    void Awake()
    {
        // получаем компонент сетки, чтобы изменять расстояние
        gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

        spacingDefault = gridLayoutGroup.spacing;
    }

    void LateUpdate()
    {
        spacingItems = gameObject.transform.childCount; // количество дочерних объектов

        // примерно на 18 карте этот способ начинает плохо работать, но до такого количества карт не дойдет (скорее всего)
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
