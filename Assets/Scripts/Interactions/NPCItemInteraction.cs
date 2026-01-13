using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

//handles NPCs that can take/react to being given items
public class NPCItemInteraction : ItemInteraction
{
    public string storyName;

    new private void Start()
    {
        base.Start();
        localizedUseText.TableReference = "UI";
        localizedUseText.TableEntryReference = "item_npc_comment";
    }

    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        //set story variable (name = comment) to true
        if(interaction.varName != "")
        {
            gm.SetSpecificVar(storyName, interaction.varName);
        }
        gameObject.GetComponent<DialogueTrigger>().StartDialogue();
        gm.dialogue.GetAllVars();
        activeItem.GetComponent<InventoryItem>().RemoveItem();
    }

    protected override void HoverText()
    {
        objName = gameObject.GetComponent<DialogueTrigger>().NPCName.GetLocalizedString();
        var dict = new Dictionary<string, object>
        {
            { "item", activeItem.displayName },
            { "name", objName }

        };
        localizedUseText.Arguments = new object[] { dict };
        gm.SetActionText(localizedUseText.GetLocalizedString());
    }

    public void Action()
    {
        //animation
    }
}
