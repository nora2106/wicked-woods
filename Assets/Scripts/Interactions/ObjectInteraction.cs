using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

//handles interactable objects like a locked chest that can be interacted with with items
public class ObjectInteraction : ItemInteraction
{
    private string comment;

    //possibly rework this to handle multiple actions with different items
    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        comment = localizedDefaultComment.GetLocalizedString();
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
            comment = interaction.comment.GetLocalizedString();
        }
        gm.QueueInteraction(new SetTextCommand(comment));
    }

    protected override void HoverText()
    {
        var dict = new Dictionary<string, object>
        {
            { "item", activeItem.displayName },
            { "object", objName }

        };
        localizedUseText.Arguments = new object[] { dict };
        gm.SetActionText(localizedUseText.GetLocalizedString());
    }
}
