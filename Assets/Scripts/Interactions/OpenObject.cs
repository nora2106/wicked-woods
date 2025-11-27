using UnityEngine;

//open object to show detail view or a different object/sprite
public class OpenObject : UsableObject, IInteractionCommand
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
        if (targetPos != null)
        {
            gm.movementTargetPos = targetPos;
        }
        gm.QueueInteraction(new ActionCommand(Execute));
    }

    // function is called after movement is completed
    public void Execute ()
    {
        if(isOpen)
        {
            Close();
            return;
        }
        if(detail != null)
        {
            detail.GetComponent<DetailView>().Open();
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

    public void Close() {
        if(!closeOnSecondClick)
        {
            return;
        }
        isOpen = false;
        if (newSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
    }
}
