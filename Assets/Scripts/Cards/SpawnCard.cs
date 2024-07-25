using Akassets.SmoothGridLayout;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    GameObject objParent;
    LerpToPlaceholder toPlaceholder;
    public void Spawn()
    {
        //Debug.Log("spawn");
        objParent = GameObject.Find("Elements Container");

        GameObject newItem;
        newItem = GameObject.Instantiate(prefab);
        newItem.name = "Item" + $" {objParent.transform.childCount + 1}";
        newItem.transform.SetParent(objParent.transform, false);
    }
}
