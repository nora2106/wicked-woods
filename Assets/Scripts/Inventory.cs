using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour, ISetup
{
    public List<ItemData> items = new();
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    private GameManager gm;

    public void Setup()
    {

        gm = GameManager.Instance;
        if (gm.save.data.inventoryItems.Count > 0)
        {
            foreach(ItemData idata in gm.save.data.inventoryItems)
            {
                if(!items.Contains(idata))
                {
                    addItem(idata);
                }
            }
        }
    }

    public void addItem(ItemData refData)
    {
        items.Add(refData);
        GameObject ob = Instantiate(itemPrefab);
        ob.transform.SetParent(GetFreeSlot(), false);
        ob.gameObject.GetComponent<InventoryItem>().data = refData;
        
        if(!gm.save.data.inventoryItems.Contains(refData)) {
            gm.save.data.inventoryItems.Add(refData);
        }
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
