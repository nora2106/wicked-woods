using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInteraction : MonoBehaviour
{
    public string defaultInteractionText = "Das funktioniert nicht.";
    public InteractionData interactionData;
    protected InventoryItem activeItem;
    protected GameManager gm;
    public string objName;

    private void Start()
    {
        gm = GameManager.Instance;
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
        gm.SetText(interactionData.defaultComment);
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
                    return;
                }
                gm.SetText(defaultInteractionText);
            }
        }
    }

    public abstract void HandleInteraction(InteractionData.ItemInteraction interaction);

    protected abstract void HoverText();

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
}
