using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData refItem;
    public Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
            handlePickup();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            handlePickup();
        }
    }

    public void handlePickup()
    {
        inventory.addItem(refItem);
        Destroy(gameObject);
    }
}
