using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Localization;

//open object to show detail view
public class OpenObject : UsableObject
{
    public GameObject detail;

    new void Start()
    {
        base.Start();
        if(locked) {
            gameObject.tag = "inspect";
        }
        if(gameObject.GetComponent<ItemInteraction>())
        {
            id = gameObject.GetComponent<ItemInteraction>().interactionData.objectID;
        }
    }

    //open object (only called when unlocked)
    public override void Action()
    {
        OpenAnimation();
        if (detail != null)
        {
            detail.GetComponent<DetailView>().Open();
        }
    }

    public void Close() {
        //close animation
    }

    override public void OpenAnimation() {
        //open animation
    }
}
