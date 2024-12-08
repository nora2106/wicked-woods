using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

//handles NPCs that can take/react to being given items
public class NPCItemInteraction : ItemInteraction
{
    public string storyName;

    void Start()
    {
/*         localizedUseText.TableReference = "UI";
        localizedUseText.TableEntryReference = "item_use_text"; */
        gm = GameManager.Instance;
    }

    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        //set story variable to true according to comment
        if(interaction.varName != "")
        {
            gm.SetSpecificVar(storyName, interaction.varName);
        }
        gameObject.GetComponent<DialogueTrigger>().StartDialogue();
    }

    protected override void HoverText()
   { 
        // TODO consider translation
        gm.SetText(activeItem.displayName + " an " + objName + " geben.");
    }

    public void Action()
    {
        //animation
    }
}
