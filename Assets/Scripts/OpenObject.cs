using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

//open object to make it inspectable/detail view
public class OpenObject : UsableObject
{
    public bool open;
    public Sprite sprite1;
    public Sprite sprite2;
    public GameObject detail;
    public string lockedText;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GameObject.FindWithTag("Text").GetComponent<Text>();
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
        if(locked) {
            gameObject.tag = "inspect";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action()
    {
        if(!locked && !open) {
            OpenAnimation();
/*             displayText.text = "";
            if(gameObject.tag == "inspect") {
                gameObject.tag = "Untagged";
            } */
        }
        else if (open) {
            if(detail != null) {
                detail.GetComponent<DetailView>().Open();
            }
        }
        else {
            displayText.text = lockedText;
        }
    }

    public void Close() {
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
        open = false;
    }

    override public void OpenAnimation() {
        //add animation
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
        open = true;
    }
}
