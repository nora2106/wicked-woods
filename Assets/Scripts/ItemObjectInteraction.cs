using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//handles interactable objects like a locked chest that can be interacted with with items
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
        }
        else if(interaction.comment != null)
        {
            gm.SetText(interaction.comment);
        }
    }

    protected override void HoverText()
    {
        gm.SetText(activeItem.displayName + " mit " + objName + " benutzen.");
    }
}
