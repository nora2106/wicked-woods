using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

//handles interactable objects like a locked chest that can be interacted with with items
public class ObjectInteraction : ItemInteraction
{
    private string comment;

    //possibly rework this to handle multiple actions with different items
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
        else if(!interaction.comment.IsEmpty)
        {
            interaction.comment.StringChanged += UpdateComment;
            gm.SetText(comment);
        }
    }

    protected override void HoverText()
    {
        //consider translations
        gm.SetText(activeItem.displayName + " mit " + objName + " benutzen.");
    }

    private void UpdateComment(string var)
    {
        comment = var;
    }
}
