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
    private DetailView detailView;
    public bool destroyOnPuzzleComplete = false;

    new void Start()
    {
        base.Start();
        baseSprite = GetComponent<SpriteRenderer>().sprite;
        if (locked)
        {
            gameObject.tag = "inspect";
        }
        if (gameObject.GetComponent<ItemInteraction>())
        {
            id = gameObject.GetComponent<ItemInteraction>().interactionData.objectID;
        }
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
        if (detail != null)
        {
            detailView = detail.GetComponent<DetailView>();
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
    public void Execute()
    {
        if (isOpen && closeOnSecondClick)
        {
            Close();
            return;
        }
        if (detailView != null)
        {
            detailView.onClose.AddListener(Close);
            detailView.Open();
        }
        if (newSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        if (showItem != null)
        {
            showItem.SetActive(true);
        }
        // detail view has puzzle
        if (detail.GetComponent<PuzzleManager>())
        {
            detail.GetComponent<PuzzleManager>().OnPuzzleSolved.AddListener(HandlePuzzleComplete);
        }
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
        if (newSprite != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
        if (detailView != null)
        {
            detailView.onClose.RemoveListener(Close);
        }
    }

    // handles puzzle completion if detail view is a puzzle
    void HandlePuzzleComplete()
    {
        detail.GetComponent<PuzzleManager>().OnPuzzleSolved.RemoveListener(HandlePuzzleComplete);
        
        if (destroyOnPuzzleComplete)
        {
            // door-specific cleanup - enable door behaviour
            if (gameObject.GetComponent<DoorObject>())
            {
                gameObject.GetComponent<DoorObject>().disabled = false;
                if (gameObject.transform.childCount > 0)
                {
                    Destroy(gameObject.transform.GetChild(0).gameObject);
                }
            }
            Destroy(gameObject.GetComponent<OpenObject>());
        }
    }
}
