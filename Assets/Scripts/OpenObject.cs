using UnityEngine;

//open object to show detail view or a different object/sprite
public class OpenObject : UsableObject, ActionAfterMovement
{
    //detail view gameobject to be opened
    public GameObject detail;
    //new sprite that current sprite is replaced with on open
    public Sprite newSprite;
    // item to be shown on open (expand to Array if needed)
    public GameObject showItem;
    public bool closeOnSecondClick;
    private bool isOpen = false;
    private Sprite baseSprite;

    new void Start()
    {
        base.Start();
        baseSprite = GetComponent<SpriteRenderer>().sprite;
        if(locked) {
            gameObject.tag = "inspect";
        }
        if(gameObject.GetComponent<ItemInteraction>())
        {
            id = gameObject.GetComponent<ItemInteraction>().interactionData.objectID;
        }
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
    }

    //open object (only called when unlocked)
    public override void Action()
    {
        if(!isOpen)
        {
            if (detail != null)
            {
                gm.movement.interactionObject = gameObject;
            }
            if (newSprite != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            if (showItem != null)
            {
                showItem.SetActive(true);
            }
            isOpen = true;
        }

        else if (closeOnSecondClick)
        {
            Close();
        }
    }

    public void ActionAfterMovement ()
    {
        detail.GetComponent<DetailView>().Open();
    }

    public void Close() {
        isOpen = false;
        if (newSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
        //close animation
    }

    override public void OpenAnimation() {
        //open animation
    }
}
