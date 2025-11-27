using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

//handle object interaction with active item (parent class)
public abstract class ItemInteraction : MonoBehaviour
{
    public InteractionData interactionData;
    protected InventoryItem activeItem;
    protected GameManager gm;
    protected string defaultComment;

    [HideInInspector] public string objName;
    [HideInInspector] public LocalizedString localizedDefaultComment;
    [HideInInspector] public LocalizedString localizedUseText;
    [HideInInspector] public LocalizedString localizedName;

    protected void Start()
    {
        gm = GameManager.Instance;

        //initialize static localized texts
        localizedDefaultComment.TableReference = "UI";
        localizedDefaultComment.TableEntryReference = "default_use_comment";
        localizedUseText.TableReference = "UI";
        localizedUseText.TableEntryReference = "item_use_text";
        localizedName.StringChanged += UpdateName;
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
        gm.ResetActionText();
    }

    //only add setter functions for identifiying names
    protected void UpdateName(string val)
    {
        objName = val;
    }
}