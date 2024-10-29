using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : ItemInteraction
{
    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        if (interaction.action) { 

            if(gameObject.GetComponent<UsableObject>().locked)
            {
                gameObject.GetComponent<UsableObject>().Unlock();
            }
            else
            {
                gameObject.GetComponent<UsableObject>().Action();
            }
            activeItem.GetComponent<InventoryItem>().RemoveItem();

        }
        else
        {
            gm.SetText(interaction.comment);
        }
    }

    protected override void HoverText()
    {
        gm.SetText(activeItem.data.displayName + " mit " + objName + " benutzen.");
    }
}
