using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

//handles NPCs that can take/react to being given items
public class NPCItemInteraction : ItemInteraction
{
    public string storyName;
    private string comment;

    void Start()
    {
        gm = GameManager.Instance;
    }

    public override void HandleInteraction(InteractionData.ItemInteraction interaction)
    {
        interaction.comment.StringChanged += updateComment;
        //set story variable to true according to comment
        if(comment != "")
        {
            gm.SetSpecificVar(storyName, comment);
        }
        gameObject.GetComponent<DialogueTrigger>().StartDialogue();
    }

    protected override void HoverText()
    {
        //consider translation
        gm.SetText(activeItem.displayName + " an " + objName + " geben.");
    }

    public void Action()
    {
        //animation
    }

    private void updateComment(string val)
    {
        comment = val;
    }
}
