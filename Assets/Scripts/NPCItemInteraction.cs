using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles NPCs that can take/react to being given items
public class NPCItemInteraction : ItemInteraction
{
    public string storyName;

    void Start()
    {
        gm = GameManager.Instance;
    }

    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        //set story variable to true according to comment
        if(interaction.comment != "")
        {
            gm.SetSpecificVar(storyName, interaction.comment);
        }
        gameObject.GetComponent<DialogueTrigger>().StartDialogue();
    }

    protected override void HoverText()
    {
        //consider translation
        gm.SetText(activeItem.id + " an " + objName + " geben.");
    }

    public void Action()
    {
        //animation
    }
}
