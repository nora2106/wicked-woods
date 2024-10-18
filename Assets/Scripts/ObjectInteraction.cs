using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    private string defaultInteractionText = "Das funktioniert nicht.";
    private GameManager gm;
    //public bool wasInspected;
    public InteractionData interactionData;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    void OnMouseDown () {
        HandleInteraction(gm.selectedItemID);
    }

    public void HandleInteraction(string selectedItem)
    {
        if (interactionData.itemInteractions.Count > 0 && selectedItem != "")
        {
            foreach (var interaction in interactionData.itemInteractions)
            {
                if (interaction.requiredItem == selectedItem)
                {
                    gm.SetText(interaction.comment);
                    return;
                }
                gm.SetText(defaultInteractionText);
            }
        }
        else
        {
            gm.SetText(interactionData.defaultComment);
        }
    }
}
