using Akassets.SmoothGridLayout;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string cardName = ""; // имя текущей карты
    GameObject placeholder, thisCard;
    LerpToPlaceholder LtP;
    [HideInInspector] public int trans;
    [SerializeField] int riseCard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // получает объекты под курсором
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        // выводит список результатов
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // перебирает список результатов
        results.ForEach((result) => 
        { 
            // пытается понять, является ли этот объект кнопкой
            Button btn = result.gameObject.GetComponent<Button>();
            if (btn != null) // если объект имеет компонент "Кнопка"
            {
                // получает объект
                thisCard = result.gameObject;
            }
        });

        cardName = thisCard.name;
        LtP = thisCard.GetComponent<LerpToPlaceholder>();

        // отключает плейсхолдер и скрипт, который к нему тянет
        LtP.enabled = false;
        float cardY = thisCard.transform.position.y + riseCard;

        trans = thisCard.transform.GetSiblingIndex(); // текущее положение в иерархии
        thisCard.transform.SetAsLastSibling(); // ставит объект последним в иерархии, чтобы он был поверх остальных объектов
        placeholder = GameObject.Find(cardName + " placeholder"); // теперь ищет его плейсхолдер
        placeholder.transform.SetSiblingIndex(trans); // ставит плейсхолдер на место карты, чтобы она не улетала

        thisCard.transform.position = new Vector2(thisCard.transform.position.x, cardY); // поднимает карту
        thisCard.transform.localScale = new Vector3(1.25f, 1.25f, 1); // немного увеличивает карту
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisCard.transform.localScale = new Vector3(1, 1, 1);
        thisCard.transform.SetSiblingIndex(trans);
        LtP.enabled = true;
    }
}
