using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, ISetup
{
    public ItemData refItem;
    private GameManager gm;
    [HideInInspector] public string id;

    public void Setup()
    {
        gm = GameManager.Instance;
        if (gm.save.data.collectedItems.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    // either pick up directly or wait for movement, depending on if player can move
    private void OnMouseDown()
    {
        if(gm.movement.canMove)
        {
            gm.QueueInteraction(new ActionCommand(PickUp));
        }

        else
        {
            PickUp();
        }
    }

    // pick up item
    public void PickUp()
    {
        gm.inventory.addItem(refItem);
        gm.save.data.collectedItems.Add(id);
        Destroy(gameObject);
    }
}
