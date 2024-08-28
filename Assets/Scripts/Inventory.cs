using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public List<ItemData> items;
    public GameObject slotPrefab;
    public GameObject itemPrefab;

    public void addItem(ItemData refData)
    {
        items.Add(refData);
        GameObject ob = Instantiate(itemPrefab);
        ob.transform.SetParent(GetFreeSlot(), false);
        ob.gameObject.GetComponent<InventoryItem>().data = refData;
    }

    public Transform GetFreeSlot()
    {
        foreach (Transform child in transform)
        {
            if(child.childCount == 0)
            {
                return child;
            }
        }
        return null;
    }

    public InventoryItem Get(ItemData refData)
    {
        // return item by itemdata id
        return null;
    }
}
