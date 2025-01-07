using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public ItemData data;
    public Inventory inventory;
    public string id;
    public string displayName;
    public LocalizedString localizedDisplayName;
    [HideInInspector] public bool selected = false;

    private Vector2 pos;
    private GameObject player;
    private GameObject usableObj;
    private GameObject combineObj;
    private GameObject newSlot;
    private GameObject cursorHandler;
    private GameManager gm;
    private LocalizedString combineText;
    private LocalizedString combineErrorText;

    public void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(20, 20);
        player = GameObject.FindWithTag("Player");
        cursorHandler = GameObject.FindWithTag("Cursor");
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        id = data.id;
        gm = GameManager.Instance;
        data.displayName.StringChanged += UpdateName;
        combineText = new LocalizedString("UI", "item_combine_text");
        combineErrorText = new LocalizedString("UI", "item_combine_error");
        localizedDisplayName = data.displayName;
    }

    public void Update()
    {
        if (combineObj){
            if (Input.GetMouseButtonDown(0))
            {
               if (combineObj.GetComponent<InventoryItem>().id == data.combineWith)
               {
                    Reset();
                    combineObj.GetComponent<InventoryItem>().RemoveItem();
                    inventory.addItem(data.newItem);
                    RemoveItem();
               }

               else
               {
                    //get localized text
                    var dict = new Dictionary<string, object>
                    {
                        { "item1", localizedDisplayName.GetLocalizedString() },
                        { "item2", combineObj.GetComponent<InventoryItem>().localizedDisplayName.GetLocalizedString() }

                    };

                    //error text - potentially switch to assigning in the editor for different sentence options
                    combineErrorText.Arguments = new object[] { dict };
                    gm.SetText(combineErrorText.GetLocalizedString());
                    transform.position = pos;
               }
            }
        }

        if (newSlot && selected && Input.GetMouseButtonDown(0))
        {
            if (newSlot.transform.childCount == 0)
            {
                selected = false;
                transform.parent = newSlot.transform;
                newSlot = null;
                transform.localPosition = Vector2.zero;
            }
            else if (newSlot.transform == transform.parent)
            {
                selected = false;
                newSlot = null;
                transform.localPosition = Vector2.zero;
            }
        }

        if (selected)
        {
            gm.selectedItemID = data.id;
            gm.itemSelected = true;
            player.GetComponent<MoveCharacter>().canMove = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
            gameObject.layer = 2;

            if(Input.GetMouseButtonDown(1)) {
                Reset();
            }
        }
    }

    private void Reset()
    {
        selected = false;
        gm.itemSelected = false;
        gm.selectedItemID = "";
        transform.localPosition = Vector2.zero;
        player.GetComponent<MoveCharacter>().canMove = true;
        gameObject.layer = 0;
    }

    //hovering over another item or inventory slot
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(selected)
        {
            if (collision.gameObject.GetComponent<InventoryItem>())
            {
                //cursorHandler.GetComponent<CursorIcon>().SetUse();
                combineObj = collision.gameObject;
                //get localized text
                var dict = new Dictionary<string, object>
                {
                    { "item1", localizedDisplayName.GetLocalizedString() },
                    { "item2", combineObj.GetComponent<InventoryItem>().localizedDisplayName.GetLocalizedString() }


                };
                combineText.Arguments = new object[] { dict };
                gm.SetActionText(combineText.GetLocalizedString());
            }
        }

        if (collision.gameObject.GetComponent<InventorySlot>())
        {
            if (selected)
            {
                newSlot = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        combineObj = null;
        gm.ResetActionText();
    }

    private void OnMouseDown()
    {
        selected = true;
    }

    public void RemoveItem()
    {
        Reset();
        if (!data.reusable)
        {
            Destroy(gameObject);
            gm.save.data.inventoryItems.Remove(data);
        }
    }

    private void UpdateName(string val)
    {
        displayName = val;
    }
}
