using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public ItemData data;
    public Inventory inventory;
    public bool selected = false;
    private Vector2 pos;
    private GameObject player;
    private GameObject usableObj;
    private GameObject combineObj;
    private GameObject newSlot;
    private GameObject cursorHandler;
    public string id;
    private GameManager gm;

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        GetComponent<SpriteRenderer>().size = new Vector2(20, 20);
        player = GameObject.FindWithTag("Player");
        cursorHandler = GameObject.FindWithTag("Cursor");
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        id = data.id;
        gm = GameManager.Instance;
    }

    public void Update()
    {
        if (usableObj)
        {
            if (Input.GetMouseButtonDown(0) && selected)
            {
                if (usableObj.GetComponent<UsableObject>().usableItemID == data.id)
                {
                    if(usableObj.GetComponent<UsableObject>().locked == true) {
                        usableObj.GetComponent<UsableObject>().Unlock();
                    }
                    else {
                        usableObj.GetComponent<UsableObject>().Action();
                    }
                    if (!data.reusable)
                    {
                        selected = false;
                        RemoveItem();
                        player.GetComponent<MoveCharacter>().canMove = true;
                    }
                }

                else
                {
                    gm.SetText("Ich kann " + data.displayName + " nicht mit " + usableObj.name + " benutzen.");
                    //add english option
                }
            }
        }

        if (combineObj){
            if (Input.GetMouseButtonDown(0))
            {
               if (combineObj.GetComponent<InventoryItem>().id == data.combineWith)
                {
                    selected = false;
                    RemoveItem();
                    Destroy(combineObj);
                    player.GetComponent<MoveCharacter>().canMove = true;
                    inventory.addItem(data.newItem);
                }

                else
                {
                    gm.SetText("Ich kann " + data.displayName + " nicht mit " + combineObj.name + " kombinieren.");
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
            player.GetComponent<MoveCharacter>().canMove = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            if(Input.GetMouseButtonDown(1)) {
                selected = false;
                gm.selectedItemID = "";
                transform.localPosition = Vector2.zero;
                player.GetComponent<MoveCharacter>().canMove = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(selected)
        {
            if (collision.gameObject.GetComponent<UsableObject>())
            {
                cursorHandler.GetComponent<CursorIcon>().SetUse();
                gm.SetText("Use " + data.name + " on " + collision.name);
                usableObj = collision.gameObject;
            }

            if (collision.gameObject.GetComponent<InventoryItem>())
            {
                cursorHandler.GetComponent<CursorIcon>().SetUse();
                gm.SetText(data.name + " mit " + collision.name + " kombinieren");
                combineObj = collision.gameObject;
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
        usableObj = null;
        //if (displayText.text.Contains(collision.name))
        //{
        //    usableObj = null;
        //}

    }
    private void OnMouseDown()
    {
        selected = true;
    }

    private void RemoveItem()
    {
        Destroy(gameObject);
        gm.save.data.inventoryItems.Remove(data);
    }
}
