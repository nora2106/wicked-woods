using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour
{
    public int count = 0;
    public InventoryItem item;

    void Update()
    {
        if(transform.childCount == 1 && item == null)
        {
            item = transform.GetChild(0).gameObject.GetComponent<InventoryItem>();
        }

        else
        {
            item = null;
        }
    }

    public void getClick()
    {
        if(item)
        {
            item.GetComponent<InventoryItem>().selected = true;
        }
    }
}
