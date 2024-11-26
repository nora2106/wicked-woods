using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

//handle object interaction with active item (parent class)
public abstract class ItemInteraction : MonoBehaviour
{
    public string defaultInteractionText = "Das funktioniert nicht.";
    public InteractionData interactionData;
    protected InventoryItem activeItem;
    protected GameManager gm;
    [HideInInspector] public string objName;
    public LocalizedString localizedName;

    private void Start()
    {
        gm = GameManager.Instance;
        localizedName.StringChanged += updateName;
    }

    private void Update()
    {
        //necessary because active item blocks onmousedown
        if(activeItem != null && Input.GetKeyDown(KeyCode.Mouse0)) {
            Interaction();
        }
    }

    void OnMouseDown()
    {
        if(interactionData.defaultComment != "")
        {
            gm.SetText(interactionData.defaultComment);
        }
    }

    public void Interaction()
    {
        if (activeItem != null)
        {
            foreach (var interaction in interactionData.itemInteractions)
            {
                if (interaction.requiredItem == activeItem.id)
                {
                    HandleInteraction(interaction);
                    activeItem.GetComponent<InventoryItem>().RemoveItem();
                    return;
                }
                gm.SetText(defaultInteractionText);
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

    private void updateName(string val)
    {
        objName = val;
    }

    private void OnDestroy()
    {
        localizedName.StringChanged -= updateName;
    }
}
