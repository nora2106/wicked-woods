using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

//handle object interaction with active item (parent class)
public abstract class ItemInteraction : MonoBehaviour
{
    public LocalizedString localizedDefaultComment;
    public LocalizedString localizedUseText;
    public InteractionData interactionData;
    protected InventoryItem activeItem;
    protected GameManager gm;
    [HideInInspector] public string objName;
    protected string defaultComment;
    [HideInInspector] public LocalizedString localizedName;

    private void Start()
    {
        gm = GameManager.Instance;

        //get and initialize localized use
        localizedUseText.TableReference = "UI";
        localizedUseText.TableEntryReference = "item_use_text";
        var dict = new Dictionary<string, object>
        {
            { "item", "" },
            { "object", "" }

        };
        localizedUseText.Arguments = new object[] { dict };

        localizedDefaultComment.StringChanged += updateDefaultComment;
        localizedUseText.StringChanged += updateUseText;
    }

    private void Update()
    {
        //necessary because active item blocks onmousedown
        if(activeItem != null && Input.GetKeyDown(KeyCode.Mouse0)) {
            Interaction();
        }
    }

    //handle interaction only if item is in list of item interactions
    public void Interaction()
    {
        if (activeItem != null)
        {
            foreach (var interaction in interactionData.itemInteractions)
            {
                if (interaction.requiredItem == activeItem.id)
                {
                    HandleInteraction(interaction);
                    return;
                }
                /* else
                {
                    gm.SetText(defaultInteractionText);
                    print("default text");
                } */
            }
        }
    }

    //handle item interaction based on object interaction data
    public abstract void HandleInteraction(InteractionData.ItemInteraction interaction);

    //text when hovering over object with active item
    protected abstract void HoverText();

    //handle item entering & exiting object collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InventoryItem>())
        {
            activeItem = collision.gameObject.GetComponent<InventoryItem>();
            HoverText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeItem = null;
    }

    // TODO replace with lambda expressions
    private void updateDefaultComment(string val)
    {
        defaultComment = val;
    }

    private void updateUseText(string val)
    {
        defaultComment = val;
    }
}
