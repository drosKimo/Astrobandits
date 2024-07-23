using Akassets.SmoothGridLayout;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowCard : MonoBehaviour
{
    string cardName = ""; // ��� ������� �����
    GameObject placeholder, thisCard;
    LerpToPlaceholder LtP;
    int trans;

    public void GetCard()
    {
        // �������� ������� ��� ��������
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        // ������� ������ �����������
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // ���������� ������ �����������
        results.ForEach((result) => 
        { 
            // �������� ������, �������� �� ���� ������ �������
            Button btn = result.gameObject.GetComponent<Button>();
            if (btn != null) // ���� ������ ����� ��������� "������"
            {
                // �������� ������
                thisCard = result.gameObject;
            }
        });

        cardName = thisCard.name;
        LtP = thisCard.GetComponent<LerpToPlaceholder>();

        // ��������� ����������� � ������, ������� � ���� �����
        LtP.enabled = false;
        float cardY = thisCard.transform.position.y + 20;

        trans = thisCard.transform.GetSiblingIndex(); // ������� ��������� � ��������
        thisCard.transform.SetAsLastSibling(); // ������ ������ ��������� � ��������, ����� �� ��� ������ ��������� ��������
        placeholder = GameObject.Find(cardName + " placeholder"); // ������ ���� ��� �����������
        placeholder.transform.SetSiblingIndex(trans); // ������ ����������� �� ����� �����, ����� ��� �� �������

        thisCard.transform.position = new Vector2(thisCard.transform.position.x, cardY); // ��������� �����
    }

    public void ReleaseCard()
    {
        thisCard.transform.SetSiblingIndex(trans);
        LtP.enabled = true;
    }
}
