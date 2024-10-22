using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, ISetup
{
    public ItemData refItem;
    private GameManager gm;
    public string id;

    public void Setup()
    {
        gm = GameManager.Instance;
        if (gm.save.data.collectedItems.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        handlePickup();
    }

    public void handlePickup()
    {
        gm.inventory.addItem(refItem);
        gm.save.data.collectedItems.Add(id);
        Destroy(gameObject);
    }
}
