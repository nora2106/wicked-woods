using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData refItem;
    public Inventory inventory;
    private GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.FindWithTag("Cursor");
    }

    private void OnMouseDown()
    {
        handlePickup();
    }

    public void handlePickup()
    {
        inventory.addItem(refItem);
        Destroy(gameObject);
    }
}
