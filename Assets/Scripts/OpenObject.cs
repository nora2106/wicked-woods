using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

//open object to show detail view
public class OpenObject : UsableObject
{
    public GameObject detail;

    void Start()
    {
        if(locked) {
            gameObject.tag = "inspect";
        }
    }

    public override void Action()
    {
        if(!locked) {
          OpenAnimation();
          if(detail != null) {
                detail.GetComponent<DetailView>().Open();
          }
        }
        else if(GetComponent<ObjectInteraction>()) {
            GetComponent<ObjectInteraction>().HandleInteraction("");
        }
    }

    public void Close() {
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
    }

    override public void OpenAnimation() {
        //add animation
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
    }
}
