using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, ISetup
{
    public ItemData refItem;
    public Inventory inventory;
    private GameManager gm;
    public string id;
    // Start is called before the first frame update

    public void Setup()
    {
        gm = GameManager.Instance;

        if(gm.gameData.collectedItems.Contains(id))
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
        inventory.addItem(refItem);
        gm.gameData.collectedItems.Add(id);
        Destroy(gameObject);
    }
}
