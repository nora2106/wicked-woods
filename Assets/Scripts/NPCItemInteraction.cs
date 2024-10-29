using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCItemInteraction : ItemInteraction
{
    public string storyName;

    void Start()
    {
        gm = GameManager.Instance;
    }

    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        print("action");
        gm.SetSpecificVar(storyName, interaction.comment);
    }

    protected override void HoverText()
    {
        gm.SetText(activeItem.id + " an " + objName + " geben.");
    }

    public void Action()
    {
        //animation
    }
}
