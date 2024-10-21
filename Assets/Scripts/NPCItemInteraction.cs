using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCItemInteraction : MonoBehaviour
{
    private GameManager gm;
    public InteractionData interactionData;
    public string defaultText;
    public string storyName;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    void OnMouseDown()
    {
        HandleInteraction(gm.selectedItemID);
    }

    public void HandleInteraction(string selectedItem)
    {
        if(selectedItem != "")
        {
            foreach (var interaction in interactionData.itemInteractions)
            {
                if (interaction.requiredItem == selectedItem)
                {
                    print("action");
                    gm.SetSpecificVar(storyName, interaction.comment);
                    return;
                }
                gm.SetText(defaultText);
            }
        }
    }
}
