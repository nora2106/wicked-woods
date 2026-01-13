using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueTrigger : MonoBehaviour, ActionAfterMovement
{
    [SerializeField] private TextAsset dialogueJSON;
    private GameManager gm;
    LocalizedString dialogueText = new LocalizedString();
    public LocalizedString NPCName;

    private void Start()
    {
        gm = GameManager.Instance;
        dialogueText.TableReference = "UI";
        dialogueText.TableEntryReference = "npc_talk_text";
    }

    private void OnMouseDown()
    {
        if (!gm.itemSelected)
        {
            gm.QueueInteraction(new ActionCommand(StartDialogue));
        }
    }

    private void OnMouseOver()
    {
        if(!gm.itemSelected)
        {
            string localizedName = NPCName.GetLocalizedString();
            var dict = new Dictionary<string, object>
            {
                { "name", localizedName}

            };
            dialogueText.Arguments = new object[] { dict };
            gm.SetActionText(dialogueText.GetLocalizedString());

        }
    }

    //reset action text when mouse leaves npc
    private void OnMouseExit()
    {
        gm.ResetActionText();
    }

    public void ActionAfterMovement()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        GameManager.Instance.OpenDialogue(dialogueJSON);
    }
}
