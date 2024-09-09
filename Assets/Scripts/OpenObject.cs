using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//open object to make it inspectable/detail view
public class OpenObject : UsableObject
{
    private bool open;
    public Sprite sprite1;
    public Sprite sprite2;
    public GameObject detail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action()
    {
        if(open == false){
            open = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
            gameObject.AddComponent<InspectObject>();
            gameObject.GetComponent<InspectObject>().viewObject = detail;
        }
        else {
            open = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
            Destroy(gameObject.GetComponent<InspectObject>());
        }
    }
}
